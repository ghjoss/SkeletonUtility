using SkeletonParserDSDef;

namespace SkeletonParserQuery
{
    partial class QueryExpand2
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

            if (!_disposed)
            {
                if (disposing)
                {
                    _expansionBindingSource = null;
                    _expansionRoot = null;
                    _findString = null;
                    _nestingGridLabel = null;
                    _SDS = null;
                    _TDS = null;
                    _xmlParmsReader = null;

                    if (_imbedChain != null && _imbedChain.Count > 0)
                    {
                        _imbedChain.Clear();
                    }
                    if (_cnt != null)
                    {
                        _cnt.Clear();
                        _cnt.Dispose();
                        _cnt = null;
                    }

                    if (_skelExpansion != null)
                    {
                        _skelExpansion.Clear();
                        _skelExpansion.Dispose();
                        _skelExpansion = null;
                    }

                    TransientDS.SkeletonExpansionRow ser = null;

                    if (_dotNestingStack != null)
                    {
                        for (int i = 1; i <= _dotNestingStack.Count; ++i)
                            ser = _dotNestingStack.Pop();
                        _dotNestingStack = null;
                    }

                    if (_selNestingStack != null)
                    {
                        for (int i = 1; i <= _selNestingStack.Count; ++i)
                            ser = _selNestingStack.Pop();
                        _selNestingStack = null;
                    }

                    if (_doNestingStack != null)
                    {
                        for (int i = 1; i <= _doNestingStack.Count; ++i)
                            ser = _doNestingStack.Pop();
                        _doNestingStack = null;
                    }

                    Nesting n;
                    if (_NestingStack != null)
                    {
                        for (int i = 1; i <= _NestingStack.Count; ++i)
                            n = _NestingStack.Pop();
                        _NestingStack = null;
                    }

                    if (_dvSkelExpansion != null)
                    {
                        _dvSkelExpansion.Dispose();
                    }
                    _disposed = true;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryExpand2));
            this.BtnExit = new System.Windows.Forms.Button();
            this.DgvVariables = new System.Windows.Forms.DataGridView();
            this.DgvNesting = new System.Windows.Forms.DataGridView();
            this.DgvSkeletonExpansion = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TsmgFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiApplyTextFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.sep1_1 = new System.Windows.Forms.ToolStripSeparator();
            this.TsmiApplySkelFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiRemoveSep = new System.Windows.Forms.ToolStripSeparator();
            this.TsmgRemoveFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiRemoveTextFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiRemoveSkelSep = new System.Windows.Forms.ToolStripSeparator();
            this.TsmiRemoveSkelFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiRemoveSkelFilterAll = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiRemoveAllSep = new System.Windows.Forms.ToolStripSeparator();
            this.TsmiRemoveFilterAll = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiGotoSep = new System.Windows.Forms.ToolStripSeparator();
            this.TsmiGotoLine = new System.Windows.Forms.ToolStripMenuItem();
            this.SkelNestingGridLabel = new System.Windows.Forms.Label();
            this.VariableGridLabel = new System.Windows.Forms.Label();
            this.ExpansionStatus = new System.Windows.Forms.StatusStrip();
            this.StatusHiddenLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusHiddenSkeletons = new System.Windows.Forms.ToolStripDropDownButton();
            this.StatusFilter = new System.Windows.Forms.ToolStripStatusLabel();
            this.BtnMap = new System.Windows.Forms.Button();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiFindString = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiFindNext = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.DgvVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvNesting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSkeletonExpansion)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.ExpansionStatus.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExit.Location = new System.Drawing.Point(969, 411);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(64, 57);
            this.BtnExit.TabIndex = 12;
            this.BtnExit.Text = "&Return";
            this.BtnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // DgvVariables
            // 
            this.DgvVariables.AllowUserToAddRows = false;
            this.DgvVariables.AllowUserToDeleteRows = false;
            this.DgvVariables.AllowUserToOrderColumns = true;
            this.DgvVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvVariables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Console", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvVariables.DefaultCellStyle = dataGridViewCellStyle1;
            this.DgvVariables.Location = new System.Drawing.Point(636, 292);
            this.DgvVariables.Name = "DgvVariables";
            this.DgvVariables.ReadOnly = true;
            this.DgvVariables.RowTemplate.Height = 24;
            this.DgvVariables.Size = new System.Drawing.Size(397, 101);
            this.DgvVariables.TabIndex = 16;
            this.DgvVariables.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.Grid_ColumnAdded_DisableSort);
            this.DgvVariables.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.DgvVariables_RowPrePaint);
            this.DgvVariables.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DgvVariables_MouseDoubleClick);
            // 
            // DgvNesting
            // 
            this.DgvNesting.AllowUserToAddRows = false;
            this.DgvNesting.AllowUserToDeleteRows = false;
            this.DgvNesting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvNesting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Console", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvNesting.DefaultCellStyle = dataGridViewCellStyle2;
            this.DgvNesting.Location = new System.Drawing.Point(1, 292);
            this.DgvNesting.Name = "DgvNesting";
            this.DgvNesting.ReadOnly = true;
            this.DgvNesting.RowTemplate.Height = 24;
            this.DgvNesting.Size = new System.Drawing.Size(615, 180);
            this.DgvNesting.TabIndex = 15;
            this.DgvNesting.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvNesting_CellEnter);
            this.DgvNesting.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.Grid_ColumnAdded_DisableSort);
            this.DgvNesting.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.DgvNesting_RowPrePaint);
            // 
            // DgvSkeletonExpansion
            // 
            this.DgvSkeletonExpansion.AllowUserToAddRows = false;
            this.DgvSkeletonExpansion.AllowUserToDeleteRows = false;
            this.DgvSkeletonExpansion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvSkeletonExpansion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvSkeletonExpansion.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Lucida Console", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvSkeletonExpansion.DefaultCellStyle = dataGridViewCellStyle3;
            this.DgvSkeletonExpansion.Location = new System.Drawing.Point(1, 23);
            this.DgvSkeletonExpansion.MultiSelect = false;
            this.DgvSkeletonExpansion.Name = "DgvSkeletonExpansion";
            this.DgvSkeletonExpansion.ReadOnly = true;
            this.DgvSkeletonExpansion.RowTemplate.Height = 24;
            this.DgvSkeletonExpansion.Size = new System.Drawing.Size(1043, 248);
            this.DgvSkeletonExpansion.TabIndex = 14;
            this.DgvSkeletonExpansion.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.Grid_ColumnAdded_DisableSort);
            this.DgvSkeletonExpansion.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.DgvSkeletonExpansion_RowPrePaint);
            this.DgvSkeletonExpansion.SelectionChanged += new System.EventHandler(this.DgvSkeletonExpansion_SelectionChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmgFilters,
            this.TsmiRemoveSep,
            this.TsmgRemoveFilters,
            this.TsmiGotoSep,
            this.TsmiGotoLine});
            this.contextMenuStrip1.Name = "ContextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(177, 82);
            // 
            // TsmgFilters
            // 
            this.TsmgFilters.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiApplyTextFilter,
            this.sep1_1,
            this.TsmiApplySkelFilter});
            this.TsmgFilters.Name = "TsmgFilters";
            this.TsmgFilters.Size = new System.Drawing.Size(176, 22);
            this.TsmgFilters.Text = "Filter...";
            // 
            // TsmiApplyTextFilter
            // 
            this.TsmiApplyTextFilter.Name = "TsmiApplyTextFilter";
            this.TsmiApplyTextFilter.Size = new System.Drawing.Size(206, 22);
            this.TsmiApplyTextFilter.Text = "Text in line...";
            this.TsmiApplyTextFilter.ToolTipText = "Show only those lines containing the text that you will specify";
            this.TsmiApplyTextFilter.Click += new System.EventHandler(this.TsmiApplyTextFilter_Click);
            // 
            // sep1_1
            // 
            this.sep1_1.Name = "sep1_1";
            this.sep1_1.Size = new System.Drawing.Size(203, 6);
            // 
            // TsmiApplySkelFilter
            // 
            this.TsmiApplySkelFilter.Name = "TsmiApplySkelFilter";
            this.TsmiApplySkelFilter.Size = new System.Drawing.Size(206, 22);
            this.TsmiApplySkelFilter.Text = "Hide selected skeletons...";
            this.TsmiApplySkelFilter.ToolTipText = "Add a specified skeleton name to the list of skeletons hidden in the expansion.";
            this.TsmiApplySkelFilter.Click += new System.EventHandler(this.TsmiApplySkelFilter_Click);
            // 
            // TsmiRemoveSep
            // 
            this.TsmiRemoveSep.Name = "TsmiRemoveSep";
            this.TsmiRemoveSep.Size = new System.Drawing.Size(173, 6);
            // 
            // TsmgRemoveFilters
            // 
            this.TsmgRemoveFilters.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiRemoveTextFilter,
            this.TsmiRemoveSkelSep,
            this.TsmiRemoveSkelFilter,
            this.TsmiRemoveSkelFilterAll,
            this.TsmiRemoveAllSep,
            this.TsmiRemoveFilterAll});
            this.TsmgRemoveFilters.Enabled = false;
            this.TsmgRemoveFilters.Name = "TsmgRemoveFilters";
            this.TsmgRemoveFilters.Size = new System.Drawing.Size(176, 22);
            this.TsmgRemoveFilters.Text = "Remove Filter...";
            // 
            // TsmiRemoveTextFilter
            // 
            this.TsmiRemoveTextFilter.Name = "TsmiRemoveTextFilter";
            this.TsmiRemoveTextFilter.Size = new System.Drawing.Size(286, 22);
            this.TsmiRemoveTextFilter.Text = "Remove text in line filter";
            this.TsmiRemoveTextFilter.ToolTipText = "Remove the text filter, leaving the skeleton filters (if any) intact.";
            this.TsmiRemoveTextFilter.Visible = false;
            this.TsmiRemoveTextFilter.Click += new System.EventHandler(this.TsmiRemoveTextFilter_Click);
            // 
            // TsmiRemoveSkelSep
            // 
            this.TsmiRemoveSkelSep.Name = "TsmiRemoveSkelSep";
            this.TsmiRemoveSkelSep.Size = new System.Drawing.Size(283, 6);
            // 
            // TsmiRemoveSkelFilter
            // 
            this.TsmiRemoveSkelFilter.Name = "TsmiRemoveSkelFilter";
            this.TsmiRemoveSkelFilter.Size = new System.Drawing.Size(286, 22);
            this.TsmiRemoveSkelFilter.Text = "Remove selected skeleton filter(buffer)...";
            this.TsmiRemoveSkelFilter.ToolTipText = "Remove one or more skeleton filters, leaving the text filters (if any) intact.";
            this.TsmiRemoveSkelFilter.Visible = false;
            this.TsmiRemoveSkelFilter.Click += new System.EventHandler(this.TsmiRemoveSkelFilter_Click);
            // 
            // TsmiRemoveSkelFilterAll
            // 
            this.TsmiRemoveSkelFilterAll.Name = "TsmiRemoveSkelFilterAll";
            this.TsmiRemoveSkelFilterAll.Size = new System.Drawing.Size(286, 22);
            this.TsmiRemoveSkelFilterAll.Text = "Remove all skeleton filters";
            this.TsmiRemoveSkelFilterAll.ToolTipText = "Remove \tall skeleton filters, leaving the text filters (if any) intact.";
            this.TsmiRemoveSkelFilterAll.Visible = false;
            this.TsmiRemoveSkelFilterAll.Click += new System.EventHandler(this.TsmiRemoveSkelFilterAll_Click);
            // 
            // TsmiRemoveAllSep
            // 
            this.TsmiRemoveAllSep.Name = "TsmiRemoveAllSep";
            this.TsmiRemoveAllSep.Size = new System.Drawing.Size(283, 6);
            // 
            // TsmiRemoveFilterAll
            // 
            this.TsmiRemoveFilterAll.Name = "TsmiRemoveFilterAll";
            this.TsmiRemoveFilterAll.Size = new System.Drawing.Size(286, 22);
            this.TsmiRemoveFilterAll.Text = "Remove all filters";
            this.TsmiRemoveFilterAll.ToolTipText = "Remove all filters, re-activate Go To Line";
            this.TsmiRemoveFilterAll.Visible = false;
            this.TsmiRemoveFilterAll.Click += new System.EventHandler(this.TsmiRemoveFilterAll_Click);
            // 
            // TsmiGotoSep
            // 
            this.TsmiGotoSep.Name = "TsmiGotoSep";
            this.TsmiGotoSep.Size = new System.Drawing.Size(173, 6);
            // 
            // TsmiGotoLine
            // 
            this.TsmiGotoLine.Name = "TsmiGotoLine";
            this.TsmiGotoLine.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.TsmiGotoLine.Size = new System.Drawing.Size(176, 22);
            this.TsmiGotoLine.Text = "Go to line...";
            this.TsmiGotoLine.Click += new System.EventHandler(this.TsmiGotoLine_Click);
            // 
            // SkelNestingGridLabel
            // 
            this.SkelNestingGridLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SkelNestingGridLabel.AutoSize = true;
            this.SkelNestingGridLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SkelNestingGridLabel.Location = new System.Drawing.Point(1, 274);
            this.SkelNestingGridLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SkelNestingGridLabel.Name = "SkelNestingGridLabel";
            this.SkelNestingGridLabel.Size = new System.Drawing.Size(224, 13);
            this.SkelNestingGridLabel.TabIndex = 17;
            this.SkelNestingGridLabel.Text = "Skeleton logic leading to selected line";
            // 
            // VariableGridLabel
            // 
            this.VariableGridLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VariableGridLabel.AutoSize = true;
            this.VariableGridLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VariableGridLabel.Location = new System.Drawing.Point(633, 275);
            this.VariableGridLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.VariableGridLabel.Name = "VariableGridLabel";
            this.VariableGridLabel.Size = new System.Drawing.Size(175, 13);
            this.VariableGridLabel.TabIndex = 18;
            this.VariableGridLabel.Text = "Variables on the selected line";
            // 
            // ExpansionStatus
            // 
            this.ExpansionStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusHiddenLabel,
            this.StatusHiddenSkeletons,
            this.StatusFilter});
            this.ExpansionStatus.Location = new System.Drawing.Point(0, 471);
            this.ExpansionStatus.Name = "ExpansionStatus";
            this.ExpansionStatus.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.ExpansionStatus.Size = new System.Drawing.Size(1047, 22);
            this.ExpansionStatus.TabIndex = 19;
            // 
            // StatusHiddenLabel
            // 
            this.StatusHiddenLabel.Name = "StatusHiddenLabel";
            this.StatusHiddenLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // StatusHiddenSkeletons
            // 
            this.StatusHiddenSkeletons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StatusHiddenSkeletons.Image = ((System.Drawing.Image)(resources.GetObject("StatusHiddenSkeletons.Image")));
            this.StatusHiddenSkeletons.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StatusHiddenSkeletons.Name = "StatusHiddenSkeletons";
            this.StatusHiddenSkeletons.Size = new System.Drawing.Size(29, 20);
            this.StatusHiddenSkeletons.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.StatusHiddenSkeletons_DropDownItemClicked);
            // 
            // StatusFilter
            // 
            this.StatusFilter.Name = "StatusFilter";
            this.StatusFilter.Size = new System.Drawing.Size(0, 17);
            // 
            // BtnMap
            // 
            this.BtnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMap.Location = new System.Drawing.Point(777, 411);
            this.BtnMap.Name = "BtnMap";
            this.BtnMap.Size = new System.Drawing.Size(64, 57);
            this.BtnMap.TabIndex = 20;
            this.BtnMap.Text = "&Graphic View";
            this.BtnMap.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnMap.UseVisualStyleBackColor = true;
            this.BtnMap.Click += new System.EventHandler(this.BtnMap_Click);
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SearchToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.MenuStrip1.Size = new System.Drawing.Size(1047, 24);
            this.MenuStrip1.TabIndex = 21;
            this.MenuStrip1.Text = "menuStrip1";
            this.MenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuStrip1_ItemClicked);
            // 
            // SearchToolStripMenuItem
            // 
            this.SearchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiFindString,
            this.TsmiFindNext});
            this.SearchToolStripMenuItem.Name = "SearchToolStripMenuItem";
            this.SearchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.SearchToolStripMenuItem.Text = "Search";
            // 
            // TsmiFindString
            // 
            this.TsmiFindString.Name = "TsmiFindString";
            this.TsmiFindString.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.TsmiFindString.Size = new System.Drawing.Size(146, 22);
            this.TsmiFindString.Text = "Find...";
            this.TsmiFindString.Click += new System.EventHandler(this.TsmiFindString_Click);
            // 
            // TsmiFindNext
            // 
            this.TsmiFindNext.Name = "TsmiFindNext";
            this.TsmiFindNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.TsmiFindNext.Size = new System.Drawing.Size(146, 22);
            this.TsmiFindNext.Text = "Find next";
            this.TsmiFindNext.Click += new System.EventHandler(this.TsmiFindNext_Click);
            // 
            // QueryExpand2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 493);
            this.Controls.Add(this.VariableGridLabel);
            this.Controls.Add(this.SkelNestingGridLabel);
            this.Controls.Add(this.BtnMap);
            this.Controls.Add(this.DgvVariables);
            this.Controls.Add(this.DgvNesting);
            this.Controls.Add(this.DgvSkeletonExpansion);
            this.Controls.Add(this.ExpansionStatus);
            this.Controls.Add(this.MenuStrip1);
            this.Controls.Add(this.BtnExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(604, 251);
            this.Name = "QueryExpand2";
            this.Text = "Skeleton Expansion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QueryExpand2_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.DgvVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvNesting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSkeletonExpansion)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ExpansionStatus.ResumeLayout(false);
            this.ExpansionStatus.PerformLayout();
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.DataGridView DgvVariables;
        private System.Windows.Forms.DataGridView DgvNesting;
        private System.Windows.Forms.DataGridView DgvSkeletonExpansion;
        private System.Windows.Forms.Label SkelNestingGridLabel;
        private System.Windows.Forms.Label VariableGridLabel;
        private System.Windows.Forms.StatusStrip ExpansionStatus;
        private System.Windows.Forms.ToolStripDropDownButton StatusHiddenSkeletons;
        private System.Windows.Forms.ToolStripStatusLabel StatusFilter;
        private System.Windows.Forms.ToolStripStatusLabel StatusHiddenLabel;
        private System.Windows.Forms.Button BtnMap;
        private System.Windows.Forms.MenuStrip MenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TsmgFilters;
        private System.Windows.Forms.ToolStripMenuItem TsmiApplyTextFilter;
        private System.Windows.Forms.ToolStripSeparator sep1_1;
        private System.Windows.Forms.ToolStripMenuItem TsmiApplySkelFilter;
        private System.Windows.Forms.ToolStripSeparator TsmiRemoveSep;
        private System.Windows.Forms.ToolStripMenuItem TsmgRemoveFilters;
        private System.Windows.Forms.ToolStripMenuItem TsmiRemoveTextFilter;
        private System.Windows.Forms.ToolStripSeparator TsmiRemoveSkelSep;
        private System.Windows.Forms.ToolStripMenuItem TsmiRemoveSkelFilter;
        private System.Windows.Forms.ToolStripMenuItem TsmiRemoveSkelFilterAll;
        private System.Windows.Forms.ToolStripSeparator TsmiRemoveAllSep;
        private System.Windows.Forms.ToolStripMenuItem TsmiRemoveFilterAll;
        private System.Windows.Forms.ToolStripSeparator TsmiGotoSep;
        private System.Windows.Forms.ToolStripMenuItem TsmiGotoLine;
        private System.Windows.Forms.ToolStripMenuItem SearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TsmiFindString;
        private System.Windows.Forms.ToolStripMenuItem TsmiFindNext;
    }
}