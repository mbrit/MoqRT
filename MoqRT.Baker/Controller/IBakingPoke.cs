﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    internal interface IBakingPoke
    {
        void RunWorkItem(BakingController owner, WorkItem item);
    }
}
