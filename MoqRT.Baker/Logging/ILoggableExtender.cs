using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Logging
{
    public static class ILoggableExtender
    {
        public static void Log(this ILoggable loggable, string message, Exception exception = null)
        {
            Logger.Log(message, exception);
        }
    }
}
