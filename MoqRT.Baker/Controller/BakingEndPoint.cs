using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MoqRT.Logging;

namespace MoqRT.Baking
{
    public class BakingEndPoint : MarshalByRefObject
    {
        public const int Port = 6663;
        public const string ServiceUri = "MoqRT.Baking.rem";

        public BakingEndPoint()
        {
        }

        public ManualResetEvent ForceBaking()
        {
            ManualResetEvent e = new ManualResetEvent(false);
            BakingController.Current.ForceBaking(e);
            return e;
        }

        public void RegisterLogWriter(ILogWriter writer)
        {
            Logger.RegisterLogWriter(writer);
        }

        public void UnregisterLogWriter(ILogWriter writer)
        {
            Logger.UnregisterLogWriter(writer);
        }
    }
}
