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
            this.btnVariables = new System.Windows.Forms.Button();
            this.btnSkeleton = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnVariables
            // 
            this.btnVariables.Location = new System.Drawing.Point(25, 50);
            this.btnVariables.Name = "btnVariables";
            this.btnVariables.Size = new System.Drawing.Size(75, 23);
            this.btnVariables.TabIndex = 0;
            this.btnVariables.Text = "&Variables";
            this.btnVariables.UseVisualStyleBackColor = true;
            this.btnVariables.Click += new System.EventHandler(this.btnVariables_Click);
            // 
            // btnSkeleton
            // 
            this.btnSkeleton.Location = new System.Drawing.Point(25, 21);
            this.btnSkeleton.Name = "btnSkeleton";
            this.btnSkeleton.Size = new System.Drawing.Size(75, 23);
            this.btnSkeleton.TabIndex = 1;
            this.btnSkeleton.Text = "&Skeletons";
            this.btnSkeleton.UseVisualStyleBackColor = true;
            this.btnSkeleton.Click += new System.EventHandler(this.btnSkeleton_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(203, 166);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 201);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSkeleton);
            this.Controls.Add(this.btnVariables);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Query";
            this.Text = "Query";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnVariables;
        private System.Windows.Forms.Button btnSkeleton;
        private System.Windows.Forms.Button btnExit;
    }
}