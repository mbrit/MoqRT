using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Logging
{
    // copies log messages from a worker appdomain into the main one...
    internal class PinnedLogDistributor : MarshalByRefObject, ILogWriter
    {
        public void Write(string message, Exception ex)
        {
            Logger.Log(message, ex);
        }
    }
}
