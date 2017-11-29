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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.cboSkeletons = new System.Windows.Forms.ComboBox();
            this.lblSkeletons = new System.Windows.Forms.Label();
            this.cboVariables = new System.Windows.Forms.ComboBox();
            this.lblVariables = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblType = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1218, 416);
            this.dataGridView1.TabIndex = 0;
            // 
            // cboSkeletons
            // 
            this.cboSkeletons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSkeletons.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboSkeletons.FormattingEnabled = true;
            this.cboSkeletons.Location = new System.Drawing.Point(12, 441);
            this.cboSkeletons.Name = "cboSkeletons";
            this.cboSkeletons.Size = new System.Drawing.Size(124, 21);
            this.cboSkeletons.TabIndex = 1;
            this.cboSkeletons.TextChanged += new System.EventHandler(this.cboSkeleton_TextChanged);
            // 
            // lblSkeletons
            // 
            this.lblSkeletons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSkeletons.AutoSize = true;
            this.lblSkeletons.Location = new System.Drawing.Point(12, 421);
            this.lblSkeletons.Name = "lblSkeletons";
            this.lblSkeletons.Size = new System.Drawing.Size(49, 13);
            this.lblSkeletons.TabIndex = 2;
            this.lblSkeletons.Text = "Skeleton";
            // 
            // cboVariables
            // 
            this.cboVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboVariables.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboVariables.FormattingEnabled = true;
            this.cboVariables.Location = new System.Drawing.Point(191, 441);
            this.cboVariables.Name = "cboVariables";
            this.cboVariables.Size = new System.Drawing.Size(119, 21);
            this.cboVariables.TabIndex = 3;
            this.cboVariables.TextChanged += new System.EventHandler(this.ComboBoxes_TextChanged);
            // 
            // lblVariables
            // 
            this.lblVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVariables.AutoSize = true;
            this.lblVariables.Location = new System.Drawing.Point(191, 421);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(45, 13);
            this.lblVariables.TabIndex = 4;
            this.lblVariables.Text = "Variable";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(1119, 429);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 33);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblType
            // 
            this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(365, 421);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 7;
            this.lblType.Text = "Type";
            // 
            // cboType
            // 
            this.cboType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(365, 441);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(119, 21);
            this.cboType.TabIndex = 6;
            this.cboType.TextChanged += new System.EventHandler(this.ComboBoxes_TextChanged);
            // 
            // Query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1218, 483);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblVariables);
            this.Controls.Add(this.cboVariables);
            this.Controls.Add(this.lblSkeletons);
            this.Controls.Add(this.cboSkeletons);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Query";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ComboBox cboSkeletons;
        private System.Windows.Forms.Label lblSkeletons;
        private System.Windows.Forms.ComboBox cboVariables;
        private System.Windows.Forms.Label lblVariables;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboType;


    }
}

