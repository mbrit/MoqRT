using System;
using System.Collections.Generic;
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

        public event EventHandler<LogEventArgs> LogMessage;
        public event EventHandler BakingStarted;
        public event EventHandler BakingFinished;

        public BakingController()
        {
            this.Runner = new BakingRunner(this);
        }

        public void Run(string assemblyPath, string appxPath)
        {
            if (this.IsRunning)
                throw new InvalidOperationException("Already running.");

            // create...
            this.Log(string.Format("Starting monitoring of '{0}'...", assemblyPath));

            // run...
            this.LastSettings = new BakingSettings(assemblyPath, appxPath);
            this.Runner.Enqueue(new BakingSettings(assemblyPath, appxPath));

            // ok...
            this.IsRunning = true;
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

        public void Refresh()
        {
            if (LastSettings == null)
                throw new InvalidOperationException("'LastSettings' is null.");
            this.Runner.Enqueue(this.LastSettings);
        }
    }
}
