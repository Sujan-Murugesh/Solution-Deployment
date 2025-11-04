using McTools.Xrm.Connection;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Sujan_Solution_Deployer.Services
{
    public static class ConnectionManager
    {
        /// <summary>
        /// Gets the IOrganizationService from a ConnectionDetail
        /// This works with XrmToolBox's connection management
        /// </summary>
        public static IOrganizationService GetOrganizationService(ConnectionDetail connectionDetail)
        {
            if (connectionDetail == null)
            {
                throw new ArgumentNullException(nameof(connectionDetail));
            }

            try
            {
                // Get the service from the connection detail
                // XrmToolBox maintains the connection, so we just use it
                var service = connectionDetail.ServiceClient?.OrganizationServiceProxy;

                if (service == null)
                {
                    throw new Exception("Unable to get organization service from connection");
                }

                return service;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting service for '{connectionDetail.ConnectionName}': {ex.Message}", ex);
            }
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
