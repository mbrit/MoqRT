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
            this.Controller.WorkItemStarted += Controller_WorkItemStarted;
            this.Controller.WorkItemFinished += Controller_WorkItemFinished;
            this.Controller.ActiveProjectChanged += Controller_ActiveProjectChanged;

            // update...
            this.RefreshRunStop();
        }

        void Controller_WorkItemFinished(object sender, EventArgs e)
        {
            // go...
            this.SafeInvoke(() =>
            {
                this.buttonRefresh.Enabled = true;
                this.buttonForceBaking.Enabled = true;
            });
        }

        void Controller_ActiveProjectChanged(object sender, EventArgs e)
        {
            this.SafeInvoke(() => this.RefreshTree());
        }

        void Controller_WorkItemStarted(object sender, EventArgs e)
        {
            lock (_logLock)
                this.LogItems.Clear();

            // go...
            this.SafeInvoke(() =>
            {
                this.buttonRefresh.Enabled = false;
                this.buttonForceBaking.Enabled = false;
            });
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
            this.buttonForceBaking.Enabled = this.buttonStop.Enabled;
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

                // run...
                this.Controller.Run(new BakingSettings(assemblyPath, appxFolder, bakingFolder));
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
            this.Controller.RefreshScan();
        }

        private void RefreshTree()
        {
            // update..
            this.treeProject.BeginUpdate();
            try
            {
                this.treeProject.Nodes.Clear();
                foreach (TestClass c in this.Controller.ActiveProject.Classes)
                {
                    var node = new TestClassTreeNode(c);
                    this.treeProject.Nodes.Add(node);
                }
            }
            finally
            {
                this.treeProject.EndUpdate();
            }
        }

        private void treeProject_AfterCheck(object sender, TreeViewEventArgs e)
        {
            ((ITreeItemNode)e.Node).HandleCheckChanged();
        }

        private void buttonForceBaking_Click(object sender, EventArgs e)
        {
            this.Controller.ForceBaking();
        }

        private void linkCheckAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (TreeNode node in this.treeProject.Nodes)
                node.Checked = true;
        }

        private void linkCheckNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (TreeNode node in this.treeProject.Nodes)
                node.Checked = false;
        }
    }
}
