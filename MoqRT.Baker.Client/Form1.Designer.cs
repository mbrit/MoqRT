namespace MoqRT.Baking.Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonBrowseBaking = new System.Windows.Forms.Button();
            this.textBakingFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonBrowseAppx = new System.Windows.Forms.Button();
            this.textAppxFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textAssembly = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textLog = new System.Windows.Forms.RichTextBox();
            this.timerLog = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.treeProject = new System.Windows.Forms.TreeView();
            this.buttonForceBaking = new System.Windows.Forms.Button();
            this.linkCheckAll = new System.Windows.Forms.LinkLabel();
            this.linkCheckNone = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonForceBaking);
            this.groupBox1.Controls.Add(this.buttonBrowseBaking);
            this.groupBox1.Controls.Add(this.textBakingFolder);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.buttonRefresh);
            this.groupBox1.Controls.Add(this.buttonStop);
            this.groupBox1.Controls.Add(this.buttonRun);
            this.groupBox1.Controls.Add(this.buttonBrowseAppx);
            this.groupBox1.Controls.Add(this.textAppxFolder);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonBrowse);
            this.groupBox1.Controls.Add(this.textAssembly);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(750, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // buttonBrowseBaking
            // 
            this.buttonBrowseBaking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseBaking.Location = new System.Drawing.Point(669, 65);
            this.buttonBrowseBaking.Name = "buttonBrowseBaking";
            this.buttonBrowseBaking.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseBaking.TabIndex = 11;
            this.buttonBrowseBaking.Text = "Br&owse >>";
            this.buttonBrowseBaking.UseVisualStyleBackColor = true;
            // 
            // textBakingFolder
            // 
            this.textBakingFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBakingFolder.Location = new System.Drawing.Point(86, 65);
            this.textBakingFolder.Name = "textBakingFolder";
            this.textBakingFolder.Size = new System.Drawing.Size(577, 20);
            this.textBakingFolder.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Ba&king folder";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(248, 98);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(126, 23);
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.Text = "&Rescan Assembly";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(167, 98);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 7;
            this.buttonStop.Text = "&Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(86, 98);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 6;
            this.buttonRun.Text = "Ru&n";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonBrowseAppx
            // 
            this.buttonBrowseAppx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseAppx.Location = new System.Drawing.Point(669, 39);
            this.buttonBrowseAppx.Name = "buttonBrowseAppx";
            this.buttonBrowseAppx.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseAppx.TabIndex = 5;
            this.buttonBrowseAppx.Text = "Bro&wse >>";
            this.buttonBrowseAppx.UseVisualStyleBackColor = true;
            // 
            // textAppxFolder
            // 
            this.textAppxFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textAppxFolder.Location = new System.Drawing.Point(86, 39);
            this.textAppxFolder.Name = "textAppxFolder";
            this.textAppxFolder.Size = new System.Drawing.Size(577, 20);
            this.textAppxFolder.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "&AppX folder";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Location = new System.Drawing.Point(669, 13);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "&Browse >>";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textAssembly
            // 
            this.textAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textAssembly.Location = new System.Drawing.Point(86, 13);
            this.textAssembly.Name = "textAssembly";
            this.textAssembly.Size = new System.Drawing.Size(577, 20);
            this.textAssembly.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Test assembly";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textLog);
            this.groupBox2.Location = new System.Drawing.Point(12, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(429, 387);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // textLog
            // 
            this.textLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLog.Location = new System.Drawing.Point(9, 16);
            this.textLog.Name = "textLog";
            this.textLog.ReadOnly = true;
            this.textLog.Size = new System.Drawing.Size(414, 362);
            this.textLog.TabIndex = 0;
            this.textLog.Text = "";
            // 
            // timerLog
            // 
            this.timerLog.Interval = 250;
            this.timerLog.Tick += new System.EventHandler(this.timerLog_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 153);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 153);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(774, 393);
            this.panel2.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(450, 393);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.linkCheckNone);
            this.panel4.Controls.Add(this.linkCheckAll);
            this.panel4.Controls.Add(this.treeProject);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(450, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(324, 393);
            this.panel4.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(444, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 393);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // treeProject
            // 
            this.treeProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeProject.CheckBoxes = true;
            this.treeProject.Location = new System.Drawing.Point(6, 6);
            this.treeProject.Name = "treeProject";
            this.treeProject.Size = new System.Drawing.Size(306, 364);
            this.treeProject.TabIndex = 0;
            this.treeProject.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeProject_AfterCheck);
            // 
            // buttonForceBaking
            // 
            this.buttonForceBaking.Location = new System.Drawing.Point(380, 98);
            this.buttonForceBaking.Name = "buttonForceBaking";
            this.buttonForceBaking.Size = new System.Drawing.Size(126, 23);
            this.buttonForceBaking.TabIndex = 12;
            this.buttonForceBaking.Text = "&Force Baking";
            this.buttonForceBaking.UseVisualStyleBackColor = true;
            this.buttonForceBaking.Click += new System.EventHandler(this.buttonForceBaking_Click);
            // 
            // linkCheckAll
            // 
            this.linkCheckAll.AutoSize = true;
            this.linkCheckAll.Location = new System.Drawing.Point(5, 373);
            this.linkCheckAll.Name = "linkCheckAll";
            this.linkCheckAll.Size = new System.Drawing.Size(51, 13);
            this.linkCheckAll.TabIndex = 1;
            this.linkCheckAll.TabStop = true;
            this.linkCheckAll.Text = "Check all";
            this.linkCheckAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCheckAll_LinkClicked);
            // 
            // linkCheckNone
            // 
            this.linkCheckNone.AutoSize = true;
            this.linkCheckNone.Location = new System.Drawing.Point(62, 373);
            this.linkCheckNone.Name = "linkCheckNone";
            this.linkCheckNone.Size = new System.Drawing.Size(65, 13);
            this.linkCheckNone.TabIndex = 2;
            this.linkCheckNone.TabStop = true;
            this.linkCheckNone.Text = "Check none";
            this.linkCheckNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCheckNone_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 546);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MoqRT Baker";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonBrowseAppx;
        private System.Windows.Forms.TextBox textAppxFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textAssembly;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox textLog;
        private System.Windows.Forms.Timer timerLog;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonBrowseBaking;
        private System.Windows.Forms.TextBox textBakingFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TreeView treeProject;
        private System.Windows.Forms.Button buttonForceBaking;
        private System.Windows.Forms.LinkLabel linkCheckNone;
        private System.Windows.Forms.LinkLabel linkCheckAll;
    }
}

