using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SkeletonParserDSDef;
using ISPFSkeletonParser;
using System.IO;

namespace SkeletonParserQuery
{
    public partial class QuerySkeleton : Form
    {
        class SkeletonGridFiller
        {
            public SkeletonGridFiller() { }
            public string Skeleton;
            public int Offset;
            public int Line;
            public string Imbedded;
            public int ImbedOffset;
            public bool EXT;
            public bool NOFT;
            public bool OPT;
            public string Ampersand;

        }
        private LibraryConcatenation _lc = null;
        private SkeletonGridFiller[] _results;
        private SkeletonParserDS _DS = null;
        private TransientDS _TDS = null;
        private ImageList _IL = null;
        private const string ALL_KEYS = " -All- ";
        private const string NO_REF = " -NoParent-";
        private SkeletonGridFiller[] _noRef;
        private string[] _allKeysArray = new string[] { ALL_KEYS, NO_REF };
        private Query _parent;
        private XMLParmsFileReader _xmlr;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parserDS"></param>
        /// The strongly typed DataSet that contains the output from a parser run.
        /// <param name="lc">The library concatenation dictionary, for each skeleton this defines where
        /// in the concatenation the skeleton was found.</param>
        /// <param name="iL"></param>
        /// The image list passed by the Query form that invokes this form. 
        /// <param name="displayConfigName"></param>
        /// The name of the configuration from the XML parameters file processed by the
        /// Query form.
        /// <param name="parent">For the purpose of updating the open Query forms count, this is the Query form that
        /// invoke *this* QuerySkeleton form.</param>
        public QuerySkeleton(SkeletonParserDS parserDS, TransientDS transientDS, LibraryConcatenation lc, ImageList iL, string displayConfigName,
            XMLParmsFileReader xmlr, Query parent)
        {
            _lc = lc;
            _DS = parserDS;
            _TDS = transientDS;
            _IL = iL;
            _parent = parent;
            _parent.AddForm(this);
            _xmlr = xmlr;
            InitializeComponent();
            dgvSkeletons.Top = menuStrip1.Top + menuStrip1.Bottom;
            menuStrip1.ShowItemToolTips = true;
            this.Text = displayConfigName + " - Skeleton Cross Reference";

            // get the skeleton names that are not referenced as child skeletons
            string[] _noRef1 = ((from o in _TDS.SkeletonLines
                                 select o.Skeleton).Distinct().Except(
                      from o2 in _DS.Skeletons
                      select o2.ChildSkeleton)).ToArray();

            _noRef = (from o in _noRef1
                      select new SkeletonGridFiller
                      {
                          Skeleton = NO_REF,
                          Offset = 0,
                          Line = 0,
                          Imbedded = o,
                          ImbedOffset = 0,
                          EXT = false,
                          NOFT = false,
                          OPT = false,
                          Ampersand = ""
                      }).ToArray();
            UserInitializeComponent();
        }
        /// <summary>
        /// Other initialization that takes place once the form components have been initialized by
        /// the generated code.
        /// </summary>
        private void UserInitializeComponent()
        {
            string[] results;
            // set the buttons with images in the ImageList passed by the Query form
            btnExit.ImageList = _IL;
            btnExit.ImageKey = "pgmReturn.ico";
            btnExit.ImageAlign = ContentAlignment.TopCenter;

            btnFilter.ImageList = _IL;
            btnFilter.ImageKey = "filter.ico";
            btnFilter.ImageAlign = ContentAlignment.TopCenter;

            btnAll.ImageList = _IL;
            btnAll.ImageKey = "FindAll.ico";
            btnAll.ImageAlign = ContentAlignment.TopCenter;
            //
            // Set the drop down combo box contents. Add the " -All Keys- " entry
            // Note: we dodge a bullet here since a ChildSkeleton (from second select) that is also 
            // a Skeleton (from first select) does not appear multiple times.
            results = (from o in _DS.Skeletons.AsEnumerable()
                       select o.Skeleton).Distinct().ToArray()
                       .Union(_allKeysArray).ToArray()
                       .Union(from o2 in _DS.Skeletons.AsEnumerable()
                              select o2.ChildSkeleton).Distinct().ToArray();


            ArrayList resultsS = new ArrayList(results);
            resultsS.Sort();
            cboSkeletons.DataSource = resultsS;

            SkelQ(ALL_KEYS);
        }
        /// <summary>
        /// Get the Skeleton data to display.
        /// </summary>
        /// <param name="filter"></param>
        /// Either a _skeleton in the list from (from the drop down ComboBox) or
        /// " -All Keys-". If " -All Keys- " then all skeletons are displayed.
        /// Otherwise a _skeleton is displayed if it matches the Skeleton column
        /// or the ChildSkeleton column, or both.
        private void SkelQ(string skelName)
        {

            _results = (

                /* **--1 --** */ skelName == ALL_KEYS ?

                 (from o in _DS.Skeletons //.AsEnumerable()
                  orderby o.Skeleton, o.ChildSkeleton
                  select new SkeletonGridFiller
                  {
                      Skeleton = o.Skeleton,
                      Offset = o.SkelOffset,
                      Line = o.LineNumber,
                      Imbedded = o.ChildSkeleton,
                      ImbedOffset = o.ChildSkelOffset,
                      EXT = o.EXT,
                      NOFT = o.NOFT,
                      OPT = o.OPT,
                      Ampersand = o.Amper
                  }
                  ).Union
                  (from sgf in _noRef
                   select sgf
                  )

                 /* **--2--** */
                 : skelName == NO_REF ?
                  (from sgf in _noRef
                   select sgf
                  )
                /* **--3--** */
                :
                (from o in _DS.Skeletons //.AsEnumerable()
                 where (o.Skeleton.ToUpper() == skelName.ToUpper() || o.ChildSkeleton.ToUpper() == skelName.ToUpper())
                 orderby o.Skeleton, o.LineNumber
                 select new SkeletonGridFiller
                 {
                     Skeleton = o.Skeleton,
                     Offset = o.SkelOffset,
                     Line = o.LineNumber,
                     Imbedded = o.ChildSkeleton,
                     ImbedOffset = o.ChildSkelOffset,
                     EXT = o.EXT,
                     NOFT = o.NOFT,
                     OPT = o.OPT,
                     Ampersand = o.Amper
                 }
                 ).Union
                 (from sgf in _noRef
                  where sgf.Imbedded == skelName.ToUpper()
                  select sgf)
            ).ToArray();

            var results2 =
                from o in _results
                orderby o.Skeleton, o.Line
                select new
                {
                    Skeleton = o.Skeleton,
                    Offset = o.Offset,
                    Line = o.Line,
                    Imbedded = o.Imbedded,
                    ImbedOffset = o.ImbedOffset,
                    EXT = o.EXT,
                    NOFT = o.NOFT,
                    OPT = o.OPT
                };
            BindingSource b1 = new BindingSource();
            b1.DataSource = results2;
            dgvSkeletons.DataSource = b1;
            dgvSkeletons.Refresh();

        }

        /// <summary>
        /// Event: paint the background of three rows white, then the next three rows green. Alternate
        /// over all visible rows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 6 < 3)
                dgvSkeletons.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            else
                dgvSkeletons.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(192,255,192);
        }

        /// <summary>
        /// Exit the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Force all keys to be displayed (call SkelQ with the " -All Keys-" parameter).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAll_Click(object sender, EventArgs e)
        {
            SkelQ(ALL_KEYS);
        }

        /// <summary>
        /// Filter the displayed content based upon the Skeleton name currently in the Text
        /// property of the combo box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            SkelQ(cboSkeletons.Text);
        }

        private void QuerySkeleton_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parent.RemoveForm(this);
        }

        delegate void SkelFileWriter(List<SkeletonParser.Skeleton> a, Dictionary<string, short> b, string c);

        private void ExportCSV_XML(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            bool process = false;
            SkelFileWriter vfWriter = null;
            if (sender.ToString() == exportCSVToolStripMenuItem.ToString())
            {
                sfd.Filter = "Comma Separated Values File (*.csv)|*.csv";
                process = true;
                vfWriter = SkeletonParser.SkeletonParserExcelFileWriter.ExcelCSVSkelsWriter;
            }
            else if (sender.ToString() == exportXMLToolStripMenuItem.ToString())
            {
                sfd.Filter = "XML (*.xml)|*.xml";
                process = true;
                vfWriter = SkeletonParser.SkeletonParserXMLFileWriter.XMLSkelsWriter;
            }
            if (process)
            {
                sfd.RestoreDirectory = true;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    List<SkeletonParser.Skeleton> _skelList = new List<SkeletonParser.Skeleton>();

                    SkeletonParser.Skeleton[] skelList =
                            (from o in _results
                             join oL in _DS.Skeletons
                             on new { Skeleton = o.Skeleton, Line = o.Line } equals
                                    new { Skeleton = oL.Skeleton, Line = oL.LineNumber }
                             orderby o.Skeleton, o.Line
                             select new SkeletonParser.Skeleton
                             {
                                 _skeleton = o.Skeleton,
                                 _lineNumber = o.Line,
                                 _concatOffsetparent = oL.SkelOffset,
                                 _childSkeleton = o.Imbedded,
                                 _concatOffsetChild = oL.SkelOffset,
                                 _extended = o.EXT,
                                 _noft = o.NOFT,
                                 _opt = o.OPT,
                                 _amper = o.Ampersand
                             }).ToArray();
                    foreach (SkeletonParser.Skeleton s in skelList)
                        _skelList.Add(s);
                    vfWriter(_skelList, _lc.MemberConcatenation, sfd.FileName);

                }
            }
        }

        private void exportUnreferencedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter sw;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML (*.xml)|*.xml";
            sfd.RestoreDirectory = true;
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                sw = new StreamWriter(sfd.FileName);
                foreach (SkeletonGridFiller sgf in _noRef)
                {
                    sw.WriteLine("<Unimbedded" +
                        " Skeleton=\"" + sgf.Imbedded + "\"" +
                        " Parent=\"" + NO_REF.Trim() + "\"" +
                        " NOFT=\"false\" OPT=\"false\" EXT=\"false\"/>");
                }
                sw.Close();
            }
        }


        private void dgvSkeletons_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool dispose = false;
            QueryExpand2 qe2 = null;
            int row = Convert.ToInt32(dgvSkeletons.Rows[e.RowIndex].Cells[2].Value);
            string skel;
            if (row == 0)
            {
                row = 1;
                skel = dgvSkeletons.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
            else
                skel = dgvSkeletons.Rows[e.RowIndex].Cells[0].Value.ToString();

            qe2 = new QueryExpand2(out dispose, skel, _DS, _TDS, _IL, skel, _xmlr, _parent, false);
            qe2.Show();
            qe2.ExternalGotLineRequest(row);
        }
    }
}