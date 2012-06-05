using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    public class BakingController : MarshalByRefObject
    {
        public bool IsRunning { get; private set; }
        private BakingSettings LastSettings { get; set; }
        private Task BakingTask { get; set; }
        private BakingRunner Runner { get; set; }
        private FileSystemWatcher Watcher { get; set; }

        public event EventHandler<LogEventArgs> LogMessage;
        public event EventHandler BakingStarted;
        public event EventHandler BakingFinished;

        public BakingController()
        {
            this.Runner = new BakingRunner(this);
        }

        public void Run(string assemblyPath, string appxPath, string bakingPath)
        {
            if (this.IsRunning)
                throw new InvalidOperationException("Already running.");

            // if...
            DisposeWatcher();
            this.Watcher = new FileSystemWatcher(Path.GetDirectoryName(assemblyPath), Path.GetFileName(assemblyPath));
            this.Watcher.Changed += Watcher_Changed;
            this.Watcher.EnableRaisingEvents = true;

            // create...
            this.Log(string.Format("Starting monitoring of '{0}'...", assemblyPath));

            // run...
            this.LastSettings = new BakingSettings(assemblyPath, appxPath, bakingPath);
            this.Runner.Enqueue(this.LastSettings);

            // ok...
            this.IsRunning = true;
        }

        private void DisposeWatcher()
        {
            if (this.Watcher != null)
            {
                try
                {
                    this.Watcher.Dispose();
                }
                finally
                {
                    this.Watcher = null;
                }
            }
        }

        void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            // try and get a lock...
            long waitUntil = Environment.TickCount + 2500;
            bool ok = false;
            while (Environment.TickCount < waitUntil)
            {
                Stream stream = null;
                try
                {
                    // try and get a write lock...
                    stream = new FileStream(e.FullPath, FileMode.Open, FileAccess.Write);
                    ok = true;
                    break;
                }
                catch
                {
                    // no-op...
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }

            if (ok)
            {
                this.Log("File change detected...");
                this.RefreshLazy(DateTime.Now.AddSeconds(5));
            }
        }

        internal void Log(string message)
        {
            this.Log(message, null);
        }

        internal void Log(string message, Exception ex)
        {
            this.OnLogMessage(new LogEventArgs(message, ex));
        }

        protected virtual void OnLogMessage(LogEventArgs e)
        {
            // raise...
            if (LogMessage != null)
                LogMessage(this, e);
        }

        internal void HandleBakingStarted()
        {
            this.OnBakingStarted(EventArgs.Empty);
        }

        protected virtual void OnBakingStarted(EventArgs e)
        {
            if (this.BakingStarted != null)
                this.BakingStarted(this, e);
        }

        internal void HandleBakingFinished()
        {
            this.OnBakingFinished(EventArgs.Empty);
        }

        protected virtual void OnBakingFinished(EventArgs e)
        {
            if (this.BakingFinished != null)
                this.BakingFinished(this, e);
        }

        public void RefreshLazy(DateTime dt)
        {
            if (LastSettings == null)
                throw new InvalidOperationException("'LastSettings' is null.");
            this.Runner.Enqueue(this.LastSettings, dt);
        }

        public void Refresh()
        {
            if (LastSettings == null)
                throw new InvalidOperationException("'LastSettings' is null.");
            this.Runner.Enqueue(this.LastSettings);
        }
    }
}
