using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MoqRT.Baking;
using MoqRT.Logging;

namespace MoqRT.Baker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // log...
                var writer = new EchoWriter();
                Logger.RegisterLogWriter(writer);
                Logger.Log(string.Format("MoqRT Baking Instructor - v{0}", typeof(BakingEndPoint).Assembly.GetName().Version));

                // tcp...
                var client = new TcpClientChannel();
                string uri = string.Format("tcp://{0}:{1}/{2}",
                         Environment.MachineName, BakingEndPoint.Port, BakingEndPoint.ServiceUri);
                BakingEndPoint ep = (BakingEndPoint)Activator.GetObject(typeof(BakingEndPoint), uri);
                ManualResetEvent e = null;
                try
                {
                }
                catch (SocketException ex)
                {
                    Logger.Log(ex.Message);
                    Logger.Log("-----------------------");
                    Logger.Log(string.Format("Failed to connect to URI '{0}'. Check that the MoqRT Baker client is running.", uri));

                    // return...
                    Environment.ExitCode = 2;
                    return;
                }

                // log...
//                ep.RegisterLogWriter(writer);
                try
                {
                    e = ep.ForceBaking();

                    // wait...
                    Logger.Log("Now waiting for baking...");
                    e.WaitOne();
                }
                finally
                {
  //                  ep.UnregisterLogWriter(writer);
                }

                // o...
                Logger.Log("All done.");
            }
            catch(Exception ex)
            {
                Logger.Log("Fatal error.", ex);
                Environment.ExitCode = 1;
            }
        }
    }
}
