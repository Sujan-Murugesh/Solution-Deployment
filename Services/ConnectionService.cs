using McTools.Xrm.Connection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Sujan_Solution_Deployer.Services
{
    public class ConnectionService
    {
        private List<ConnectionDetail> connections;
        private readonly string connectionsFilePath;

        public ConnectionService()
        {
            // XrmToolBox stores connections in AppData
            var xrmToolBoxPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MscrmTools",
                "XrmToolBox",
                "Connections");

            connectionsFilePath = Path.Combine(xrmToolBoxPath, "MscrmTools.ConnectionsList.xml");
            LoadConnections();
        }

        /// <summary>
        /// Load all connections from XrmToolBox connection file
        /// </summary>
        private void LoadConnections()
        {
            connections = new List<ConnectionDetail>();

            try
            {
                if (File.Exists(connectionsFilePath))
                {
                    var serializer = new XmlSerializer(typeof(CrmConnections));
                    using (var reader = new StreamReader(connectionsFilePath))
                    {
                        var crmConnections = (CrmConnections)serializer.Deserialize(reader);
                        if (crmConnections?.Connections != null)
                        {
                            connections = crmConnections.Connections;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading connections: {ex.Message}");
                // If we can't load from XrmToolBox, start with empty list
                connections = new List<ConnectionDetail>();
            }
        }

        /// <summary>
        /// Get all available connections
        /// </summary>
        public List<ConnectionDetail> GetAllConnections()
        {
            return connections?.ToList() ?? new List<ConnectionDetail>();
        }

        /// <summary>
        /// Get connection by name (case-insensitive)
        /// </summary>
        public ConnectionDetail GetConnectionByName(string connectionName)
        {
            if (string.IsNullOrEmpty(connectionName))
                return null;

            return connections?.FirstOrDefault(c =>
                c.ConnectionName.Equals(connectionName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get connection by organization URL
        /// </summary>
        public ConnectionDetail GetConnectionByUrl(string organizationUrl)
        {
            if (string.IsNullOrEmpty(organizationUrl))
                return null;

            return connections?.FirstOrDefault(c =>
                c.OrganizationServiceUrl.Equals(organizationUrl, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Search connections by partial name match
        /// </summary>
        public List<ConnectionDetail> SearchConnections(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAllConnections();

            return connections?.Where(c =>
                c.ConnectionName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                c.OrganizationFriendlyName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList() ?? new List<ConnectionDetail>();
        }

        /// <summary>
        /// Get connections for a specific environment type
        /// </summary>
        public List<ConnectionDetail> GetConnectionsByEnvironmentType(EnvironmentType environmentType)
        {
            // This is a simple implementation - you might want to enhance it
            // based on your naming conventions or add custom metadata
            return connections?.Where(c =>
            {
                var name = c.ConnectionName.ToLower();
                switch (environmentType)
                {
                    case EnvironmentType.Development:
                        return name.Contains("dev") || name.Contains("development");
                    case EnvironmentType.Test:
                        return name.Contains("test") || name.Contains("tst") || name.Contains("qa");
                    case EnvironmentType.UAT:
                        return name.Contains("uat") || name.Contains("staging");
                    case EnvironmentType.Production:
                        return name.Contains("prod") || name.Contains("production");
                    default:
                        return true;
                }
            }).ToList() ?? new List<ConnectionDetail>();
        }

        /// <summary>
        /// Refresh connections from file
        /// </summary>
        public void RefreshConnections()
        {
            LoadConnections();
        }

        /// <summary>
        /// Get connection count
        /// </summary>
        public int GetConnectionCount()
        {
            return connections?.Count ?? 0;
        }

        /// <summary>
        /// Check if any connections are available
        /// </summary>
        public bool HasConnections()
        {
            return connections != null && connections.Count > 0;
        }
    }

    /// <summary>
    /// Environment types for categorizing connections
    /// </summary>
    public enum EnvironmentType
    {
        Development,
        Test,
        UAT,
        Production,
        Other
    }

    /// <summary>
    /// Container class for XML serialization of XrmToolBox connections
    /// </summary>
    [XmlRoot("CrmConnections")]
    public class CrmConnections
    {
        [XmlArray("Connections")]
        [XmlArrayItem("ConnectionDetail")]
        public List<ConnectionDetail> Connections { get; set; }
    }
}
