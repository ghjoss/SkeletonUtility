using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pitman.Printing;
using SkeletonParserDSDef;
using ISPFSkeletonParser;

namespace SkeletonParserQuery
{
    public partial class QueryExpand1 : Form
    {
        Query _root;
        TreeNode _tn;
        ImageList _iL;
        SkeletonParserDS _SDS;
        TransientDS _TDS;
        XMLParmsFileReader _xmlr;
        public QueryExpand1(string skelName, TreeNode tN, ImageList iL, Query root, SkeletonParserDS SDS,TransientDS TDS, XMLParmsFileReader xmlr)
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
            _root = root;
            _root.AddForm(this);
            this.Text = this.Text + skelName;
            _iL = iL;
            _SDS = SDS;
            _TDS = TDS;
            _xmlr = xmlr;
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
            _root.RemoveForm(this);
            this.Dispose();
        }

        private void TvExpansion_DoubleClick(object sender, EventArgs e)
        {
            bool _dispose = false;
            string skel = tvExpansion.SelectedNode.Text;
            /*
             * string msg =  "SDS: " + _SDS.ToString() + " \nTDS: " + _TDS.ToString() + "\nIL: " + _iL.ToString() + "\nXMLRDR: " + _xmlr.ToString() + "\nSkel:" + skel;
             * MessageBox.Show(msg);
            */
            string lbl = skel + " expansion";
            QueryExpand2 qExp = new QueryExpand2(out _dispose, skel, this._SDS, this._TDS, this._iL, lbl, this._xmlr, _root, true);
            qExp.Show();
        }
    }
}
