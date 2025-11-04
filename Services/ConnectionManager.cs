using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Reflection;

namespace Sujan_Solution_Deployer.Services
{
    public static class ConnectionManager
    {
        /// <summary>
        /// Gets the IOrganizationService from a ConnectionDetail
        /// </summary>
        public static IOrganizationService GetOrganizationService(ConnectionDetail connectionDetail)
        {
            if (connectionDetail == null)
            {
                throw new ArgumentNullException(nameof(connectionDetail));
            }

            try
            {
                // Try to use existing service client if available
                if (connectionDetail.ServiceClient != null && connectionDetail.ServiceClient.IsReady)
                {
                    return connectionDetail.ServiceClient.OrganizationServiceProxy;
                }

                // Create new connection using connection string
                string connectionString = BuildConnectionString(connectionDetail);

                var serviceClient = new CrmServiceClient(connectionString);

                if (!serviceClient.IsReady)
                {
                    throw new Exception($"Failed to connect: {serviceClient.LastCrmError}");
                }

                return serviceClient.OrganizationServiceProxy;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting service for '{connectionDetail.ConnectionName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a CrmServiceClient from ConnectionDetail
        /// </summary>
        public static CrmServiceClient CreateServiceClient(ConnectionDetail connectionDetail)
        {
            if (connectionDetail == null)
            {
                throw new ArgumentNullException(nameof(connectionDetail));
            }

            try
            {
                string connectionString = BuildConnectionString(connectionDetail);
                var serviceClient = new CrmServiceClient(connectionString);

                if (!serviceClient.IsReady)
                {
                    throw new Exception($"Failed to connect: {serviceClient.LastCrmError}");
                }

                return serviceClient;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating service client for '{connectionDetail.ConnectionName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Builds a connection string from ConnectionDetail using reflection for compatibility
        /// </summary>
        private static string BuildConnectionString(ConnectionDetail connectionDetail)
        {
            var connectionType = connectionDetail.GetType();

            // Get authentication type
            bool useOnline = GetPropertyValue<bool>(connectionDetail, "UseOnline", false);
            bool useOAuth = GetPropertyValue<bool>(connectionDetail, "UseOAuth", false);
            bool useIfd = GetPropertyValue<bool>(connectionDetail, "UseIfd", false);

            // Get credentials
            string password = GetPassword(connectionDetail);

            // Build appropriate connection string
            if (useOnline || useOAuth || connectionDetail.OrganizationServiceUrl.Contains("dynamics.com"))
            {
                return BuildOnlineConnectionString(connectionDetail, password);
            }
            else if (useIfd)
            {
                return BuildIfdConnectionString(connectionDetail, password);
            }
            else
            {
                return BuildOnPremiseConnectionString(connectionDetail, password);
            }
        }

        private static string GetPassword(ConnectionDetail connectionDetail)
        {
            // Try different property names for password
            var password = GetPropertyValue<string>(connectionDetail, "SavedPassword", null);
            if (string.IsNullOrEmpty(password))
            {
                password = GetPropertyValue<string>(connectionDetail, "UserPassword", null);
            }
            return password ?? string.Empty;
        }

        private static T GetPropertyValue<T>(object obj, string propertyName, T defaultValue)
        {
            try
            {
                var property = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (property != null && property.CanRead)
                {
                    var value = property.GetValue(obj);
                    if (value != null)
                    {
                        return (T)value;
                    }
                }
            }
            catch
            {
                // Property doesn't exist or can't be read
            }
            return defaultValue;
        }

        private static string BuildOnlineConnectionString(ConnectionDetail connectionDetail, string password)
        {
            var connString = $"AuthType=OAuth;" +
                           $"Url={connectionDetail.OrganizationServiceUrl};" +
                           $"Username={connectionDetail.UserName};";

            // Add password if available
            if (!string.IsNullOrEmpty(password))
            {
                connString += $"Password={password};";
            }

            // Add OAuth parameters
            connString += $"AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;" +
                         $"RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;" +
                         $"LoginPrompt=Auto;" +
                         $"RequireNewInstance=True;";

            return connString;
        }

        private static string BuildIfdConnectionString(ConnectionDetail connectionDetail, string password)
        {
            var homeRealmUri = GetPropertyValue<string>(connectionDetail, "HomeRealmUrl", string.Empty);

            var connString = $"AuthType=IFD;" +
                           $"Url={connectionDetail.OrganizationServiceUrl};";

            if (!string.IsNullOrEmpty(homeRealmUri))
            {
                connString += $"HomeRealmUri={homeRealmUri};";
            }

            // Add domain if available
            if (!string.IsNullOrEmpty(connectionDetail.UserDomain))
            {
                connString += $"Domain={connectionDetail.UserDomain};";
            }

            connString += $"Username={connectionDetail.UserName};";

            // Add password if available
            if (!string.IsNullOrEmpty(password))
            {
                connString += $"Password={password};";
            }

            return connString;
        }

        private static string BuildOnPremiseConnectionString(ConnectionDetail connectionDetail, string password)
        {
            var connString = $"AuthType=AD;" +
                           $"Url={connectionDetail.OrganizationServiceUrl};";

            // Add domain if available
            if (!string.IsNullOrEmpty(connectionDetail.UserDomain))
            {
                connString += $"Domain={connectionDetail.UserDomain};";
            }

            connString += $"Username={connectionDetail.UserName};";

            // Add password if available
            if (!string.IsNullOrEmpty(password))
            {
                connString += $"Password={password};";
            }

            return connString;
        }

        /// <summary>
        /// Test if connection is valid
        /// </summary>
        public static bool TestConnection(ConnectionDetail connectionDetail, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var service = GetOrganizationService(connectionDetail);

                // Execute WhoAmI to verify connection
                var whoAmI = (Microsoft.Crm.Sdk.Messages.WhoAmIResponse)service.Execute(
                    new Microsoft.Crm.Sdk.Messages.WhoAmIRequest());

                return whoAmI.UserId != Guid.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
