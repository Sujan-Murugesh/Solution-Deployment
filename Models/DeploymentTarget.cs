using McTools.Xrm.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sujan_Solution_Deployer.Models
{
    public class DeploymentTarget
    {
        public string Name { get; set; }
        public ConnectionDetail ConnectionDetail { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
