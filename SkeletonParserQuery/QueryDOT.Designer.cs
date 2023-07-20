﻿namespace ISPFSkeletonParser
{
    partial class QueryDOT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryDOT));
            this.dgvTables = new System.Windows.Forms.DataGridView();
            this.cboSkeletons = new System.Windows.Forms.ComboBox();
            this.lblSkeleton = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboTables = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTables)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTables
            // 
            this.dgvTables.AllowUserToAddRows = false;
            this.dgvTables.AllowUserToDeleteRows = false;
            this.dgvTables.AllowUserToOrderColumns = true;
            this.dgvTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTables.Location = new System.Drawing.Point(-1, 23);
            this.dgvTables.MultiSelect = false;
            this.dgvTables.Name = "dgvTables";
            this.dgvTables.ReadOnly = true;
            this.dgvTables.RowTemplate.Height = 24;
            this.dgvTables.Size = new System.Drawing.Size(467, 348);
            this.dgvTables.TabIndex = 0;
            this.dgvTables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTables_CellDoubleClick);
            this.dgvTables.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvTables_RowPrepPaint);
            // 
            // cboSkeletons
            // 
            this.cboSkeletons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSkeletons.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSkeletons.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSkeletons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkeletons.FormattingEnabled = true;
            this.cboSkeletons.Location = new System.Drawing.Point(13, 403);
            this.cboSkeletons.Name = "cboSkeletons";
            this.cboSkeletons.Size = new System.Drawing.Size(81, 21);
            this.cboSkeletons.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cboSkeletons, "Select a specific _skeleton. Press the Filter button to activate.");
            // 
            // lblSkeleton
            // 
            this.lblSkeleton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSkeleton.AutoSize = true;
            this.lblSkeleton.Location = new System.Drawing.Point(12, 383);
            this.lblSkeleton.Name = "lblSkeleton";
            this.lblSkeleton.Size = new System.Drawing.Size(49, 13);
            this.lblSkeleton.TabIndex = 2;
            this.lblSkeleton.Text = "Skeleton";
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFilter.Location = new System.Drawing.Point(232, 386);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(64, 57);
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
            this.btnAll.Location = new System.Drawing.Point(314, 386);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(64, 57);
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
            this.btnExit.Location = new System.Drawing.Point(393, 386);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(64, 57);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "&Return";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnExit, "Close this form");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // cboTables
            // 
            this.cboTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboTables.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboTables.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTables.FormattingEnabled = true;
            this.cboTables.Location = new System.Drawing.Point(106, 403);
            this.cboTables.Name = "cboTables";
            this.cboTables.Size = new System.Drawing.Size(105, 21);
            this.cboTables.TabIndex = 13;
            this.toolTip1.SetToolTip(this.cboTables, "Select a specific _skeleton. Press the Filter button to activate.");
            // 
            // lblTable
            // 
            this.lblTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(106, 383);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(34, 13);
            this.lblTable.TabIndex = 14;
            this.lblTable.Text = "Table";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(468, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportCSVToolStripMenuItem,
            this.exportXMLToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // exportCSVToolStripMenuItem
            // 
            this.exportCSVToolStripMenuItem.Name = "exportCSVToolStripMenuItem";
            this.exportCSVToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.exportCSVToolStripMenuItem.Text = "Export CSV...";
            this.exportCSVToolStripMenuItem.Click += new System.EventHandler(this.ExportCSV_XML);
            // 
            // exportXMLToolStripMenuItem
            // 
            this.exportXMLToolStripMenuItem.Name = "exportXMLToolStripMenuItem";
            this.exportXMLToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.exportXMLToolStripMenuItem.Text = "Export XML...";
            this.exportXMLToolStripMenuItem.Click += new System.EventHandler(this.ExportCSV_XML);
            // 
            // QueryDOT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 451);
            this.Controls.Add(this.lblTable);
            this.Controls.Add(this.cboTables);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblSkeleton);
            this.Controls.Add(this.cboSkeletons);
            this.Controls.Add(this.dgvTables);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(484, 251);
            this.Name = "QueryDOT";
            this.Text = "QueryDOT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QueryDOT_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTables)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTables;
        private System.Windows.Forms.ComboBox cboSkeletons;
        private System.Windows.Forms.Label lblSkeleton;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.ComboBox cboTables;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportXMLToolStripMenuItem;
    }
}