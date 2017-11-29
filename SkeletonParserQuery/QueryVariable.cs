using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ISPFSkeletonParser;
using SkeletonParserDSDef;
using System.IO;

namespace SkeletonParserQuery
{
    public partial class QueryVariable : Form
    {
        class VariableGridFiller
        {
            public VariableGridFiller() { }

            public string Skeleton;
            public string Variable;
            public int Line;
            public string Cmd;
            public string Type;
            public short Position;
        }
        private bool _disposed = false;
        private VariableGridFiller[] _results;
        private SkeletonParserDS _DS = null;
        private TransientDS _TDS = null;
        private ImageList _IL = null;
        private const string ALL_KEYS = " -All- ";
        private string[] _allKeysArray = new string[] { ALL_KEYS };
        private string _varName = ALL_KEYS;
        private Query _parent;
        private XMLParmsFileReader _xmlr;
        private BindingSource _b1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parserDS"></param>
        /// <param name="transientDS"></param>
        /// <param name="iL"></param>
        /// <param name="label"></param>
        /// <param name="parent"></param>
        public QueryVariable(SkeletonParserDS parserDS, TransientDS transientDS, ImageList iL, string label, XMLParmsFileReader xmlr,
            Query parent)
        {
            _DS = parserDS;
            _TDS = transientDS;
            _IL = iL;
            _parent = parent;
            _parent.AddForm(this);
            _varName = ALL_KEYS;
            _xmlr = xmlr;
          
            InitializeComponent();
            this.Text = label + " - Variable Cross Reference";
            UserInitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parserDS"></param>
        /// <param name="transientDS"></param>
        /// <param name="il"></param>
        /// <param name="label"></param>
        /// <param name="variable"></param>
        /// <param name="parent"></param>
        public QueryVariable(SkeletonParserDS parserDS, TransientDS transientDS, ImageList il, string label, string varName,
            XMLParmsFileReader xmlr,Query parent)
        {
            _DS = parserDS;
            _TDS = transientDS;
            _IL = il;
            _parent = parent;
            _parent.AddForm(this);
            _varName = varName;
            _xmlr = xmlr;
            InitializeComponent();
            this.Text = label + " - Variable Cross Reference for " + varName;
            UserInitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UserInitializeComponent()
        {
            dgvVariables.Top = menuStrip1.Top + menuStrip1.Bottom;
            menuStrip1.ShowItemToolTips = true;
            // initialize the control buttons with images passed from the Query or QueryExpansion
            // forms.   
            btnExit.ImageList = _IL;
            btnExit.ImageKey = "pgmReturn.ico";
            btnExit.ImageAlign = ContentAlignment.TopCenter;

            btnFilter.ImageList = _IL;
            btnFilter.ImageKey = "filter.ico";
            btnFilter.ImageAlign = ContentAlignment.TopCenter;

            btnAll.ImageList = _IL;
            btnAll.ImageKey = "FindAll.ico";
            btnAll.ImageAlign = ContentAlignment.TopCenter;

            // initialize the dropdown combo boxes and the data grid
            SkelQ(ALL_KEYS, _varName, ALL_KEYS);
        }

        /// <summary>
        /// Load the cboVariables, cboSkeletons and cboTypes drop down lists. 
        /// Load the main _variable datagrid.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="variable"></param>
        /// <param name="varType"></param>
        private void SkelQ(string skelName, string varName, string varType)
        {
            // load the cboVariables drop down combo box.
            // if from Query form, _varName is ALL_KEYS
            if (_varName == ALL_KEYS)
                CboVariables_LoadKeys(skelName, varName, varType);
            // if from QueryExpansion, _varName is the _variable being queried. Don't allow
            // changes from the cboVariables combo box (make it invisible).
            else
            {
                cboVariables.Visible = false;
                lblVariables.Visible = false;
            }

            // load the cboSkeletons drop down combo box
            CboSkeletons_LoadKeys(skelName, varName, varType);

            //load the cboType drop down combo box
            CboType_LoadKeys(skelName, varName, varType);

            // load the datagrid based upon any filters that may be active.
            _results = 
                (skelName == ALL_KEYS && varName == ALL_KEYS && varType == ALL_KEYS) 
              ?   (from o in _DS.Variables
                   select new VariableGridFiller { 
                      Skeleton = o.Skeleton, 
                      Variable = o.Variable, 
                      Line = o.LineNumber, 
                      Cmd = o.Command, 
                      Type = o.Type,
                      Position = o.Position}
                  ).ToArray() 
              : (skelName == ALL_KEYS && varName != ALL_KEYS && varType == ALL_KEYS) 
              ?  (from o in _DS.Variables
                  where o.Variable == varName
                  select new VariableGridFiller { 
                      Skeleton = o.Skeleton, 
                      Variable = o.Variable, 
                      Line = o.LineNumber, 
                      Cmd = o.Command,
                      Type = o.Type,
                      Position = o.Position
                  }
                 ).ToArray() 
              : (skelName == ALL_KEYS && varName == ALL_KEYS && varType != ALL_KEYS)
              ?  (from o in _DS.Variables
                  where o.Type == varType
                  select new VariableGridFiller { 
                      Skeleton = o.Skeleton, 
                      Variable = o.Variable, 
                      Line = o.LineNumber, 
                      Cmd = o.Command,
                      Type = o.Type,
                      Position = o.Position
                  }
                 ).ToArray() 
              : (skelName != ALL_KEYS && varName == ALL_KEYS && varType == ALL_KEYS) 
              ?  (from o in _DS.Variables
                  where o.Skeleton == skelName
                  select new VariableGridFiller { 
                      Skeleton = o.Skeleton, 
                      Variable = o.Variable, 
                      Line = o.LineNumber, 
                      Cmd = o.Command,
                      Type = o.Type,
                      Position = o.Position
                  }
                 ).ToArray() 
              : (skelName != ALL_KEYS && varName != ALL_KEYS && varType == ALL_KEYS)
              ?  (from o in _DS.Variables
                  where o.Skeleton == skelName && o.Variable == varName
                  select new VariableGridFiller { 
                      Skeleton = o.Skeleton, 
                      Variable = o.Variable, 
                      Line = o.LineNumber, 
                      Cmd = o.Command,
                      Type = o.Type,
                      Position = o.Position
                  }
                 ).ToArray() 
              : (skelName != ALL_KEYS && varName == ALL_KEYS && varType != ALL_KEYS)
              ?  (from o in _DS.Variables
                 where o.Skeleton == skelName && o.Type == varType
                 select new VariableGridFiller {
                     Skeleton = o.Skeleton,
                     Variable = o.Variable,
                     Line = o.LineNumber,
                     Cmd = o.Command,
                     Type = o.Type,
                     Position = o.Position}
                 ).ToArray() 
              : (skelName == ALL_KEYS && varName != ALL_KEYS && varType != ALL_KEYS)
              ?  (from o in _DS.Variables
                  where o.Variable == varName && o.Type == varType
                  select new VariableGridFiller { 
                      Skeleton = o.Skeleton, 
                      Variable = o.Variable, 
                      Line = o.LineNumber, 
                      Cmd = o.Command,
                      Type = o.Type,
                      Position = o.Position
                  }
                 ).ToArray()
             : (from o in _DS.Variables
                 where o.Skeleton == skelName
                 && o.Variable == varName
                 && o.Type == varType
                 select new VariableGridFiller { 
                     Skeleton = o.Skeleton, 
                     Variable = o.Variable, 
                     Line = o.LineNumber, 
                     Cmd = o.Command,
                     Type = o.Type,
                     Position = o.Position
                 }
                ).ToArray();

            // apply any selected user sorting (orderby)
            string order = cboOrderBy.Text;

            var results2 =
                   order == "Variable Name, Skeleton Name"
                 ? (from o in _results
                    orderby o.Variable, o.Skeleton
                    select new { o.Skeleton, o.Variable, o.Line, o.Cmd, o.Type })
                 : order == "Skeleton Name, Line Number"
                 ? (from o in _results
                    orderby o.Skeleton, o.Line
                    select new { o.Skeleton, o.Variable, o.Line, o.Cmd, o.Type })
                 :
                    (from o in _results
                     orderby o.Skeleton, o.Variable
                     select new { o.Skeleton, o.Variable, o.Line, o.Cmd, o.Type })
                ;
            
            _b1 = new BindingSource
            {
                DataSource = results2
            };
            dgvVariables.DataSource = _b1;
            dgvVariables.Refresh();
        }

        /// <summary>
        /// Load the cboSkeletons drop down combo. Note: The names returned may be filtered by
        /// _variable name and type name also.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="variable"></param>
        /// <param name="typeName"></param>
        private void CboSkeletons_LoadKeys(string skelName, string varName, string typeName)
        {
            string[] results = null;
            // Build the cboSkeletons drop down.
            if (skelName == ALL_KEYS && varName == ALL_KEYS && typeName == ALL_KEYS)
                results = (from o in _DS.Variables.AsEnumerable()
                           select o.Skeleton).Distinct().ToArray().Union(_allKeysArray).ToArray();
            else if (skelName == ALL_KEYS && varName == ALL_KEYS && typeName != ALL_KEYS)
                results = (from o in _DS.Variables.AsEnumerable()
                           where o.Type == typeName
                           select o.Skeleton).Distinct().ToArray().Union(_allKeysArray).ToArray();
            else if (skelName == ALL_KEYS && varName != ALL_KEYS && typeName == ALL_KEYS)
//                if (_varName == ALL_KEYS)
                    results = (from o in _DS.Variables.AsEnumerable()
                               where o.Variable == varName
                               select o.Skeleton).Distinct().ToArray().Union(_allKeysArray).ToArray();
//                else
//                    results = (from o in _DS.Variables.AsEnumerable()
//                               where o.Variable == varName
//                               select o.Skeleton).Distinct().ToArray();
            else if (skelName == ALL_KEYS && varName != ALL_KEYS && typeName != ALL_KEYS)
//                if (_varName == ALL_KEYS)
                    results = (from o in _DS.Variables.AsEnumerable()
                               where o.Variable == varName && o.Type == typeName
                               select o.Skeleton).Distinct().ToArray().Union(_allKeysArray).ToArray();
//                else
//                    results = (from o in _DS.Variables.AsEnumerable()
//                               where o.Variable == varName && o.Type == typeName
//                               select o.Skeleton).Distinct().ToArray();
            else if (skelName != ALL_KEYS && varName == ALL_KEYS && typeName == ALL_KEYS)
                results = (from o in _DS.Variables.AsEnumerable()
                           where o.Skeleton == skelName
                           select o.Skeleton).Distinct().ToArray().Union(_allKeysArray).ToArray();
            else if (skelName != ALL_KEYS && varName == ALL_KEYS && typeName != ALL_KEYS)
                results = (from o in _DS.Variables.AsEnumerable()
                           where o.Skeleton == skelName && o.Type == typeName
                           select o.Skeleton).Distinct().ToArray().Union(_allKeysArray).ToArray();
            else if (skelName != ALL_KEYS && varName != ALL_KEYS && typeName == ALL_KEYS)
//                if (_varName == ALL_KEYS)
                    results = (from o in _DS.Variables.AsEnumerable()
                               where o.Skeleton == skelName && o.Variable == varName
                               select o.Skeleton).Distinct().ToArray().Union(_allKeysArray).ToArray();
//                else
//                    results = (from o in _DS.Variables.AsEnumerable()
//                               where o.Skeleton == skelName && o.Variable == varName
//                               select o.Skeleton).Distinct().ToArray();
            else if (skelName != ALL_KEYS && varName != ALL_KEYS && typeName != ALL_KEYS)
//                if (_varName == ALL_KEYS)
                    results = (from o in _DS.Variables.AsEnumerable()
                               where o.Skeleton == skelName && o.Variable == varName && o.Type == typeName
                               select o.Skeleton).Distinct().ToArray().Union(_allKeysArray).ToArray();
//                else
//                    results = (from o in _DS.Variables.AsEnumerable()
//                               where o.Skeleton == skelName &&  o.Variable == varName && o.Type == typeName
//                               select o.Skeleton).Distinct().ToArray();
            ArrayList resultsS = new ArrayList(results);
            resultsS.Sort();
            cboSkeletons.DataSource = resultsS;
            cboSkeletons.SelectedItem = skelName;
        }
        /// <summary>
        /// Load the drop down combo box for _variable names with the unique data for searching.
        /// </summary>
        /// <param name="filter">
        /// If the filter is not " -All Keys- " then the _variable combo box and 
        /// vartypes combo box will contain only the data relevant to filter.</param>
        /// <param name="variable"></param>
        /// <param name="typeName"></param>
        private void CboVariables_LoadKeys(string skelName, string varName, string typeName)
        {
            var results = (skelName == ALL_KEYS && typeName == ALL_KEYS) ?
                (from o in _DS.Variables.AsEnumerable()
                           select o.Variable).Distinct().ToArray().Union(_allKeysArray).ToArray()
                :
                (skelName != ALL_KEYS && typeName == ALL_KEYS) ?
                (from o in _DS.Variables.AsEnumerable()
                 where o.Skeleton == skelName
                 select o.Variable).Distinct().ToArray().Union(_allKeysArray).ToArray()
                :
                (skelName == ALL_KEYS && typeName != ALL_KEYS) ?
                (from o in _DS.Variables.AsEnumerable()
                 where o.Type == typeName
                 select o.Variable).Distinct().ToArray().Union(_allKeysArray).ToArray()
                :
                (from o in _DS.Variables.AsEnumerable()
                 where o.Skeleton == skelName && o.Type == typeName
                 select o.Variable).Distinct().ToArray().Union(_allKeysArray).ToArray()
            ;
            ArrayList resultsV = new ArrayList(results);
            resultsV.Sort();
            cboVariables.DataSource = resultsV;
            cboVariables.SelectedItem = varName;
        }

        /// <summary>
        /// Load the drop down combo box for reference types with the unique data for searching.
        /// </summary>
        /// <param name="skelName">
        /// If the skelName is not " -All Keys- " then the _variable combo box and 
        /// vartypes combo box will contain only the data relevant to skelName.</param>
        /// <param name="variable"></param>
        /// <param name="typeName"></param>
        private void CboType_LoadKeys(string skelName, string varName, string typeName)
        {
            var results = (skelName == ALL_KEYS && varName == ALL_KEYS ) ?
                (from o in _DS.Variables.AsEnumerable()
                       select o.Type).Distinct().ToArray().Union(_allKeysArray).ToArray()
                :
                (skelName == ALL_KEYS && varName != ALL_KEYS) ?
                (from o in _DS.Variables.AsEnumerable()
                       where o.Variable == varName
                       select o.Type).Distinct().ToArray().Union(_allKeysArray).ToArray()
                :
                (skelName != ALL_KEYS && varName == ALL_KEYS) ?
                (from o in _DS.Variables.AsEnumerable()
                       where o.Skeleton == skelName
                       select o.Type).Distinct().ToArray().Union(_allKeysArray).ToArray()
                :
                (from o in _DS.Variables.AsEnumerable()
                    where o.Skeleton == skelName && o.Variable == varName
                    select o.Type).Distinct().ToArray().Union(_allKeysArray).ToArray()
            ;
            ArrayList resultsT = new ArrayList(results);
            resultsT.Sort();
            cboType.DataSource = resultsT;
            cboType.SelectedItem = typeName;
        }

        /// <summary>
        /// Event: paint the background of three rows white, then the next three rows green. Alternate
        /// over all visible rows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvVariables_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 6 < 3)
                dgvVariables.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            else
                dgvVariables.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(192,255,192);
        }

        /// <summary>
        /// Force all keys to be displayed. The _variable name may be either ALL_KEYS or an
        /// actual _variable name if this form was invoked from QueryExpansion. The _skeleton and
        /// type will be passed as ALL_KEYS.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAll_Click(object sender, EventArgs e)
        {
            SkelQ(ALL_KEYS, _varName, ALL_KEYS);
            if (cboVariables.Visible)
                cboVariables.SelectedIndex = 0;
            cboSkeletons.SelectedIndex = 0;
            cboType.SelectedIndex = 0;
        }

        /// <summary>
        /// Filter the displayed content based upon the Skeleton name currently in the Text
        /// property of the combo boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFilter_Click(object sender, EventArgs e)
        {
            string varName = _varName == ALL_KEYS ? cboVariables.Text : _varName;
            SkelQ(cboSkeletons.Text, varName , cboType.Text);
        }

        /// <summary>
        /// Exit the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QueryVariable_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parent.RemoveForm(this);
        }

        delegate void varFileWriter(List<SkeletonParser.Variable> a,string b,bool c);

        private void ExportCSV_XML(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            bool process = false;
            varFileWriter vfWriter = null;
            if (sender.ToString() == exportCSVToolStripMenuItem.ToString())
            {
                sfd.Filter = "Comma Separated Values File (*.csv)|*.csv";
                process = true;
                vfWriter = SkeletonParser.SkeletonParserExcelFileWriter.ExcelCSVVarsWriter;
            }
            else if (sender.ToString() == exportXMLToolStripMenuItem.ToString())
            {
                sfd.Filter = "XML (*.xml)|*.xml";
                process = true;
                vfWriter = SkeletonParser.SkeletonParserXMLFileWriter.XMLVarsWriter;
            }
            if (process)
            {
                sfd.RestoreDirectory = true;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    List<SkeletonParser.Variable> _varList = new List<SkeletonParser.Variable>();

                    var varList = (from o in _results
                                   join oL in _TDS.SkeletonLines
                                   on new { o.Skeleton, o.Line } equals new { Skeleton = oL.Skeleton, Line = oL.LineNumber }
                                   orderby o.Skeleton, o.Variable, o.Line
                                   select new SkeletonParser.Variable
                                   {
                                       _inputTextLine = oL.LineText,
                                       _variable = o.Variable,
                                       _typeCode = SkeletonParserRE.cVarTypes[o.Type],
                                       _skeleton = o.Skeleton,
                                       _lineNumber = o.Line,
                                       _position = o.Position,
                                       _commandCode = SkeletonParserRE.cKeywords[o.Cmd]
                                   }).ToArray();
                    foreach (SkeletonParser.Variable v in varList)
                        _varList.Add(v);
                    vfWriter(_varList, sfd.FileName,true); // dbgInputLine = true causes Line text to print

                }
            }

        }

        public new void Dispose()
        {
            Dispose(true);
        }

        private void ExportCSV11ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Comma Separated Values File (*.csv)|*.csv",
                RestoreDirectory = true
            };
            StreamWriter sw = null;
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                List<SkeletonParser.Variable> _varList = new List<SkeletonParser.Variable>();
                string[] varList = (from o in _results
                               orderby o.Skeleton, o.Variable
                               select o.Skeleton.Trim()+","+ o.Variable.Trim()
                               ).Distinct().ToArray();
                
                sw = new StreamWriter(sfd.FileName);
                sw.WriteLine("Skeleton,Variable");
                foreach (string s in varList)
                {
                    sw.WriteLine(s);
                }
                sw.Close();
            }
        }

        private void DgvVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool dispose = false;
            QueryExpand2 qe2 = null;
            int row = Convert.ToInt32(dgvVariables.Rows[e.RowIndex].Cells[2].Value);
            string skel = dgvVariables.Rows[e.RowIndex].Cells[0].Value.ToString();

            qe2 = new QueryExpand2(out dispose, skel, _DS, _TDS, _IL, skel, _xmlr, _parent, false);
            qe2.Show();
            qe2.ExternalGotLineRequest(row);

        }
    }
}
