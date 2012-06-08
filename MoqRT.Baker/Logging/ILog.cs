using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Logging
{
    public interface ILog
    {
        void Log(string message);
        void Log(string message, Exception ex);
    }
}
