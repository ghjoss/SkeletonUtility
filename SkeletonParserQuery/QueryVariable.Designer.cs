namespace SkeletonParserQuery
{
    partial class QueryVariable
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
            if (!_disposed)
            {
                if (disposing)
                {
                    for (int i = 0; i < _results.Length; ++ i)
                        _results[i] = null;
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryVariable));
            this.cboSkeletons = new System.Windows.Forms.ComboBox();
            this.lblSkeletons = new System.Windows.Forms.Label();
            this.cboVariables = new System.Windows.Forms.ComboBox();
            this.lblVariables = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblType = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.dgvVariables = new System.Windows.Forms.DataGridView();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.cboOrderBy = new System.Windows.Forms.ComboBox();
            this.lblOrderBy = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCSV11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboSkeletons
            // 
            this.cboSkeletons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSkeletons.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSkeletons.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSkeletons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkeletons.FormattingEnabled = true;
            this.cboSkeletons.Location = new System.Drawing.Point(19, 503);
            this.cboSkeletons.Margin = new System.Windows.Forms.Padding(4);
            this.cboSkeletons.Name = "cboSkeletons";
            this.cboSkeletons.Size = new System.Drawing.Size(116, 24);
            this.cboSkeletons.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cboSkeletons, "Select a specific _skeleton. Press the Filter button to activate.");
            // 
            // lblSkeletons
            // 
            this.lblSkeletons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSkeletons.AutoSize = true;
            this.lblSkeletons.Location = new System.Drawing.Point(16, 478);
            this.lblSkeletons.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSkeletons.Name = "lblSkeletons";
            this.lblSkeletons.Size = new System.Drawing.Size(63, 17);
            this.lblSkeletons.TabIndex = 2;
            this.lblSkeletons.Text = "Skeleton";
            // 
            // cboVariables
            // 
            this.cboVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboVariables.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboVariables.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVariables.FormattingEnabled = true;
            this.cboVariables.Location = new System.Drawing.Point(152, 503);
            this.cboVariables.Margin = new System.Windows.Forms.Padding(4);
            this.cboVariables.Name = "cboVariables";
            this.cboVariables.Size = new System.Drawing.Size(116, 24);
            this.cboVariables.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cboVariables, "Select a specific _variable. Press the Filter button to activate.");
            // 
            // lblVariables
            // 
            this.lblVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVariables.AutoSize = true;
            this.lblVariables.Location = new System.Drawing.Point(149, 478);
            this.lblVariables.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(60, 17);
            this.lblVariables.TabIndex = 4;
            this.lblVariables.Text = "Variable";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(918, 478);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 70);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "&Return";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnExit, "Close this form");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // lblType
            // 
            this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(283, 478);
            this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(40, 17);
            this.lblType.TabIndex = 7;
            this.lblType.Text = "Type";
            // 
            // cboType
            // 
            this.cboType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.DropDownWidth = 293;
            this.cboType.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(286, 503);
            this.cboType.Margin = new System.Windows.Forms.Padding(4);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(222, 24);
            this.cboType.TabIndex = 6;
            this.toolTip1.SetToolTip(this.cboType, "Select a specific _variable reference type. Press the Filter button to activate.");
            // 
            // dgvVariables
            // 
            this.dgvVariables.AllowUserToAddRows = false;
            this.dgvVariables.AllowUserToDeleteRows = false;
            this.dgvVariables.AllowUserToOrderColumns = true;
            this.dgvVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvVariables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvVariables.Location = new System.Drawing.Point(0, 32);
            this.dgvVariables.Margin = new System.Windows.Forms.Padding(4);
            this.dgvVariables.MultiSelect = false;
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.ReadOnly = true;
            this.dgvVariables.RowTemplate.Height = 24;
            this.dgvVariables.Size = new System.Drawing.Size(1006, 438);
            this.dgvVariables.TabIndex = 0;
            this.dgvVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvVariables_CellDoubleClick);
            this.dgvVariables.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.DgvVariables_RowPrePaint);
            // 
            // btnAll
            // 
            this.btnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAll.Location = new System.Drawing.Point(819, 478);
            this.btnAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(80, 70);
            this.btnAll.TabIndex = 8;
            this.btnAll.Text = "&All";
            this.btnAll.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnAll, "Show all variables in all skeletons");
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.BtnAll_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFilter.Location = new System.Drawing.Point(726, 478);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(4);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(80, 70);
            this.btnFilter.TabIndex = 9;
            this.btnFilter.Text = "F&ilter";
            this.btnFilter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnFilter, "Filter displayed data using the \r\nvalues to the left.");
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.BtnFilter_Click);
            // 
            // cboOrderBy
            // 
            this.cboOrderBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboOrderBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboOrderBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrderBy.DropDownWidth = 300;
            this.cboOrderBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboOrderBy.FormattingEnabled = true;
            this.cboOrderBy.Items.AddRange(new object[] {
            "Skeleton Name, Variable Name",
            "Skeleton Name, Line Number",
            "Variable Name, Skeleton Name"});
            this.cboOrderBy.Location = new System.Drawing.Point(516, 503);
            this.cboOrderBy.Margin = new System.Windows.Forms.Padding(4);
            this.cboOrderBy.Name = "cboOrderBy";
            this.cboOrderBy.Size = new System.Drawing.Size(202, 24);
            this.cboOrderBy.TabIndex = 10;
            this.toolTip1.SetToolTip(this.cboOrderBy, "Select a sort order for the displayed data.");
            // 
            // lblOrderBy
            // 
            this.lblOrderBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOrderBy.AutoSize = true;
            this.lblOrderBy.Location = new System.Drawing.Point(516, 478);
            this.lblOrderBy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOrderBy.Name = "lblOrderBy";
            this.lblOrderBy.Size = new System.Drawing.Size(65, 17);
            this.lblOrderBy.TabIndex = 11;
            this.lblOrderBy.Text = "Order By";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1006, 28);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportCSVToolStripMenuItem,
            this.exportXMLToolStripMenuItem,
            this.exportCSV11ToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // exportCSVToolStripMenuItem
            // 
            this.exportCSVToolStripMenuItem.Name = "exportCSVToolStripMenuItem";
            this.exportCSVToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.exportCSVToolStripMenuItem.Text = "Export CSV...";
            this.exportCSVToolStripMenuItem.ToolTipText = "Write a Comma Separated Value extract of the selected rows.";
            this.exportCSVToolStripMenuItem.Click += new System.EventHandler(this.ExportCSV_XML);
            // 
            // exportXMLToolStripMenuItem
            // 
            this.exportXMLToolStripMenuItem.Name = "exportXMLToolStripMenuItem";
            this.exportXMLToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.exportXMLToolStripMenuItem.Text = "Export XML...";
            this.exportXMLToolStripMenuItem.ToolTipText = "Export a Skeleton-Variable Cross-Reference for the currently selected rows.";
            this.exportXMLToolStripMenuItem.Click += new System.EventHandler(this.ExportCSV_XML);
            // 
            // exportCSV11ToolStripMenuItem
            // 
            this.exportCSV11ToolStripMenuItem.Name = "exportCSV11ToolStripMenuItem";
            this.exportCSV11ToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.exportCSV11ToolStripMenuItem.Text = "Export CSV 1-1...";
            this.exportCSV11ToolStripMenuItem.ToolTipText = "Export a 1-1 Skeleton-Variable CSV for documenting Variables";
            this.exportCSV11ToolStripMenuItem.Click += new System.EventHandler(this.ExportCSV11ToolStripMenuItem_Click);
            // 
            // QueryVariable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 555);
            this.Controls.Add(this.lblOrderBy);
            this.Controls.Add(this.cboOrderBy);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblVariables);
            this.Controls.Add(this.cboVariables);
            this.Controls.Add(this.lblSkeletons);
            this.Controls.Add(this.cboSkeletons);
            this.Controls.Add(this.dgvVariables);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1024, 300);
            this.Name = "QueryVariable";
            this.Text = "Variables Cross Reference";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QueryVariable_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ComboBox cboSkeletons;
        private System.Windows.Forms.Label lblSkeletons;
        private System.Windows.Forms.ComboBox cboVariables;
        private System.Windows.Forms.Label lblVariables;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.DataGridView dgvVariables;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.ComboBox cboOrderBy;
        private System.Windows.Forms.Label lblOrderBy;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCSV11ToolStripMenuItem;


    }
}

