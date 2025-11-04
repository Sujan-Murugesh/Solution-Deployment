using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sujan_Solution_Deployer.Models
{
    public class DeploymentProgress
    {
        public string SolutionName { get; set; }
        public string TargetEnvironment { get; set; }
        public DeploymentStatus Status { get; set; }
        public int ProgressPercentage { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public Exception Error { get; set; }

        public DeploymentProgress()
        {
            Timestamp = DateTime.Now;
        }
    }

    public enum DeploymentStatus
    {
        Queued,
        Exporting,
        BackingUp,
        Importing,
        Publishing,
        Completed,
        Failed,
        Cancelled
    }
}
