using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    internal class WorkItem
    {
        internal BakingSettings Settings { get; private set; }
        internal DateTime AtOrAfter { get; set; }

        internal WorkItem(BakingSettings settings, DateTime atOrAfter)
        {
            this.Settings = settings;
            this.AtOrAfter = atOrAfter;
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

    }
}
