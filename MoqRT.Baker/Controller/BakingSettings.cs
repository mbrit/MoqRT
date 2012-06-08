using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    [Serializable]
    public class BakingSettings
    {
        internal string AssemblyPath { get; private set; }
        internal string AppxPath { get; private set; }
        internal string BakingPath { get; private set; }
        internal string PackageId { get; private set; }

        public BakingSettings(string assemblyPath, string appxPath, string bakingPath, string packageID)
        {
            this.AssemblyPath = assemblyPath;
            this.AppxPath = appxPath;
            this.BakingPath = bakingPath;
            this.PackageId = packageID;
        }

        internal BakingSettings Clone()
        {
            return new BakingSettings(this.AssemblyPath, this.AppxPath, this.BakingPath, this.PackageId);
        }

        internal string AssemblyFolder
        {
            get
            {
                return Path.GetDirectoryName(this.AssemblyPath);
            }
        }

        internal string AssemblyFilename
        {
            get
            {
                return Path.GetFileName(this.AssemblyPath);
            }
        }
    }
}
