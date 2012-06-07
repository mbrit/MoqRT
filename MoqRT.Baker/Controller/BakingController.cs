﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    public class BakingController : MarshalByRefObject, ILog
    {
        public bool IsRunning { get; private set; }
        private BakingSettings LastSettings { get; set; }
        private Task BakingTask { get; set; }
        private BakingRunner Runner { get; set; }
        private FileSystemWatcher Watcher { get; set; }
        private TestProject _activeProject;

        public event EventHandler<LogEventArgs> LogMessage;
        public event EventHandler WorkItemStarted;
        public event EventHandler WorkItemFinished;
        public event EventHandler ScanningStarted;
        public event EventHandler ScanningFinished;
        public event EventHandler BakingStarted;
        public event EventHandler BakingFinished;
        public event EventHandler ActiveProjectChanged;

        public BakingController()
        {
            this.Runner = new BakingRunner(this);
        }

        public void Run(BakingSettings settings)
        {
            if (this.IsRunning)
                throw new InvalidOperationException("Already running.");

            // if...
            DisposeWatcher();
            this.Watcher = new FileSystemWatcher(Path.GetDirectoryName(settings.AssemblyPath), Path.GetFileName(settings.AssemblyPath));
            this.Watcher.Changed += Watcher_Changed;
            this.Watcher.EnableRaisingEvents = true;

            // create...
            this.Log(string.Format("Starting monitoring of '{0}'...", settings.AssemblyPath));

            // run...
            this.LastSettings = settings.Clone();
            this.Runner.EnqueueScan(this.LastSettings);

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
                this.RefreshScanLazy(DateTime.Now.AddSeconds(5));
            }
        }

        public void Log(string message)
        {
            this.Log(message, null);
        }

        public void Log(string message, Exception ex)
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

        internal void HandleWorkItemStarted()
        {
            this.OnWorkItemStarted(EventArgs.Empty);
        }

        protected virtual void OnWorkItemStarted(EventArgs e)
        {
            if (this.WorkItemStarted != null)
                this.WorkItemStarted(this, e);
        }

        internal void HandleWorkItemFinished()
        {
            this.OnWorkItemFinished(EventArgs.Empty);
        }

        protected virtual void OnWorkItemFinished(EventArgs e)
        {
            if (this.WorkItemFinished != null)
                this.WorkItemFinished(this, e);
        }

        internal void HandleScanningStarted()
        {
            this.OnScanningStarted(EventArgs.Empty);
        }

        protected virtual void OnScanningStarted(EventArgs e)
        {
            if (this.ScanningStarted != null)
                this.ScanningStarted(this, e);
        }

        internal void HandleScanningFinished()
        {
            this.OnScanningFinished(EventArgs.Empty);
        }

        protected virtual void OnScanningFinished(EventArgs e)
        {
            if (this.ScanningFinished != null)
                this.ScanningFinished(this, e);
        }

        public void RefreshScanLazy(DateTime dt)
        {
            if (LastSettings == null)
                throw new InvalidOperationException("'LastSettings' is null.");
            this.Runner.EnqueueScan(this.LastSettings, dt);
        }

        public void RefreshScan()
        {
            if (LastSettings == null)
                throw new InvalidOperationException("'LastSettings' is null.");
            this.Runner.EnqueueScan(this.LastSettings);
        }

        protected void OnActiveProjectChanged(EventArgs e)
        {
            if (this.ActiveProjectChanged != null)
                this.ActiveProjectChanged(this, e);
        }

        public TestProject ActiveProject
        {
            get
            {
                return _activeProject;
            }
            set
            {
                // patch...
                if (_activeProject != null)
                    value.PatchIncludes(_activeProject);

                // update...
                _activeProject = value;

                // set...
                this.OnActiveProjectChanged(EventArgs.Empty);
            }
        }

        public void ForceBaking()
        {
            this.Runner.EnqueueBaking(this.LastSettings);
        }
    }
}
