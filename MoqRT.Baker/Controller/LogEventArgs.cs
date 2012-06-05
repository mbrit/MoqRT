using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    public class LogEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }

        internal LogEventArgs(string message, Exception ex)
        {
            this.Message = message;
            this.Exception = ex;
        }
    }
}
