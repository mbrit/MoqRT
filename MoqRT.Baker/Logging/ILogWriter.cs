﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Logging
{
    public interface ILogWriter
    {
        void Write(string message, Exception ex);
    }
}
