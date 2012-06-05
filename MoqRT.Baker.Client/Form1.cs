using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoqRT.Baking.Client
{
    public partial class Form1 : Form
    {
        private BakingController Controller { get; set; }
        private List<string> LogItems { get; set; }
        private int LogHash { get; set; }
        private object _logLock = new object();

        public Form1()
        {
            InitializeComponent();

            this.LogItems = new List<string>();
            this.Controller = new BakingController();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // sub...
            this.Controller.LogMessage += Controller_LogMessage;
            this.Controller.BakingStarted += Controller_BakingStarted;

            // update...
            this.RefreshRunStop();
        }

        void Controller_BakingStarted(object sender, EventArgs e)
        {
            lock (_logLock)
                this.LogItems.Clear();
        }

        void Controller_LogMessage(object sender, LogEventArgs e)
        {
            this.AddToLog(e.Message, e.Exception);
        }

        private void AddToLog(string message, Exception ex)
        {
            lock (_logLock)
            {
                if (ex == null)
                    this.LogItems.Add(message);
                else
                    this.LogItems.Add(string.Format("{0} --> {1}", message, ex));
            }
        }

        private void RefreshRunStop()
        {
            this.buttonRun.Enabled = !(Controller.IsRunning);
            this.buttonStop.Enabled = !(this.buttonRun.Enabled);
            this.buttonRefresh.Enabled = this.buttonStop.Enabled;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Assemblies (*.dll)|*.dll|All Files (*.*)|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.textAssembly.Text = dialog.FileName;

                    var appx = Path.Combine(Path.GetDirectoryName(dialog.FileName), "AppX");
                    if (Directory.Exists(appx))
                        this.textAppxFolder.Text = appx;
                    else
                        this.textAppxFolder.Text = string.Empty;

                    var baking = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(dialog.FileName)), "Baking");
                    if (!(Directory.Exists(baking)))
                        Directory.CreateDirectory(baking);
                    this.textBakingFolder.Text = baking;
                }
            }
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            try
            {
                string assemblyPath = this.textAssembly.Text.Trim();
                if (string.IsNullOrEmpty(assemblyPath))
                {
                    FormExtender.ShowMessage(this, "You must select an assembly.");
                    return;
                }
                string appxFolder = this.textAppxFolder.Text.Trim();
                if(string.IsNullOrEmpty(appxFolder))
                {
                    FormExtender.ShowMessage(this, "You must select an AppX folder.");
                    return;
                }
                string bakingFolder = this.textBakingFolder.Text.Trim();
                if (string.IsNullOrEmpty(bakingFolder))
                {
                    FormExtender.ShowMessage(this, "You must select an baking folder.");
                    return;
                }

                this.Controller.Run(assemblyPath, appxFolder, bakingFolder);
                this.timerLog.Enabled = true;
            }
            catch (Exception ex)
            {
                this.AddToLog("Failed.", ex);
            }
            finally
            {
                this.RefreshRunStop();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} - v{1}", this.Text, typeof(Form1).Assembly.GetName().Version);
        }

        private void timerLog_Tick(object sender, EventArgs e)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                foreach (string item in this.LogItems)
                {
                    builder.Append(item);
                    builder.Append("\r\n");
                }

                // I know this isn't a hash.... quick and dirty to stop it flickering...
                int hash = builder.Length;
                if (hash == this.LogHash)
                    return;

                this.textLog.Text = builder.ToString();
                this.textLog.Select(this.textLog.Text.Length, 0);
                this.textLog.ScrollToCaret();
                this.LogHash = hash;
            }
            catch
            {
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.Controller.Refresh();
        }
    }
}
