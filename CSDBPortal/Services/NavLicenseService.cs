using CSDBPortal.Data;
using CSDBPortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CSDBPortal.Services
{
    /// <summary>
    /// Handles License.lic generation and IETP_License DB records for .nav publishing.
    ///
    /// License.lic binary format (matches NavIETM Viewer exactly):
    ///   [encrypted trial flag]           — always present (Unsecured and Secured)
    ///   [encrypted provider/client key]  — Secured packages only
    ///
    /// Encryption: same RijndaelManaged/AES-128 CBC used for XML files.
    /// The Viewer distinguishes Secured from Unsecured by file size:
    ///   Unsecured → file size ≤ 16 bytes (trial flag block only)
    ///   Secured   → file size  > 16 bytes (trial flag + client key blocks)
    /// </summary>
    public class NavLicenseService
    {
        private static readonly byte[] Key = new UnicodeEncoding().GetBytes("!@#$%^&*");

        public const string ProviderKeySettingName = "NavPublishProviderKey";

        private readonly ApplicationDbContext _db;

        public NavLicenseService(ApplicationDbContext db)
        {
            _db = db;
        }

        // ── Public API ───────────────────────────────────────────────────────

        /// <summary>
        /// Returns the portal's ProviderKey from ApplicationSettings, creating one
        /// the first time this is called.
        /// </summary>
        public async Task<string> GetOrCreateProviderKeyAsync()
        {
            var setting = await _db.ApplicationSettings
                .FirstOrDefaultAsync(s => s.Key == ProviderKeySettingName);

            if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
                return setting.Value;

            // First-time: generate a secure random Base64 key (32 bytes → 44-char string)
            var keyBytes = new byte[32];
            RandomNumberGenerator.Fill(keyBytes);
            var newKey = Convert.ToBase64String(keyBytes);

            if (setting == null)
            {
                _db.ApplicationSettings.Add(new ApplicationSetting
                {
                    Key   = ProviderKeySettingName,
                    Value = newKey
                });
            }
            else
            {
                setting.Value = newKey;
            }

            await _db.SaveChangesAsync();
            return newKey;
        }

        /// <summary>
        /// Replaces the portal's ProviderKey with a newly generated one and persists it.
        /// All previously-published Secured packages will stop validating against this key.
        /// </summary>
        public async Task<string> RegenerateProviderKeyAsync()
        {
            var keyBytes = new byte[32];
            RandomNumberGenerator.Fill(keyBytes);
            var newKey = Convert.ToBase64String(keyBytes);

            var setting = await _db.ApplicationSettings
                .FirstOrDefaultAsync(s => s.Key == ProviderKeySettingName);

            if (setting == null)
            {
                _db.ApplicationSettings.Add(new ApplicationSetting
                {
                    Key   = ProviderKeySettingName,
                    Value = newKey
                });
            }
            else
            {
                setting.Value = newKey;
            }

            await _db.SaveChangesAsync();
            return newKey;
        }

        /// <summary>
        /// Generates the binary content for License.lic.
        /// </summary>
        /// <param name="isDraft">True → trial flag = 1 (viewer shows watermarks)</param>
        /// <param name="providerKey">Non-null → Secured package; null → Unsecured</param>
        public byte[] GenerateLicenseFile(bool isDraft, string? providerKey)
        {
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);

            // Block 1: trial flag (always written)
            writer.Write(EncryptString(isDraft ? "1" : "0"));

            // Block 2: client key (Secured packages only)
            if (!string.IsNullOrWhiteSpace(providerKey))
                writer.Write(EncryptString(providerKey));

            return ms.ToArray();
        }

        /// <summary>
        /// Inserts or updates the IETP_License record for the published .nav file.
        /// </summary>
        public async Task UpsertLicenseRecordAsync(
            string navFileName, bool isSecured, bool isDraft, string? providerKey)
        {
            byte[]? clientKeyBytes = isSecured && !string.IsNullOrWhiteSpace(providerKey)
                ? Encoding.UTF8.GetBytes(providerKey)
                : null;

            var existing = await _db.IetpLicenses
                .FirstOrDefaultAsync(l => l.IETP == navFileName);

            if (existing != null)
            {
                existing.IsSecured    = isSecured;
                existing.ClientKey    = clientKeyBytes;
                existing.IsTrial      = isDraft;
                existing.LicenseKey   = null;          // reset; set by Viewer admin after activation
                existing.CreationTime = DateTime.UtcNow;
            }
            else
            {
                _db.IetpLicenses.Add(new IetpLicense
                {
                    IETP         = navFileName,
                    IsSecured    = isSecured,
                    ClientKey    = clientKeyBytes,
                    LicenseKey   = null,
                    IsTrial      = isDraft,
                    CreationTime = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync();
        }

        // ── Private helpers ──────────────────────────────────────────────────

        /// <summary>
        /// Encrypts a string value using AES-128 CBC — matches the Viewer's CryptoStream/StreamWriter
        /// pattern so the Viewer can decrypt each block independently.
        /// </summary>
        private static byte[] EncryptString(string value)
        {
            using var aes = Aes.Create();
            aes.Key     = Key;
            aes.IV      = Key;
            aes.Mode    = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(value);          // StreamWriter.Write(string) — matches CSDBLite exactly
            }
            return ms.ToArray();
        }
    }
}
