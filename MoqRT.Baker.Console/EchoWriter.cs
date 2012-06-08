using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoqRT.Logging;

namespace MoqRT
{
    internal class EchoWriter : MarshalByRefObject, ILogWriter
    {
        public void Write(string message, Exception ex)
        {
            if (ex != null)
                Console.WriteLine(message + " --> " + ex.ToString());
            else
                Console.WriteLine(message);
        }
    }
}
