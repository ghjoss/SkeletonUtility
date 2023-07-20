using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ISPFSkeletonParser;
using SkeletonParserDSDef;

namespace ISPFSkeletonParser
{
    public partial class QueryProgram : Form
    {
        class ProgGridFiller
        {
            public ProgGridFiller() { }
            public string Skeleton;
            public string Program;
            public int Line;
        }
        private ProgGridFiller[] _results;
        private BindingSource _tablesBindingSource = null;
        private SkeletonParserDS _DS = null;
        private TransientDS _TDS = null;
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
        public QueryProgram(SkeletonParserDS parserDS, TransientDS transientDS, ImageList iL, string displayConfigName,
            XMLParmsFileReader xmlr,Query parent)
        {
            _DS = parserDS;
            _TDS = transientDS;
            _IL = iL;
            _parent = parent;
            _parent.AddForm(this);
            _xmlr = xmlr;
            InitializeComponent();
            dgvPrograms.Top = menuStrip1.Top + menuStrip1.Bottom;
            menuStrip1.ShowItemToolTips = true;
            this.Text = displayConfigName + " - Executed Program Cross Reference";
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
            results = (from o in _DS.Programs.AsEnumerable()
                       where ((from p in _DS.Programs.AsEnumerable()
                               where p.Skeleton == o.Skeleton
                               select p.Skeleton).Count()) > 0
                       select o.Skeleton).Distinct().ToArray()
                       .Union(_allKeysArray).ToArray();


            ArrayList resultsS = new ArrayList(results);
            resultsS.Sort();
            cboSkeletons.DataSource = resultsS;

            results = (from o in _DS.Programs.AsEnumerable()
                       where ((from p in _DS.Programs.AsEnumerable()
                               where p.ProgramName == o.ProgramName
                               select p.ProgramName).Count()) > 0
                       select o.ProgramName).Distinct().ToArray()
                       .Union(_allKeysArray).ToArray();
            ArrayList resultsP = new ArrayList(results);
            resultsP.Sort();
            cboPrograms.DataSource = resultsP;

            dgvPrograms.AutoGenerateColumns = true;
            _results =
                    (from o in _DS.Programs.AsEnumerable()
                     orderby o.Skeleton
                     select new ProgGridFiller
                     {
                         Skeleton = o.Skeleton,
                         Program = o.ProgramName,
                         Line = o.LineNumber
                     }).ToArray();
            var results2 = from o in _results
                           orderby o.Skeleton, o.Line
                           select new
                           {
                               Skeleton = o.Skeleton,
                               Program = o.Program,
                               Line = o.Line
                           };
            _tablesBindingSource = new BindingSource();
            _tablesBindingSource.DataSource = results2;
            dgvPrograms.DataSource = _tablesBindingSource;
        }

        /// <summary>
        /// Get the Skeleton data to display.
        /// </summary>
        /// <param name="filter"></param>
        /// Either a skeleton and/or table in the list from (from the drop down ComboBoxes) or
        /// " -All Keys-". If " -All Keys- " then all skeletons and tables are displayed.
        /// Otherwise a skeleton is displayed if it matches the Skeleton column and/or a table
        /// if it matches the Table column.
        private void SkelQ(string skelName, string programName)
        {
            _results = (from o in _DS.Programs.AsEnumerable()
                        select new ProgGridFiller
                        {
                            Skeleton = o.Skeleton,
                            Program = o.ProgramName,
                            Line = o.LineNumber
                        }).ToArray();

            var results2 =
                (skelName == ALL_KEYS && programName == ALL_KEYS ?
                    (from o in _results
                     orderby o.Skeleton, o.Line
                     select new
                     {
                         Skeleton = o.Skeleton,
                         Program = o.Program,
                         Line = o.Line
                     })
                :
                skelName == ALL_KEYS && programName != ALL_KEYS ?
                    (from o in _results
                     where o.Program == programName.ToUpper()
                     orderby o.Skeleton, o.Line
                     select new
                     {
                         Skeleton = o.Skeleton,
                         Program = o.Program,
                         Line = o.Line
                     })
                :
                skelName != ALL_KEYS && programName == ALL_KEYS ?
                    (from o in _results
                     where o.Skeleton == skelName.ToUpper()
                     orderby o.Line
                     select new
                     {
                         Skeleton = o.Skeleton,
                         Program = o.Program,
                         Line = o.Line
                     })
                :
                    (from o in _results
                     where o.Skeleton == skelName.ToUpper()
                        && o.Program == programName.ToUpper()
                     orderby o.Line
                     select new
                     {
                         Skeleton = o.Skeleton,
                         Program = o.Program,
                         Line = o.Line
                     })).ToArray();


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
                dgvPrograms.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            else
                dgvPrograms.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(192,255,192);
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
            SkelQ(cboSkeletons.Text, cboPrograms.Text);
            dgvPrograms.Refresh();
        }

        private void QueryProgram_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parent.RemoveForm(this);
        }

        delegate void ProgFileWriter(List<SkeletonParser.Program> a, string b, bool c);
        private void ExportCSV_XML(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            bool process = false;
            ProgFileWriter vfWriter = null;
            if (sender.ToString() == exportCSVToolStripMenuItem.ToString())
            {
                sfd.Filter = "Comma Separated Values File (*.csv)|*.csv";
                process = true;
                vfWriter = SkeletonParser.SkeletonParserExcelFileWriter.ExcelCSVProgsWrite;
            }
            else if (sender.ToString() == exportXMLToolStripMenuItem.ToString())
            {
                sfd.Filter = "XML (*.xml)|*.xml";
                process = true;
                vfWriter = SkeletonParser.SkeletonParserXMLFileWriter.XMLProgsWriter;
            }
            if (process)
            {
                sfd.RestoreDirectory = true;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    List<SkeletonParser.Program> _progList = new List<SkeletonParser.Program>();

                    var progList = (from o in _results
                                    join oL in _TDS.SkeletonLines
                                    on new { Skeleton = o.Skeleton, Line = o.Line }
                                    equals new { Skeleton = oL.Skeleton, Line = oL.LineNumber }
                                    orderby o.Skeleton, o.Program, o.Line
                                    select new SkeletonParser.Program
                                    {
                                        _skeleton = o.Skeleton,
                                        _program = o.Program,
                                        _lineNumber = o.Line,
                                        _inputTextLine = oL.LineText
                                    }).ToArray();
                    foreach (SkeletonParser.Program p in progList)
                        _progList.Add(p);
                    vfWriter(_progList, sfd.FileName, true);
                }
            }

        }

        private void exportCSVToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void dgvPrograms_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool dispose = false;
            QueryExpand2 qe2 = null;
            int row = Convert.ToInt32(dgvPrograms.Rows[e.RowIndex].Cells[2].Value);
            string skel = dgvPrograms.Rows[e.RowIndex].Cells[0].Value.ToString();

            qe2 = new QueryExpand2(out dispose, skel, _DS, _TDS, _IL, skel, _xmlr, _parent, false);
            qe2.Show();
            qe2.ExternalGotLineRequest(row);
        }

    }
}
