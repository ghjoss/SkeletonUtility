using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pitman.Printing;

namespace SkeletonParserQuery
{
    public partial class QueryExpand1 : Form
    {
        QueryExpand2 _parent;
        TreeNode _tn;
        public QueryExpand1(string skelName, TreeNode tN, ImageList iL, QueryExpand2 parent)
        {
            InitializeComponent();
            _tn = tN;
            tvExpansion.Nodes.Add(tN);
            tvExpansion.ExpandAll();
            tvExpansion.Refresh();
            btnExit.ImageList = iL;
            btnExit.ImageKey = "pgmReturn.ico";
            btnExit.ImageAlign = ContentAlignment.TopCenter;
            btnCollapse.ImageList = iL;
            btnCollapse.ImageKey = "CollapseGraph";
            btnCollapse.ImageAlign = ContentAlignment.TopCenter;
            btnExpand.ImageList = iL;
            btnExpand.ImageKey = "ExpandGraph";
            btnExpand.ImageAlign = ContentAlignment.TopCenter;
            _parent = parent;
            _parent.AddForm(this);
            this.Text = this.Text + skelName;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QueryExpand1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //_parent.RemoveForm(this);
            tvExpansion.Nodes.Remove(_tn);
        }

        private void BtnCollapse_Click(object sender, EventArgs e)
        {
            tvExpansion.CollapseAll();
        }

        private void BtnExpand_Click(object sender, EventArgs e)
        {
            tvExpansion.ExpandAll();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "jpeg (*.jpg)|*.jpg|bitmap (*.bmp)|*.bmp|gif (*.gif)|*.gif";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            DialogResult dr = saveFileDialog1.ShowDialog();
            string fileExt;
            
            if (dr == DialogResult.OK)
            {
                fileExt = new System.IO.FileInfo(saveFileDialog1.FileName).Extension;
                TreeView tv = new TreeView();
                foreach (TreeNode tn in tvExpansion.Nodes)
                    tv.Nodes.Add((TreeNode)tn.Clone());
                Pitman.Printing.PrintHelper ph = new PrintHelper();

                Image i = ph.PrepareTreeImage(tvExpansion, tv);
                if (fileExt.ToLower() == ".gif")
                    i.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                else if (fileExt.ToLower() == ".bmp")
                    i.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                else
                    i.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void QueryExpand1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _parent.RemoveForm(this);
            this.Dispose();
        }
    }
}
