using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sujan_Solution_Deployer.Services
{
    /// Manages persistent storage of SMTP settings with encrypted password
    public class SmtpSettingsManager
    {
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SujanSolutionDeployer",
            "smtp_settings.xml");

        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("SujanDeployer2024");

        /// SMTP Settings model
        [Serializable]
        public class SmtpSettings
        {
            public string SmtpHost { get; set; } = "smtp.gmail.com";
            public int SmtpPort { get; set; } = 587;
            public bool EnableSsl { get; set; } = true;
            public string SenderEmail { get; set; } = "";
            public string SenderName { get; set; } = "Sujan Solution Deployer";

            // Store encrypted password as Base64 string
            public string EncryptedPassword { get; set; } = "";

            // This property is not serialized
            [XmlIgnore]
            public string Password
            {
                get => DecryptPassword(EncryptedPassword);
                set => EncryptedPassword = EncryptPassword(value);
            }
        }

        /// Save SMTP settings to file
        public static void SaveSettings(SmtpSettings settings)
        {
            try
            {
                // Ensure directory exists
                var directory = Path.GetDirectoryName(SettingsFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Serialize to XML
                var serializer = new XmlSerializer(typeof(SmtpSettings));
                using (var writer = new StreamWriter(SettingsFilePath))
                {
                    serializer.Serialize(writer, settings);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save SMTP settings: {ex.Message}", ex);
            }
        }

        /// Load SMTP settings from file
        public static SmtpSettings LoadSettings()
        {
            try
            {
                if (!File.Exists(SettingsFilePath))
                {
                    return new SmtpSettings(); // Return default settings
                }

                var serializer = new XmlSerializer(typeof(SmtpSettings));
                using (var reader = new StreamReader(SettingsFilePath))
                {
                    return (SmtpSettings)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load SMTP settings: {ex.Message}");
                return new SmtpSettings(); // Return default on error
            }
        }

        /// Check if settings exist
        public static bool SettingsExist()
        {
            return File.Exists(SettingsFilePath);
        }

        /// Delete saved settings
        public static void DeleteSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    File.Delete(SettingsFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete SMTP settings: {ex.Message}", ex);
            }
        }

        /// Encrypt password using Windows DPAPI
        private static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(password);
                byte[] encryptedBytes = ProtectedData.Protect(
                    plainBytes,
                    Entropy,
                    DataProtectionScope.CurrentUser);

                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Encryption error: {ex.Message}");
                return string.Empty;
            }
        }

        /// Decrypt password using Windows DPAPI
        private static string DecryptPassword(string encryptedPassword)
        {
            if (string.IsNullOrEmpty(encryptedPassword))
                return string.Empty;

            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
                byte[] plainBytes = ProtectedData.Unprotect(
                    encryptedBytes,
                    Entropy,
                    DataProtectionScope.CurrentUser);

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Decryption error: {ex.Message}");
                return string.Empty;
            }
        }

        /// Get settings file location for display/troubleshooting
        public static string GetSettingsFilePath()
        {
            return SettingsFilePath;
        }
    }
}
