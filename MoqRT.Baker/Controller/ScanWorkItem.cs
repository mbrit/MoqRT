using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    [Serializable]
    internal class ScanWorkItem : WorkItem
    {
        internal ScanWorkItem(BakingSettings settings, DateTime atOrAfter)  
            : base(settings, atOrAfter)
        {
        }

        internal override void Run(BakingContext context)
        {
            context.Owner.HandleScanningStarted();
            try
            {
                context.Log(string.Format("Scanning test assembly '{0}'...", this.Settings.AssemblyFilename));

                // create...
                var project = new TestProject(context.TestAssembly, context);
                context.Owner.ActiveProject = project;

                //  ok...
                context.Log("Finished scanning test assembly.");
            }
            finally
            {
                context.Owner.HandleScanningFinished();
            }
        }
    }
}
