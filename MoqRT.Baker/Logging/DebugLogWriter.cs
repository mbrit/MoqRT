using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Logging
{
    internal class DebugLogWriter : ILogWriter
    {
        public void Write(string message, Exception ex)
        {
            WriteMessage(message, ex);
        }

        internal static void WriteMessage(string message, Exception ex)
        {
            if (ex != null)
                Debug.WriteLine("MoqRT: " + message + " --> " + ex.ToString());
            else
                Debug.WriteLine("MoqRT: " + message);
        }
    }
}
