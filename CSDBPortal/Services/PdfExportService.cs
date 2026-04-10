using CSDBPortal.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace CSDBPortal.Services
{
    /// <summary>
    /// Generates a PDF from a project's data modules using Apache FOP.
    ///
    /// Pipeline:
    ///   DB XML strings → temp directory → XSLT (→ XSL-FO) → Apache FOP (Java) → PDF bytes
    /// </summary>
    public static class PdfExportService
    {
        // ── Public entry point ────────────────────────────────────────────────

        /// <summary>
        /// Generates a PDF for the given project and returns its raw bytes.
        /// </summary>
        public static async Task<byte[]> GeneratePdfAsync(
            Project project,
            List<DataModuleCode> dataModules,
            bool isDraft)
        {
            var tempDir = Path.Combine(Path.GetTempPath(), $"csdb_pdf_{Guid.NewGuid():N}");
            Directory.CreateDirectory(tempDir);

            try
            {
                // 1. Write each data-module XML to the temp directory
                foreach (var dm in dataModules)
                {
                    if (string.IsNullOrWhiteSpace(dm.xml)) continue;
                    var content = isDraft ? NavWatermarkService.WatermarkXml(dm.xml) : dm.xml;
                    File.WriteAllText(Path.Combine(tempDir, $"{dm.DMC}.xml"), content, Encoding.UTF8);
                }

                // 2. Write navigation.xml (plain — FOP reads it directly)
                var navXml  = BuildNavigationXml(project, dataModules);
                var navPath = Path.Combine(tempDir, "navigation.xml");
                File.WriteAllText(navPath, navXml, Encoding.UTF8);

                // 3. Copy admonishment images (Warning / Note / Caution)
                CopyAdmonishmentImages(tempDir);

                // 4. Locate XSLT and JAR resources
                var appDir  = AppContext.BaseDirectory;
                var xsltPath = Path.Combine(appDir, "Resources", "Print", "print_all_pages.xslt");
                var jarDir   = Path.Combine(appDir, "Resources", "Jars");

                if (!File.Exists(xsltPath))
                    throw new FileNotFoundException($"XSLT not found: {xsltPath}");
                if (!Directory.Exists(jarDir))
                    throw new DirectoryNotFoundException($"JAR directory not found: {jarDir}");

                // 5. XSLT transform: navigation.xml → XSL-FO
                var foPath = Path.Combine(tempDir, "output.fo");
                RunXsltTransform(navPath, xsltPath, tempDir, foPath);

                // Fix two common FO artefacts before handing off to FOP
                var foText = File.ReadAllText(foPath)
                    .Replace("text-align=\"\"", "")
                    .Replace("fo:basic-link internal-destination=\"\"",
                             "fo:basic-link internal-destination=\"-\"");
                File.WriteAllText(foPath, foText);

                // 6. Apache FOP: XSL-FO → PDF
                var pdfPath = Path.Combine(tempDir, "output.pdf");
                await RunFopAsync(foPath, pdfPath, jarDir, tempDir);

                return File.ReadAllBytes(pdfPath);
            }
            finally
            {
                try { Directory.Delete(tempDir, recursive: true); } catch { /* best-effort */ }
            }
        }

        // ── XSLT transform ────────────────────────────────────────────────────

        private static void RunXsltTransform(
            string navPath, string xsltPath, string tempDir, string foPath)
        {
            var xslt = new XslCompiledTransform();
            using (var xsltReader = XmlReader.Create(xsltPath))
                xslt.Load(xsltReader, new XsltSettings(true, true), new XmlUrlResolver());

            var args = new XsltArgumentList();
            args.AddExtensionObject("com.delos.model.Processing", new XsltProcessingExtension());
            args.AddParam("path",     "", tempDir + Path.DirectorySeparatorChar);
            args.AddParam("logopath", "", tempDir + Path.DirectorySeparatorChar);
            args.AddParam("color",    "", "black");
            args.AddParam("size",     "", "12");

            var readerSettings = new XmlReaderSettings
            {
                DtdProcessing   = DtdProcessing.Parse,
                XmlResolver     = new XmlUrlResolver(),
                ValidationType  = ValidationType.None
            };

            var writerSettings = new XmlWriterSettings
            {
                Indent               = false,
                OmitXmlDeclaration   = false,
                Encoding             = Encoding.UTF8
            };

            using var foStream = new MemoryStream();
            using (var xmlReader = XmlReader.Create(navPath, readerSettings))
            using (var xmlWriter = XmlWriter.Create(foStream, writerSettings))
                xslt.Transform(xmlReader, args, xmlWriter, new XmlUrlResolver());

            File.WriteAllBytes(foPath, foStream.ToArray());
        }

        // ── Apache FOP invocation ─────────────────────────────────────────────

        private static async Task RunFopAsync(
            string foPath, string pdfPath, string jarDir, string workingDir)
        {
            var javaExe = FindJavaExecutable();
            if (string.IsNullOrEmpty(javaExe))
                throw new FileNotFoundException(
                    "Java runtime not found. Install Java 8+ and ensure it is on the PATH, " +
                    "or set the JAVA_HOME environment variable.");

            // Build explicit classpath from all JARs (avoids wildcard shell-expansion issues)
            var sep = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ";" : ":";
            var jars = Directory.GetFiles(jarDir, "*.jar");
            var classpath = string.Join(sep, jars);

            // FOP flags:  -r = relaxed validation  -q = quiet
            var arguments =
                $"-cp \"{classpath}\" org.apache.fop.cli.Main -r -q " +
                $"-fo \"{foPath}\" -pdf \"{pdfPath}\"";

            var output = new StringBuilder();
            var errors = new StringBuilder();

            using var proc = new Process();
            proc.StartInfo = new ProcessStartInfo
            {
                FileName               = javaExe,
                Arguments              = arguments,
                WorkingDirectory       = workingDir,
                UseShellExecute        = false,
                CreateNoWindow         = true,
                RedirectStandardOutput = true,
                RedirectStandardError  = true
            };

            proc.OutputDataReceived += (_, e) => { if (e.Data != null) output.AppendLine(e.Data); };
            proc.ErrorDataReceived  += (_, e) => { if (e.Data != null) errors.AppendLine(e.Data); };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            // Allow up to 3 minutes for FOP to finish
            var completed = await Task.Run(() => proc.WaitForExit(180_000));
            if (!completed)
            {
                proc.Kill(entireProcessTree: true);
                throw new TimeoutException("Apache FOP did not complete within the 3-minute timeout.");
            }

            if (!File.Exists(pdfPath))
                throw new Exception(
                    $"FOP did not produce a PDF.\n\nFOP output:\n{errors}");
        }

        // ── Java discovery ────────────────────────────────────────────────────

        private static string FindJavaExecutable()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var javaName  = isWindows ? "java.exe" : "java";

            // 1. JAVA_HOME
            var javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrWhiteSpace(javaHome))
            {
                var candidate = Path.Combine(javaHome, "bin", javaName);
                if (File.Exists(candidate)) return candidate;
            }

            // 2. System PATH: `where` (Windows) / `which` (Unix)
            try
            {
                var whichCmd = isWindows ? "where" : "which";
                using var proc = Process.Start(new ProcessStartInfo
                {
                    FileName               = whichCmd,
                    Arguments              = javaName,
                    RedirectStandardOutput = true,
                    UseShellExecute        = false,
                    CreateNoWindow         = true
                });
                var line = proc!.StandardOutput.ReadLine();
                proc.WaitForExit();
                if (!string.IsNullOrWhiteSpace(line) && File.Exists(line.Trim()))
                    return line.Trim();
            }
            catch { /* PATH search unavailable */ }

            // 3. Common Unix paths
            if (!isWindows)
            {
                foreach (var p in new[] { "/usr/bin/java", "/usr/local/bin/java", "/opt/java/bin/java" })
                    if (File.Exists(p)) return p;
            }

            return null;
        }

        // ── Navigation XML builder ────────────────────────────────────────────

        /// <summary>
        /// Builds the navigation.xml siteMap from the project's data modules.
        /// Produces plain (unencrypted) XML — FOP reads it directly.
        /// </summary>
        private static string BuildNavigationXml(Project project, List<DataModuleCode> modules)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<!--Arbortext, Inc., 1988-2018, v.4002-->");
            sb.AppendLine("<siteMap>");

            var rootTitle = System.Security.SecurityElement.Escape(
                project.Title ?? project.Name ?? "IETP");
            var firstUrl = modules.Any()
                ? $"{modules[0].DMC}.xml"
                : "#";
            sb.AppendLine($"    <siteMapNode title=\"{rootTitle}\" url=\"{firstUrl}\">");

            foreach (var dm in modules)
            {
                if (string.IsNullOrWhiteSpace(dm.DMC)) continue;
                var title = System.Security.SecurityElement.Escape(
                    !string.IsNullOrWhiteSpace(dm.TechName) ? dm.TechName :
                    !string.IsNullOrWhiteSpace(dm.InfoName) ? dm.InfoName :
                    dm.DMC);
                sb.AppendLine($"        <siteMapNode title=\"{title}\" url=\"{dm.DMC}.xml\" />");
            }

            sb.AppendLine("    </siteMapNode>");
            sb.AppendLine("</siteMap>");
            return sb.ToString();
        }

        // ── Image helpers ─────────────────────────────────────────────────────

        private static void CopyAdmonishmentImages(string destDir)
        {
            var appDir    = AppContext.BaseDirectory;
            var imagesDir = Path.Combine(appDir, "Resources", "Images");
            if (!Directory.Exists(imagesDir)) return;

            foreach (var name in new[] { "Warning.png", "Note.png", "Caution.png", "Background.jpg" })
            {
                var src = Path.Combine(imagesDir, name);
                if (File.Exists(src))
                    File.Copy(src, Path.Combine(destDir, name), overwrite: true);
            }
        }
    }
}
