using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sujan_Solution_Deployer.Models;
using SolutionInfo = Sujan_Solution_Deployer.Models.SolutionInfo;

namespace Sujan_Solution_Deployer.Services
{
    public class SolutionService
    {
        private readonly IOrganizationService _service;

        public SolutionService(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public List<SolutionInfo> GetAllSolutions(bool isManaged)
        {
            try
            {
                var query = new QueryExpression("solution")
                {
                    ColumnSet = new ColumnSet(
                        "solutionid",
                        "uniquename",
                        "friendlyname",
                        "version",
                        "ismanaged",
                        "installedon",
                        "publisherid",
                        "description"
                    ),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("isvisible", ConditionOperator.Equal, true),
                            new ConditionExpression("ismanaged", ConditionOperator.Equal, isManaged)
                        }
                    }
                };

                var solutions = _service.RetrieveMultiple(query);
                var solutionList = new List<SolutionInfo>();

                foreach (var solution in solutions.Entities)
                {
                    try
                    {
                        var publisherId = solution.GetAttributeValue<EntityReference>("publisherid");
                        var publisher = _service.Retrieve("publisher", publisherId.Id, new ColumnSet("friendlyname"));

                        solutionList.Add(new SolutionInfo
                        {
                            SolutionId = solution.Id,
                            UniqueName = solution.GetAttributeValue<string>("uniquename"),
                            FriendlyName = solution.GetAttributeValue<string>("friendlyname"),
                            Version = solution.GetAttributeValue<string>("version"),
                            IsManaged = solution.GetAttributeValue<bool>("ismanaged"),
                            InstalledOn = solution.GetAttributeValue<DateTime>("installedon"),
                            PublisherName = publisher.GetAttributeValue<string>("friendlyname"),
                            Description = solution.Contains("description") ?
                                solution.GetAttributeValue<string>("description") : string.Empty
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error processing solution: {ex.Message}");
                    }
                }

                return solutionList.OrderBy(s => s.FriendlyName).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving solutions: {ex.Message}", ex);
            }
        }

        public byte[] ExportSolution(string solutionUniqueName, bool exportAsManaged, Action<string> progressCallback = null)
        {
            try
            {
                progressCallback?.Invoke($"Preparing to export solution: {solutionUniqueName}");

                var exportRequest = new ExportSolutionRequest
                {
                    SolutionName = solutionUniqueName,
                    Managed = exportAsManaged,
                    ExportAutoNumberingSettings = true,
                    ExportCalendarSettings = true,
                    ExportCustomizationSettings = true,
                    ExportEmailTrackingSettings = true,
                    ExportGeneralSettings = true,
                    ExportMarketingSettings = true,
                    ExportOutlookSynchronizationSettings = true,
                    ExportRelationshipRoles = true,
                    ExportIsvConfig = true,
                    ExportSales = true,
                    ExportExternalApplications = true
                };

                progressCallback?.Invoke($"Exporting solution: {solutionUniqueName}...");
                var response = (ExportSolutionResponse)_service.Execute(exportRequest);
                progressCallback?.Invoke($"Solution exported successfully: {solutionUniqueName}");

                return response.ExportSolutionFile;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting solution '{solutionUniqueName}': {ex.Message}", ex);
            }
        }

        public string BackupSolution(string solutionUniqueName, string backupPath, bool exportAsManaged, Action<string> progressCallback = null)
        {
            try
            {
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                progressCallback?.Invoke($"Backing up solution: {solutionUniqueName}");

                var solutionFile = ExportSolution(solutionUniqueName, exportAsManaged, progressCallback);

                var managedLabel = exportAsManaged ? "managed" : "unmanaged";
                var fileName = $"{solutionUniqueName}_{DateTime.Now:yyyyMMdd_HHmmss}_{managedLabel}.zip";
                var fullPath = Path.Combine(backupPath, fileName);

                File.WriteAllBytes(fullPath, solutionFile);

                progressCallback?.Invoke($"Backup saved: {fullPath}");

                return fullPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error backing up solution '{solutionUniqueName}': {ex.Message}", ex);
            }
        }

        public Guid ImportSolution(
            byte[] solutionFile,
            bool publishWorkflows,
            bool overwriteCustomizations,
            Action<string> progressCallback = null)
        {
            try
            {
                progressCallback?.Invoke("Preparing solution import...");

                var importJobId = Guid.NewGuid();
                var importRequest = new ImportSolutionRequest
                {
                    CustomizationFile = solutionFile,
                    PublishWorkflows = publishWorkflows,
                    OverwriteUnmanagedCustomizations = overwriteCustomizations,
                    ImportJobId = importJobId
                };

                progressCallback?.Invoke("Importing solution...");
                _service.Execute(importRequest);
                progressCallback?.Invoke("Solution import initiated successfully");

                return importJobId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error importing solution: {ex.Message}", ex);
            }
        }

        public (string Status, double Progress, bool IsCompleted) GetImportJobStatus(Guid importJobId)
        {
            try
            {
                var importJob = _service.Retrieve("importjob", importJobId,
                    new ColumnSet("progress", "completedon", "startedon", "data"));

                var isCompleted = importJob.Contains("completedon") &&
                                 importJob.GetAttributeValue<DateTime?>("completedon").HasValue;

                var progress = importJob.Contains("progress") ?
                    importJob.GetAttributeValue<double>("progress") : 0;

                string status;
                if (isCompleted)
                {
                    status = "Completed";
                }
                else if (progress > 0)
                {
                    status = $"In Progress: {progress:F0}%";
                }
                else
                {
                    status = "Queued";
                }

                return (status, progress, isCompleted);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting import job status: {ex.Message}", ex);
            }
        }

        public void PublishAllCustomizations(Action<string> progressCallback = null)
        {
            try
            {
                progressCallback?.Invoke("Publishing all customizations...");

                var request = new PublishAllXmlRequest();
                _service.Execute(request);

                progressCallback?.Invoke("Customizations published successfully");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error publishing customizations: {ex.Message}", ex);
            }
        }

        #region ==> Deployment Histoty
        public SolutionInfo GetSolutionByUniqueName(string uniqueName)
        {
            try
            {
                var query = new QueryExpression("solution")
                {
                    ColumnSet = new ColumnSet(
                        "solutionid",
                        "uniquename",
                        "friendlyname",
                        "version",
                        "ismanaged",
                        "installedon",
                        "publisherid",
                        "description"
                    ),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                {
                    new ConditionExpression("uniquename", ConditionOperator.Equal, uniqueName)
                }
                    }
                };

                var solutions = _service.RetrieveMultiple(query);

                if (solutions.Entities.Count == 0)
                {
                    return null; // Solution doesn't exist
                }

                var solution = solutions.Entities[0];
                var publisherId = solution.GetAttributeValue<EntityReference>("publisherid");
                var publisher = _service.Retrieve("publisher", publisherId.Id, new ColumnSet("friendlyname"));

                return new SolutionInfo
                {
                    SolutionId = solution.Id,
                    UniqueName = solution.GetAttributeValue<string>("uniquename"),
                    FriendlyName = solution.GetAttributeValue<string>("friendlyname"),
                    Version = solution.GetAttributeValue<string>("version"),
                    IsManaged = solution.GetAttributeValue<bool>("ismanaged"),
                    InstalledOn = solution.GetAttributeValue<DateTime>("installedon"),
                    PublisherName = publisher.GetAttributeValue<string>("friendlyname"),
                    Description = solution.Contains("description") ?
                        solution.GetAttributeValue<string>("description") : string.Empty
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving solution '{uniqueName}': {ex.Message}", ex);
            }
        }

        public int CompareVersions(string version1, string version2)
        {
            try
            {
                var v1 = new Version(version1);
                var v2 = new Version(version2);
                return v1.CompareTo(v2);
            }
            catch
            {
                return 0; // If version parsing fails, consider them equal
            }
        }
        public string IncrementVersion(string currentVersion, VersionIncrementType incrementType)
        {
            try
            {
                var version = new Version(currentVersion);

                // ✅ Add debug logging
                System.Diagnostics.Debug.WriteLine($"IncrementVersion called: current={currentVersion}, type={incrementType}");

                string newVersion = currentVersion;

                switch (incrementType)
                {
                    case VersionIncrementType.Major:
                        newVersion = new Version(version.Major + 1, 0, 0, 0).ToString();
                        break;
                    case VersionIncrementType.Minor:
                        newVersion = new Version(version.Major, version.Minor + 1, 0, 0).ToString();
                        break;
                    case VersionIncrementType.Build:
                        newVersion = new Version(version.Major, version.Minor, version.Build + 1, 0).ToString();
                        break;
                    case VersionIncrementType.Revision:
                        newVersion = new Version(version.Major, version.Minor, version.Build, version.Revision + 1).ToString();
                        break;
                    default:
                        newVersion = currentVersion;
                        break;
                }

                System.Diagnostics.Debug.WriteLine($"IncrementVersion result: {newVersion}");
                return newVersion;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"IncrementVersion error: {ex.Message}");
                return currentVersion;
            }
        }

        public enum VersionIncrementType
        {
            Major,
            Minor,
            Build,
            Revision
        }

        public void UpdateSolutionVersion(string solutionUniqueName, string newVersion, Action<string> progressCallback = null)
        {
            try
            {
                progressCallback?.Invoke($"Updating solution version: {solutionUniqueName} to v{newVersion}");

                // Get the solution
                var query = new QueryExpression("solution")
                {
                    ColumnSet = new ColumnSet("solutionid", "uniquename"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                {
                    new ConditionExpression("uniquename", ConditionOperator.Equal, solutionUniqueName),
                    new ConditionExpression("ismanaged", ConditionOperator.Equal, false)
                }
                    }
                };

                var solutions = _service.RetrieveMultiple(query);

                if (solutions.Entities.Count == 0)
                {
                    throw new Exception($"Unmanaged solution '{solutionUniqueName}' not found");
                }

                var solution = solutions.Entities[0];

                // Update the version
                var updateSolution = new Entity("solution")
                {
                    Id = solution.Id
                };
                updateSolution["version"] = newVersion;

                _service.Update(updateSolution);

                progressCallback?.Invoke($"✅ Version updated successfully to v{newVersion}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating solution version: {ex.Message}", ex);
            }
        }
        #endregion
    }
}
