using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MoqRT.Logging;

namespace MoqRT.Baking
{
    [Serializable]
    internal abstract class WorkItem : ILoggable
    {
        internal BakingSettings Settings { get; private set; }
        internal DateTime AtOrAfter { get; set; }
        internal ManualResetEvent Waiter { get; private set; }

        internal WorkItem(BakingSettings settings, DateTime atOrAfter, ManualResetEvent waiter)
        {
            this.Settings = settings;
            this.AtOrAfter = atOrAfter;
            this.Waiter = waiter;
        }

        internal bool OkToRun
        {
            get
            {
                if (this.AtOrAfter == DateTime.MinValue)
                    return true;
                else
                    return DateTime.Now >= this.AtOrAfter;
            }
        }

        internal abstract void Run(BakingContext context);
    }
}
