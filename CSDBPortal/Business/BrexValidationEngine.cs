using CSDBPortal.Data;
using CSDBPortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace CSDBPortal.Business
{
    /// <summary>
    /// Validates a generated data-module XML against the active BREX rules for its project.
    /// Supports Discrete (allowed value list), Length (max chars), Range (numeric bounds),
    /// and Nesting (ancestor element) constraint types.
    /// Results are persisted to the XmlValidation table.
    /// </summary>
    public class BrexValidationEngine
    {
        private readonly ApplicationDbContext _db;

        public BrexValidationEngine(ApplicationDbContext db)
        {
            _db = db;
        }

        // ── Public entry point ────────────────────────────────────────────────

        public async Task<(bool Passed, string Message)> ValidateAsync(int dmcId, string userName)
        {
            var dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            if (dmc == null)
                return await FailAsync(dmcId, "Data module not found.", userName);

            if (string.IsNullOrWhiteSpace(dmc.xml))
                return await FailAsync(dmcId, "No XML has been generated for this data module yet.", userName);

            var rules = await _db.BrexRules
                .Where(r => r.ProjectId == dmc.ProjectId && r.IsActive == true && r.XmlTag != null)
                .ToListAsync();

            if (!rules.Any())
            {
                await UpsertAsync(dmcId, true, "No active BREX rules defined for this project.", userName);
                return (true, "No active BREX rules defined for this project.");
            }

            XmlDocument xmlDoc = new XmlDocument();
            try { xmlDoc.LoadXml(dmc.xml); }
            catch (Exception ex)
            {
                return await FailAsync(dmcId, $"Data module XML is malformed: {ex.Message}", userName);
            }

            var xMan = new XmlNamespaceManager(xmlDoc.NameTable);
            var failures = new List<string>();

            foreach (var rule in rules)
            {
                // If the rule targets a specific DM type, skip rules that don't match
                if (!string.IsNullOrWhiteSpace(rule.Dmtype) &&
                    !string.Equals(rule.Dmtype, dmc.InfoName, StringComparison.OrdinalIgnoreCase))
                    continue;

                var (passed, msg) = ApplyRule(xmlDoc, xMan, rule);
                if (!passed)
                    failures.Add(msg);
            }

            bool allPassed = failures.Count == 0;
            string summary = allPassed
                ? "All BREX rules passed."
                : string.Join(" | ", failures);

            await UpsertAsync(dmcId, allPassed, summary, userName);
            return (allPassed, summary);
        }

        // ── Rule dispatch ─────────────────────────────────────────────────────

        private static (bool Passed, string Message) ApplyRule(
            XmlDocument doc, XmlNamespaceManager xMan, BrexRule rule)
        {
            // For Nesting rules the SubXmlTag is the required ancestor, not a child element,
            // so we locate XmlTag directly. For all other types SubXmlTag is an optional
            // child element to narrow the selection.
            bool isNesting = string.Equals(rule.Type, "Nesting", StringComparison.OrdinalIgnoreCase);

            string xpath = (!isNesting && !string.IsNullOrWhiteSpace(rule.SubXmlTag))
                ? $"//{rule.XmlTag}/{rule.SubXmlTag}"
                : $"//{rule.XmlTag}";

            XmlNodeList? nodes;
            try { nodes = doc.SelectNodes(xpath, xMan); }
            catch { return (true, string.Empty); } // Unparseable XPath — skip

            if (nodes == null || nodes.Count == 0)
                return (false, $"[{Label(rule)}] Required element '{xpath}' not found in data module.");

            string target = string.IsNullOrWhiteSpace(rule.AttributeName)
                ? $"<{rule.XmlTag}>"
                : $"<{rule.XmlTag}> @{rule.AttributeName}";

            foreach (XmlNode node in nodes)
            {
                string value = string.IsNullOrWhiteSpace(rule.AttributeName)
                    ? node.InnerText?.Trim() ?? string.Empty
                    : node.Attributes?[rule.AttributeName]?.Value?.Trim() ?? string.Empty;

                var result = (rule.Type ?? string.Empty).ToUpperInvariant() switch
                {
                    "DISCRETE" => CheckDiscrete(value, rule.MatchValue, target, Label(rule)),
                    "LENGTH"   => CheckLength(value, rule.Length, target, Label(rule)),
                    "RANGE"    => CheckRange(value, rule.RangeValue, target, Label(rule)),
                    "NESTING"  => CheckNesting(node, rule.SubXmlTag, Label(rule)),
                    _          => (Passed: true, Message: string.Empty)
                };

                if (!result.Passed) return result;
            }

            return (true, string.Empty);
        }

        // ── Constraint checkers ───────────────────────────────────────────────

        private static (bool Passed, string Message) CheckDiscrete(
            string value, string? matchValue, string target, string label)
        {
            if (string.IsNullOrWhiteSpace(matchValue)) return (true, string.Empty);
            var allowed = matchValue.Split(',')
                .Select(v => v.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            return allowed.Contains(value)
                ? (true, string.Empty)
                : (false, $"[{label}] {target} value '{value}' not in allowed set: {matchValue}");
        }

        private static (bool Passed, string Message) CheckLength(
            string value, int? maxLength, string target, string label)
        {
            if (maxLength == null) return (true, string.Empty);
            return value.Length <= maxLength.Value
                ? (true, string.Empty)
                : (false, $"[{label}] {target} length {value.Length} exceeds maximum {maxLength}");
        }

        private static (bool Passed, string Message) CheckRange(
            string value, string? rangeValue, string target, string label)
        {
            if (string.IsNullOrWhiteSpace(rangeValue)) return (true, string.Empty);

            var parts = rangeValue.Split('-');
            if (parts.Length != 2
                || !double.TryParse(parts[0].Trim(), out double min)
                || !double.TryParse(parts[1].Trim(), out double max))
                return (true, string.Empty); // Unparseable range — skip

            if (!double.TryParse(value, out double num))
                return (false, $"[{label}] {target} value '{value}' is not numeric (range check)");

            return num >= min && num <= max
                ? (true, string.Empty)
                : (false, $"[{label}] {target} value {num} is outside allowed range {min}–{max}");
        }

        private static (bool Passed, string Message) CheckNesting(
            XmlNode node, string? requiredAncestor, string label)
        {
            if (string.IsNullOrWhiteSpace(requiredAncestor)) return (true, string.Empty);

            XmlNode? cursor = node.ParentNode;
            while (cursor != null && cursor.NodeType != XmlNodeType.Document)
            {
                if (string.Equals(cursor.LocalName, requiredAncestor, StringComparison.OrdinalIgnoreCase))
                    return (true, string.Empty);
                cursor = cursor.ParentNode;
            }
            return (false, $"[{label}] <{node.LocalName}> must be nested within <{requiredAncestor}>");
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static string Label(BrexRule rule) =>
            !string.IsNullOrWhiteSpace(rule.RuleName) ? rule.RuleName
            : !string.IsNullOrWhiteSpace(rule.Group)  ? rule.Group
            : $"Rule {rule.Id}";

        private async Task<(bool, string)> FailAsync(int dmcId, string message, string userName)
        {
            if (dmcId > 0) await UpsertAsync(dmcId, false, message, userName);
            return (false, message);
        }

        private async Task UpsertAsync(int dmcId, bool passed, string message, string userName)
        {
            var existing = await _db.XmlValidations.FirstOrDefaultAsync(v => v.DataModuleId == dmcId);
            if (existing != null)
            {
                existing.UploadStatus = passed;
                existing.Message      = message;
                existing.UploadedBy   = userName;
                existing.UploadedOn   = DateTime.UtcNow;
            }
            else
            {
                _db.XmlValidations.Add(new XmlValidation
                {
                    DataModuleId = dmcId,
                    UploadStatus = passed,
                    Message      = message,
                    UploadedBy   = userName,
                    UploadedOn   = DateTime.UtcNow
                });
            }
            await _db.SaveChangesAsync();
        }
    }
}
