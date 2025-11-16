using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sujan_Solution_Deployer.Services
{
    public class EmailSettings
    {
        public bool EmailNotificationEnabled { get; set; }
        public string NotificationEmail { get; set; }
    }

    public static class EmailSettingsManager
    {
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SujanSolutionDeployer",
            "EmailSettings.xml");

        public static void SaveSettings(EmailSettings settings)
        {
            try
            {
                var directory = Path.GetDirectoryName(SettingsPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var serializer = new XmlSerializer(typeof(EmailSettings));
                using (var writer = new StreamWriter(SettingsPath))
                {
                    serializer.Serialize(writer, settings);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving email settings: {ex.Message}");
            }
        }

        public static EmailSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var serializer = new XmlSerializer(typeof(EmailSettings));
                    using (var reader = new StreamReader(SettingsPath))
                    {
                        return (EmailSettings)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading email settings: {ex.Message}");
            }

            return new EmailSettings();
        }

        public static bool SettingsExist()
        {
            return File.Exists(SettingsPath);
        }
    }
}
