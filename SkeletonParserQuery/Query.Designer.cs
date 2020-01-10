namespace SkeletonParserQuery
{
    partial class Query
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Query));
            this.btnVariables = new System.Windows.Forms.Button();
            this.QueryImageList = new System.Windows.Forms.ImageList(this.components);
            this.btnSkeleton = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboSkeletons = new System.Windows.Forms.ComboBox();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnDOT = new System.Windows.Forms.Button();
            this.btnPrograms = new System.Windows.Forms.Button();
            this.cboSkeletons2 = new System.Windows.Forms.ComboBox();
            this.cboSkeletons3 = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnVariables
            // 
            this.btnVariables.ImageKey = "skeltovar.ico";
            this.btnVariables.ImageList = this.QueryImageList;
            this.btnVariables.Location = new System.Drawing.Point(414, 38);
            this.btnVariables.Name = "btnVariables";
            this.btnVariables.Size = new System.Drawing.Size(64, 57);
            this.btnVariables.TabIndex = 3;
            this.btnVariables.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnVariables, "Display the Skeleton to Variable cross-reference.");
            this.btnVariables.UseVisualStyleBackColor = true;
            this.btnVariables.Click += new System.EventHandler(this.btnVariables_Click);
            // 
            // QueryImageList
            // 
            this.QueryImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("QueryImageList.ImageStream")));
            this.QueryImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.QueryImageList.Images.SetKeyName(0, "FindAll.ico");
            this.QueryImageList.Images.SetKeyName(1, "pgmReturn.ico");
            this.QueryImageList.Images.SetKeyName(2, "pgmExit.ico");
            this.QueryImageList.Images.SetKeyName(3, "filter.ico");
            this.QueryImageList.Images.SetKeyName(4, "skeltovar.ico");
            this.QueryImageList.Images.SetKeyName(5, "skeltotable.ico");
            this.QueryImageList.Images.SetKeyName(6, "skeltoskel.ico");
            this.QueryImageList.Images.SetKeyName(7, "skeltree.ico");
            this.QueryImageList.Images.SetKeyName(8, "skeltoprog.ico");
            this.QueryImageList.Images.SetKeyName(9, "configurations2.ico");
            this.QueryImageList.Images.SetKeyName(10, "RoadMap.ico");
            this.QueryImageList.Images.SetKeyName(11, "Expand.ico");
            this.QueryImageList.Images.SetKeyName(12, "CRDFLE12.ICO");
            this.QueryImageList.Images.SetKeyName(13, "imbeds2.ico");
            this.QueryImageList.Images.SetKeyName(14, "skelmapper0");
            this.QueryImageList.Images.SetKeyName(15, "skelmapper1");
            this.QueryImageList.Images.SetKeyName(16, "skelmapper2");
            this.QueryImageList.Images.SetKeyName(17, "skelmapper3");
            this.QueryImageList.Images.SetKeyName(18, "skelmapper4");
            this.QueryImageList.Images.SetKeyName(19, "skelmapper5");
            this.QueryImageList.Images.SetKeyName(20, "skelmapper6");
            this.QueryImageList.Images.SetKeyName(21, "skelmapper7");
            this.QueryImageList.Images.SetKeyName(22, "skelmapper8");
            this.QueryImageList.Images.SetKeyName(23, "skelmapper9");
            this.QueryImageList.Images.SetKeyName(24, "skelmapper10");
            this.QueryImageList.Images.SetKeyName(25, "skelmapper11");
            this.QueryImageList.Images.SetKeyName(26, "skelmapper12");
            this.QueryImageList.Images.SetKeyName(27, "Configurations");
            this.QueryImageList.Images.SetKeyName(28, "CollapseGraph");
            this.QueryImageList.Images.SetKeyName(29, "ExpandGraph");
            // 
            // btnSkeleton
            // 
            this.btnSkeleton.ImageKey = "skeltoskel.ico";
            this.btnSkeleton.ImageList = this.QueryImageList;
            this.btnSkeleton.Location = new System.Drawing.Point(322, 38);
            this.btnSkeleton.Name = "btnSkeleton";
            this.btnSkeleton.Size = new System.Drawing.Size(64, 57);
            this.btnSkeleton.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnSkeleton, "Display the Skeleton to )IM\'d skeleton cross-reference.");
            this.btnSkeleton.UseVisualStyleBackColor = true;
            this.btnSkeleton.Click += new System.EventHandler(this.btnSkeleton_Click);
            // 
            // btnExit
            // 
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.ImageKey = "pgmExit.ico";
            this.btnExit.ImageList = this.QueryImageList;
            this.btnExit.Location = new System.Drawing.Point(414, 210);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(64, 57);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "E&xit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.ImageKey = "Configurations";
            this.btnConfig.ImageList = this.QueryImageList;
            this.btnConfig.Location = new System.Drawing.Point(14, 38);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(64, 57);
            this.btnConfig.TabIndex = 0;
            this.btnConfig.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnConfig, "Browse for XML configuration file.");
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // cboSkeletons
            // 
            this.cboSkeletons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSkeletons.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSkeletons.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSkeletons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkeletons.FormattingEnabled = true;
            this.cboSkeletons.Location = new System.Drawing.Point(14, 168);
            this.cboSkeletons.Name = "cboSkeletons";
            this.cboSkeletons.Size = new System.Drawing.Size(190, 21);
            this.cboSkeletons.TabIndex = 6;
            this.toolTip1.SetToolTip(this.cboSkeletons, "Select a specific skeleton. Press the \'Expand\' button to process.");
            // 
            // btnExpand
            // 
            this.btnExpand.ImageKey = "Expand.ico";
            this.btnExpand.ImageList = this.QueryImageList;
            this.btnExpand.Location = new System.Drawing.Point(124, 38);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(64, 57);
            this.btnExpand.TabIndex = 1;
            this.btnExpand.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnExpand, "Expand the skeleton name specified in the drop-down selector below.");
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnDOT
            // 
            this.btnDOT.ImageKey = "skeltotable.ico";
            this.btnDOT.ImageList = this.QueryImageList;
            this.btnDOT.Location = new System.Drawing.Point(322, 106);
            this.btnDOT.Name = "btnDOT";
            this.btnDOT.Size = new System.Drawing.Size(64, 57);
            this.btnDOT.TabIndex = 4;
            this.btnDOT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnDOT, "Display the Skeleton to Tables cross-reference.");
            this.btnDOT.UseVisualStyleBackColor = true;
            this.btnDOT.Click += new System.EventHandler(this.btnDOT_Click);
            // 
            // btnPrograms
            // 
            this.btnPrograms.ImageKey = "skeltoprog.ico";
            this.btnPrograms.ImageList = this.QueryImageList;
            this.btnPrograms.Location = new System.Drawing.Point(414, 106);
            this.btnPrograms.Name = "btnPrograms";
            this.btnPrograms.Size = new System.Drawing.Size(64, 57);
            this.btnPrograms.TabIndex = 5;
            this.btnPrograms.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnPrograms, "Display the Skeleton to called programs cross-reference.");
            this.btnPrograms.UseVisualStyleBackColor = true;
            this.btnPrograms.Click += new System.EventHandler(this.btnPrograms_Click);
            // 
            // cboSkeletons2
            // 
            this.cboSkeletons2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSkeletons2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSkeletons2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSkeletons2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkeletons2.FormattingEnabled = true;
            this.cboSkeletons2.Location = new System.Drawing.Point(14, 202);
            this.cboSkeletons2.Name = "cboSkeletons2";
            this.cboSkeletons2.Size = new System.Drawing.Size(190, 21);
            this.cboSkeletons2.TabIndex = 7;
            this.toolTip1.SetToolTip(this.cboSkeletons2, "Select a specific skeleton. Press the \'Expand\' button to process.");
            // 
            // cboSkeletons3
            // 
            this.cboSkeletons3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSkeletons3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSkeletons3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSkeletons3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkeletons3.FormattingEnabled = true;
            this.cboSkeletons3.Location = new System.Drawing.Point(14, 235);
            this.cboSkeletons3.Name = "cboSkeletons3";
            this.cboSkeletons3.Size = new System.Drawing.Size(190, 21);
            this.cboSkeletons3.TabIndex = 8;
            this.toolTip1.SetToolTip(this.cboSkeletons3, "Select a specific skeleton. Press the \'Expand\' button to process.");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 271);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(483, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "expansionStatus";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(468, 17);
            this.StatusLabel.Spring = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(483, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 151);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Select up to 3 FTINCL\'d skeletons to expand";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 137);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Expand:";
            // 
            // Query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 293);
            this.Controls.Add(this.cboSkeletons3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSkeletons2);
            this.Controls.Add(this.btnPrograms);
            this.Controls.Add(this.btnDOT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboSkeletons);
            this.Controls.Add(this.btnExpand);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSkeleton);
            this.Controls.Add(this.btnVariables);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(499, 332);
            this.MinimumSize = new System.Drawing.Size(499, 332);
            this.Name = "Query";
            this.Text = "Query";
            this.Shown += new System.EventHandler(this.Query_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVariables;
        private System.Windows.Forms.Button btnSkeleton;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ImageList QueryImageList;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.ComboBox cboSkeletons;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDOT;
        private System.Windows.Forms.Button btnPrograms;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboSkeletons2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSkeletons3;
    }
}