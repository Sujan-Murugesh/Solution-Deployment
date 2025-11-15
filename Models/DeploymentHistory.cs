using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sujan_Solution_Deployer.Models
{
    public class DeploymentHistory
    {
        public int Id { get; set; }
        public DateTime DeploymentDate { get; set; }
        public string SolutionUniqueName { get; set; }
        public string SolutionFriendlyName { get; set; }
        public string SourceVersion { get; set; }
        public string TargetVersion { get; set; }
        public string SourceEnvironment { get; set; }
        public string TargetEnvironment { get; set; }
        public bool IsManaged { get; set; }
        public bool DeployedAsManaged { get; set; }
        public DeploymentStatus Status { get; set; }
        public string DeployedBy { get; set; }
        public string ErrorMessage { get; set; }
        public int DurationSeconds { get; set; }
        public bool BackupCreated { get; set; }
        public string BackupPath { get; set; }

        public string StatusDisplay
        {
            get
            {
                switch (Status)
                {
                    case DeploymentStatus.Completed:
                        return "✅ Success";
                    case DeploymentStatus.Failed:
                        return "❌ Failed";
                    case DeploymentStatus.Cancelled:
                        return "⚠️ Cancelled";
                    default:
                        return "⏳ In Progress";
                }
            }
        }


        public string DurationDisplay
        {
            get
            {
                var ts = TimeSpan.FromSeconds(DurationSeconds);
                if (ts.TotalHours >= 1)
                    return $"{ts.Hours}h {ts.Minutes}m";
                else if (ts.TotalMinutes >= 1)
                    return $"{ts.Minutes}m {ts.Seconds}s";
                else
                    return $"{ts.Seconds}s";
            }
        }

        public string Notes { get; set; }
    }
}
