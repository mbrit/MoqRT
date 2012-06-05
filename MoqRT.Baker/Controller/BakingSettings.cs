using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    [Serializable]
    internal class BakingSettings
    {
        internal string AssemblyPath { get; private set; }
        internal string AppxPath { get; private set; }

        public BakingSettings(string assemblyPath, string appxPath)
        {
            this.AssemblyPath = assemblyPath;
            this.AppxPath = appxPath;
        }

        internal BakingSettings Clone()
        {
            return new BakingSettings(this.AssemblyPath, this.AppxPath);
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
