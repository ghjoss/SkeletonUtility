namespace ISPFSkeletonParser
{
    partial class QueryProgram
    {
        /// <summary>
        /// Required designer _variable.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryProgram));
            this.dgvPrograms = new System.Windows.Forms.DataGridView();
            this.cboSkeletons = new System.Windows.Forms.ComboBox();
            this.lblSkeleton = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboPrograms = new System.Windows.Forms.ComboBox();
            this.lblProgram = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrograms)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPrograms
            // 
            this.dgvPrograms.AllowUserToAddRows = false;
            this.dgvPrograms.AllowUserToDeleteRows = false;
            this.dgvPrograms.AllowUserToOrderColumns = true;
            this.dgvPrograms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPrograms.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPrograms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrograms.Location = new System.Drawing.Point(-1, 28);
            this.dgvPrograms.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPrograms.MultiSelect = false;
            this.dgvPrograms.Name = "dgvPrograms";
            this.dgvPrograms.ReadOnly = true;
            this.dgvPrograms.RowTemplate.Height = 24;
            this.dgvPrograms.Size = new System.Drawing.Size(623, 402);
            this.dgvPrograms.TabIndex = 0;
            this.dgvPrograms.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrograms_CellDoubleClick);
            this.dgvPrograms.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvTables_RowPrepPaint);
            // 
            // cboSkeletons
            // 
            this.cboSkeletons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSkeletons.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSkeletons.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSkeletons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkeletons.FormattingEnabled = true;
            this.cboSkeletons.Location = new System.Drawing.Point(17, 496);
            this.cboSkeletons.Margin = new System.Windows.Forms.Padding(4);
            this.cboSkeletons.Name = "cboSkeletons";
            this.cboSkeletons.Size = new System.Drawing.Size(106, 24);
            this.cboSkeletons.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cboSkeletons, "Select a specific _skeleton. Press the Filter button to activate.");
            // 
            // lblSkeleton
            // 
            this.lblSkeleton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSkeleton.AutoSize = true;
            this.lblSkeleton.Location = new System.Drawing.Point(16, 471);
            this.lblSkeleton.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSkeleton.Name = "lblSkeleton";
            this.lblSkeleton.Size = new System.Drawing.Size(63, 17);
            this.lblSkeleton.TabIndex = 2;
            this.lblSkeleton.Text = "Skeleton";
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFilter.Location = new System.Drawing.Point(277, 475);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(4);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(85, 70);
            this.btnFilter.TabIndex = 12;
            this.btnFilter.Text = "F&ilter";
            this.btnFilter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnFilter, "Filter displayed data using the \r_skeletonon drop down on the left.");
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnAll
            // 
            this.btnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAll.Location = new System.Drawing.Point(379, 475);
            this.btnAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(85, 70);
            this.btnAll.TabIndex = 11;
            this.btnAll.Text = "&All";
            this.btnAll.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnAll, "Show all skeletons and related embedded skeletons.");
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(524, 475);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 70);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "&Return";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnExit, "Close this form");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cboPrograms
            // 
            this.cboPrograms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboPrograms.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboPrograms.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPrograms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrograms.FormattingEnabled = true;
            this.cboPrograms.Location = new System.Drawing.Point(152, 496);
            this.cboPrograms.Margin = new System.Windows.Forms.Padding(4);
            this.cboPrograms.Name = "cboPrograms";
            this.cboPrograms.Size = new System.Drawing.Size(105, 24);
            this.cboPrograms.TabIndex = 13;
            this.toolTip1.SetToolTip(this.cboPrograms, "Select a specific _skeleton. Press the Filter button to activate.");
            // 
            // lblProgram
            // 
            this.lblProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProgram.AutoSize = true;
            this.lblProgram.Location = new System.Drawing.Point(149, 471);
            this.lblProgram.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProgram.Name = "lblProgram";
            this.lblProgram.Size = new System.Drawing.Size(62, 17);
            this.lblProgram.TabIndex = 14;
            this.lblProgram.Text = "Program";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(622, 28);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportCSVToolStripMenuItem,
            this.exportXMLToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.ToolTipText = "Write a Comma Separated Value extract of the displayed data.";
            // 
            // exportCSVToolStripMenuItem
            // 
            this.exportCSVToolStripMenuItem.Name = "exportCSVToolStripMenuItem";
            this.exportCSVToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.exportCSVToolStripMenuItem.Text = "Export CSV...";
            this.exportCSVToolStripMenuItem.Click += new System.EventHandler(this.ExportCSV_XML);
            // 
            // exportXMLToolStripMenuItem
            // 
            this.exportXMLToolStripMenuItem.Name = "exportXMLToolStripMenuItem";
            this.exportXMLToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.exportXMLToolStripMenuItem.Text = "Export XML...";
            this.exportXMLToolStripMenuItem.Click += new System.EventHandler(this.ExportCSV_XML);
            // 
            // QueryProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 555);
            this.Controls.Add(this.lblProgram);
            this.Controls.Add(this.cboPrograms);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblSkeleton);
            this.Controls.Add(this.cboSkeletons);
            this.Controls.Add(this.dgvPrograms);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(640, 300);
            this.Name = "QueryProgram";
            this.Text = "QueryProgram";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QueryProgram_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrograms)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPrograms;
        private System.Windows.Forms.ComboBox cboSkeletons;
        private System.Windows.Forms.Label lblSkeleton;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblProgram;
        private System.Windows.Forms.ComboBox cboPrograms;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportXMLToolStripMenuItem;
    }
}