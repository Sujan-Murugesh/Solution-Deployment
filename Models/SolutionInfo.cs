using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sujan_Solution_Deployer.Models
{
    public class SolutionInfo
    {
        public Guid SolutionId { get; set; }
        public string UniqueName { get; set; }
        public string FriendlyName { get; set; }
        public string Version { get; set; }
        public bool IsManaged { get; set; }
        public DateTime InstalledOn { get; set; }
        public string PublisherName { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{FriendlyName} (v{Version}) - {(IsManaged ? "Managed" : "Unmanaged")}";
        }
    }
}
