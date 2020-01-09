using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SkeletonParserDSDef;
using ISPFSkeletonParser;

namespace SkeletonParserQuery
{
    public partial class QueryDOT : Form
    {
        class DOTGridFiller 
        {
            public string  Skeleton;
            public string Table;
            public int Line;

            public DOTGridFiller() {}
       }
        //class SkelOffsetQ
        //{
        //    public short Offset;

        //    public SkelOffsetQ() {}
        //}

        private DOTGridFiller[] _results;
        private BindingSource _tablesBindingSource = null;
        private SkeletonParserDS _DS = null;
        private TransientDS _TS = null;
        private ImageList _IL = null;
        private const string ALL_KEYS = " -All- ";
        private string[] _allKeysArray = new string[] { ALL_KEYS };
        private Query _parent;
        private XMLParmsFileReader _xmlr;
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parserDS"></param>
        /// The strongly typed DataSet that contains the output from a parser run.
        /// <param name="iL"></param>
        /// The image list passed by the Query form that invokes this form. 
        /// <param name="displayConfigName"></param>
        /// The name of the configuration from the XML parameters file processed by the
        /// Query form.
        public QueryDOT(SkeletonParserDS parserDS,  TransientDS parserTS, ImageList iL, 
            string displayConfigName, XMLParmsFileReader xmlrdr, Query parent)
        {
            _DS = parserDS;
            _TS = parserTS;
            _IL = iL;
            _parent = parent;
            _parent.AddForm(this);
            _xmlr = xmlrdr;
            InitializeComponent();
            dgvTables.Top = menuStrip1.Top + menuStrip1.Bottom;
            menuStrip1.ShowItemToolTips = true;
            this.Text = displayConfigName + " - )DOT Cross Reference";
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
            results = (from o in _DS.DOT.AsEnumerable()
                       where ((from p in _DS.DOT.AsEnumerable()
                               where p.Skeleton == o.Skeleton
                               select p.Skeleton).Count()) > 0
                       select o.Skeleton).Distinct().ToArray()
                       .Union(_allKeysArray).ToArray();

            
            ArrayList resultsS = new ArrayList(results);
            resultsS.Sort();
            cboSkeletons.DataSource = resultsS;

            results = (from o in _DS.DOT.AsEnumerable()
                       where ((from p in _DS.DOT.AsEnumerable()
                               where p.TableName == o.TableName
                               select p.TableName).Count()) > 0
                       select o.TableName).Distinct().ToArray()
                       .Union(_allKeysArray).ToArray();
            ArrayList resultsT = new ArrayList(results);
            resultsT.Sort();
            cboTables.DataSource = resultsT;

            dgvTables.AutoGenerateColumns = true;
            _results =
                    (from o in _DS.DOT.AsEnumerable()
                     orderby o.Skeleton, o.LineNumber
                     select new DOTGridFiller
                     {
                         Skeleton = o.Skeleton,
                         Table = o.TableName,
                         Line = o.LineNumber
                     }).ToArray();

            var results2 =
                (from o in _results
                 select new
                 {
                     Skeleton = o.Skeleton,
                     Table = o.Table,
                     Line = o.Line
                 });
            _tablesBindingSource = new BindingSource();
            _tablesBindingSource.DataSource = results2;
            dgvTables.DataSource = _tablesBindingSource;
        }

        /// <summary>
        /// Get the Skeleton data to display.
        /// </summary>
        /// <param name="filter"></param>
        /// Either a skeleton and/or table in the list from (from the drop down ComboBoxes) or
        /// " -All Keys-". If " -All Keys- " then all skeletons and tables are displayed.
        /// Otherwise a skeleton is displayed if it matches the Skeleton column and/or a table
        /// if it matches the Table column.
        private void SkelQ(string skelName, string tableName)
        {
            _results =
                skelName == ALL_KEYS && tableName == ALL_KEYS ?
                    (from o in _DS.DOT.AsEnumerable()
                    orderby o.Skeleton, o.LineNumber
                    select new DOTGridFiller
                    {
                       Skeleton = o.Skeleton,
                        Table = o.TableName,
                        Line = o.LineNumber
                    }).ToArray()
                :
                skelName == ALL_KEYS && tableName != ALL_KEYS ?
                    (from o in _DS.DOT.AsEnumerable()
                     where o.TableName == tableName.ToUpper()
                     orderby o.Skeleton, o.LineNumber
                     select new DOTGridFiller
                     {  
                         Skeleton = o.Skeleton,
                        Table = o.TableName,
                        Line = o.LineNumber
                     }).ToArray()
                :
                skelName != ALL_KEYS && tableName == ALL_KEYS ?
                    (from o in _DS.DOT.AsEnumerable()
                     where o.Skeleton == skelName.ToUpper()
                     orderby o.LineNumber
                     select new DOTGridFiller
                    {
                        Skeleton = o.Skeleton,
                        Table = o.TableName,
                        Line = o.LineNumber
                    }).ToArray()
                :
                    (from o in _DS.DOT.AsEnumerable()
                     where o.Skeleton == skelName.ToUpper()
                        && o.TableName == tableName.ToUpper()
                     orderby o.LineNumber
                     select new DOTGridFiller
                    {
                        Skeleton = o.Skeleton,
                        Table = o.TableName,
                        Line = o.LineNumber
                    }).ToArray();
            var results2 =
                (from o in _results
                 select new
                 {
                     Skeleton = o.Skeleton,
                     Table = o.Table,
                     Line = o.Line
                 });

            _tablesBindingSource.DataSource = results2;
        }

        /// <summary>
        /// Event: paint the background of three rows white, then the next three rows green. Alternate
        /// over all visible rows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTables_RowPrepPaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 6 < 3)
                dgvTables.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            else
                dgvTables.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(192,255,192);
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
            SkelQ(ALL_KEYS, ALL_KEYS);
        }

        /// <summary>
        /// Filter the displayed content based upon the Skeleton name currently in the Text
        /// property of the combo box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            SkelQ(cboSkeletons.Text, cboTables.Text);
            dgvTables.Refresh();
        }

        private void QueryDOT_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parent.RemoveForm(this);
        }

        delegate void DotFileWriter(List<SkeletonParser.DOT> a,string b);

        private void ExportCSV_XML(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            bool process = false;
            DotFileWriter vfWriter = null;
            if (sender.ToString() == exportCSVToolStripMenuItem.ToString())
            {
                sfd.Filter = "Comma Separated Values File (*.csv)|*.csv";
                process = true;
                vfWriter = SkeletonParser.SkeletonParserExcelFileWriter.ExcelCSVDOTsWriter;
            }
            else if (sender.ToString() == exportXMLToolStripMenuItem.ToString())
            {
                sfd.Filter = "XML (*.xml)|*.xml";
                process = true;
                vfWriter = SkeletonParser.SkeletonParserXMLFileWriter.XMLDOTsWriter;
            }
            if (process)
            {
                sfd.RestoreDirectory = true;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    List<SkeletonParser.DOT> _csvList = new List<SkeletonParser.DOT>();

                    var dotList = (from o in _results
                                   orderby o.Skeleton, o.Table, o.Line
                                   select new SkeletonParser.DOT
                                   {
                                       _skeleton = o.Skeleton,
                                       _table = o.Table,
                                       _lineNumber = o.Line
                                   }).ToArray();
                    foreach (SkeletonParser.DOT d in dotList)
                        _csvList.Add(d);
                    vfWriter(_csvList, sfd.FileName);
                }
            }

        }

        private void dgvTables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool dispose = false;
            QueryExpand2 qe2 = null;
            string skel = dgvTables.Rows[e.RowIndex].Cells[0].Value.ToString();
            int row = Convert.ToInt32(dgvTables.Rows[e.RowIndex].Cells[2].Value);
            qe2 = new QueryExpand2(out dispose, skel,_DS, _TS, _IL, skel, _xmlr, _parent, false);
            qe2.Show();
            qe2.ExternalGotLineRequest(row);

        }
    }
}
