using System.Security.Cryptography;
using System.Text;

namespace CSDBPortal.Services
{
    /// <summary>
    /// Encrypts XML content for .nav file packaging using the same algorithm as the NavIETM Viewer.
    /// Algorithm: AES-128 CBC, PKCS7 padding.
    /// Key + IV: UTF-16LE bytes of the shared password (matches Viewer implementation exactly).
    /// </summary>
    public static class NavXmlEncryptionService
    {
        // Must match the password used in the NavIETM Viewer application
        private static readonly byte[] Key = new UnicodeEncoding().GetBytes("!@#$%^&*");

        /// <summary>
        /// Encrypts a UTF-8 XML string and returns the encrypted byte array.
        /// </summary>
        public static byte[] EncryptXml(string xmlContent)
        {
            var contentBytes = Encoding.UTF8.GetBytes(xmlContent);
            return EncryptBytes(contentBytes);
        }

        /// <summary>
        /// Encrypts a raw byte array using AES-128 CBC (same as Viewer).
        /// </summary>
        public static byte[] EncryptBytes(byte[] data)
        {
            using var aes = Aes.Create();
            aes.Key     = Key;
            aes.IV      = Key;   // same 16-byte value used as both key and IV — matches Viewer
            aes.Mode    = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
            }
            return ms.ToArray();
        }
    }
}
