using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Logging
{
    public static class Logger
    {
        private static List<ILogWriter> Writers { get; set; }
        private static object _writersLock = new object();

        static Logger()
        {
            Writers = new List<ILogWriter>();
            Writers.Add(new DebugLogWriter());
        }

        public static void RegisterLogWriter(ILogWriter writer)
        {
            lock(_writersLock)
                Writers.Add(writer);
        }

        public static void UnregisterLogWriter(ILogWriter writer)
        {
            lock (_writersLock)
            {
                int index = Writers.IndexOf(writer);
                if (index != -1)
                    Writers.RemoveAt(index);
            }
        }

        public static void Log(string message, Exception exception = null)
        {
            var writers = GetWriters();
            foreach (var writer in writers)
            {
                try
                {
                    writer.Write(message, exception);
                }
                catch(Exception ex)
                {
                    DebugLogWriter.WriteMessage("Failed to write message.", ex);
                }
            }
        }

        private static List<ILogWriter> GetWriters()
        {
            lock (_writersLock)
            {
                var results = new List<ILogWriter>();
                foreach (var writer in Writers)
                    results.Add(writer);
                return results;
            }
        }
    }
}
