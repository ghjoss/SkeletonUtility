using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;
using SkeletonParserDSDef;
using ISPFSkeletonParser;
using GlenwoodUtilities;

namespace SkeletonParserQuery
{
    public partial class QueryExpand2 : Form
    {
        private int[] _findResults = null;
        private int _findOffset = 0;
        private string _findString = "";
        private bool _matchCase = false;
        private bool _matchWord = false;
        private Stack<TransientDS.SkeletonExpansionRow>
            _dotNestingStack = new Stack<TransientDS.SkeletonExpansionRow>(16); // really only need 3
        private Stack<TransientDS.SkeletonExpansionRow>
            _selNestingStack = new Stack<TransientDS.SkeletonExpansionRow>(40); // really only need 32
        private Stack<TransientDS.SkeletonExpansionRow>
            _doNestingStack = new Stack<TransientDS.SkeletonExpansionRow>(100);

        private SkeletonParserDS _SDS;          // the filled parser info data structures
        private TransientDS _TDS;               // transient data structures
        private ImageList _IL;                  // the application image list
        private Query _parent;              // the calling form, for referencing its status bar
        private QueryExpand1 _parent1;
        private Stack<Nesting> _NestingStack;   // stack of )xxx skel commands active when a line is read
        private TransientDS.CommandNestingDataTable _cnt;
        private TransientDS.SkeletonExpansionDataTable _skelExpansion;
        private DataView _dvSkelExpansion;

        private int _offset;                     // current line in full expansion
        ArrayList _imbedChain = new ArrayList();
        ArrayList _skelHide = new ArrayList(32);

        private ToolStripDropDown _dropDown = new ToolStripDropDown();

        private string
            _filterString = "",
            _nestingGridLabel = "",
            _skelName = "";

        private bool _fullExpand = true;

        private TreeNode _expansionRoot = null;

        private BindingSource _expansionBindingSource;

        private XMLParmsFileReader _xmlParmsReader;

        /// <summary>
        /// Form constructor
        /// </summary>
        /// <param name="dispose">return value: if false the expand failed.</param>
        /// <param name="skelName">The top level skeleton to expand.</param>
        /// <param name="parserSDS">Parsed skeleton data (skels, variables, ...)</param>
        /// <param name="parserTDS">Transient datasets. The expansion def lives here</param>
        /// <param name="iL">The application'buffer image list, kept on the parent form (Query)</param>
        /// <param name="displayConfigName">
        /// The configuration name from the XML config file read when
        /// the application started up, or when a new config is loaded. Displays on the Expansion
        /// title bar.
        /// </param>
        /// <param name="xmlParmsRdr">The configuration data, accessible here to allow checking what
        /// to do on recursive imbeds.
        /// </param>
        /// <param name="parent"></param>
        public QueryExpand2(out bool dispose, string skelName, SkeletonParserDS parserSDS, TransientDS parserTDS,
            ImageList iL, string displayConfigName, XMLParmsFileReader xmlParmsRdr, Query parent, bool fullExpand = true)
        {
            _skelExpansion = new TransientDS.SkeletonExpansionDataTable();
            _cnt = new TransientDS.CommandNestingDataTable();
            _NestingStack = new Stack<Nesting>(100); // 32 SEL + 16 IM + 3 DOT + padding
            InitializeComponent();
            _parent = parent;
            _parent.AddForm(this);

            QueryExpand2Common(out dispose, skelName, parserSDS, parserTDS, iL, displayConfigName, xmlParmsRdr,
                fullExpand);
            if (!dispose)
                LoadGrid();
        }

        public QueryExpand2(out bool dispose, string[] skelNames, SkeletonParserDS parserSDS, TransientDS parserTDS,
             ImageList iL, string displayConfigName, XMLParmsFileReader xmlParmsRdr, Query parent, bool fullExpand = true)
        {
            _skelExpansion = new TransientDS.SkeletonExpansionDataTable();
            _cnt = new TransientDS.CommandNestingDataTable();
            _NestingStack = new Stack<Nesting>(100); // 32 SEL + 16 IM + 3 DOT + padding
            InitializeComponent();

            dispose = true;
            _parent = parent;
            _parent.AddForm(this);

            foreach (string s in skelNames)
            {
                if (s == Query.NULL_SKEL)
                    break;

                QueryExpand2Common(out dispose, s, parserSDS, parserTDS, iL, displayConfigName, xmlParmsRdr,
                        fullExpand);
                if (dispose)
                    break;
            }
            if (!dispose)
                LoadGrid();
        }

        private int Find(string s, bool matchCase, bool matchWord)
        {
            RegexOptions ro = RegexOptions.None;
            // replace any Regex special characters that the user typed with escaped (\) versions of those characters
            s = s
                .Replace(@"\", @"\\")
                .Replace(@"(", @"\(")
                .Replace(@")", @"\)")
                .Replace(@"[", @"\[")
                .Replace(@".", @"\.")
                .Replace(@"*", @"\*")
                .Replace(@"+", @"\+")
                .Replace(@"?", @"\?")
                .Replace(@"^", @"\^")
                .Replace(@"$", @"\$")
                .Replace(@"&", @"\&")
                .Replace(@"}", @"\}")
                .Replace(@"{", @"\{")
                .Replace(@":", @"\:")
                .Replace(@"<", @"\<")
                .Replace(@">", @"\>")
                .Replace(@"#", @"\#")
                .Replace(@"=", @"\=")
                .Replace(@"-", @"\-")
                ;

            if (matchWord)
                s = @"\b" + s + @"\b";

            if (!matchCase)
                ro = RegexOptions.IgnoreCase;

            _findResults = (from o in _skelExpansion
                               where Regex.Match(o.LineText,s,ro).Success
                               orderby o.Offset
                               select o.Offset).ToArray<int>();
            if (_findResults.Length > 0)
                return _findOffset = 0;
            else
                return -1;
        }

        private int FindNext()
        {   
            if (_findResults != null)
                if (_findOffset < _findResults.Length - 1)
                    return _findOffset + 1;
                else
                    return -1;
            else
                return -1;
        }

        private void LoadGrid()
        {
            DataGridViewColumn dc;

            // use a binding source so that Row filtering is enabled.
            _expansionBindingSource = new BindingSource();

            _dvSkelExpansion = _skelExpansion.AsDataView();
            _dvSkelExpansion.Sort = "Offset";
            _expansionBindingSource.DataSource = _dvSkelExpansion;
            _expansionBindingSource.Filter = "";
            DgvSkeletonExpansion.DataSource = _expansionBindingSource;

            DgvSkeletonExpansion.Refresh();
            //if (DgvSkeletonExpansion.ColumnCount > 0)
            //{
            DataGridViewCellStyle dgv1_columnHeaderStyle = new DataGridViewCellStyle
            {
                // use a fixed width font for the skeleton text and its header 'ruler'
                Font = new Font("Lucida Console", 11.5F, FontStyle.Regular)
             };
            DgvSkeletonExpansion.Columns[4].DefaultCellStyle = dgv1_columnHeaderStyle;
            DgvSkeletonExpansion.Columns[4].HeaderCell.Style = dgv1_columnHeaderStyle;
            // use the datasource column captions as the grid column names.
            dc = DgvSkeletonExpansion.Columns[0]; //"Skeleton"];
            dc.HeaderText = _skelExpansion.SkeletonColumn.Caption;
            dc.Width = 100;
            dc = DgvSkeletonExpansion.Columns[1]; //["Offset"];
            dc.HeaderText = _skelExpansion.OffsetColumn.Caption;
            dc.Width = 70;
            dc = DgvSkeletonExpansion.Columns[2]; //["EndOffset"];
            dc.HeaderText = _skelExpansion.EndOffsetColumn.Caption;
            dc.Width = 70;
            dc = DgvSkeletonExpansion.Columns[3]; //["LineNumber"];
            dc.HeaderText = _skelExpansion.LineNumberColumn.Caption;
            dc.Width = 70;
            dc = DgvSkeletonExpansion.Columns[4]; //["LineText"];
            dc.HeaderText = _skelExpansion.LineTextColumn.Caption;
            dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            _parent.btnExpandEnable();
            _parent.StatusUpdate("Expansion complete.");

        }
        private void QueryExpand2Common(out bool dispose, string skelName, SkeletonParserDS parserSDS, TransientDS parserTDS,
             ImageList iL, string displayConfigName, XMLParmsFileReader xmlParmsRdr, bool fullExpand = true)
        {
            _skelName = skelName;
            _SDS = parserSDS;
            _TDS = parserTDS;
            _IL = iL;
            _xmlParmsReader = xmlParmsRdr;
            _fullExpand = fullExpand;
            this.Text = displayConfigName + " - Skeleton Expansion";
            dispose = !UserInitializeComponent(skelName);
        }
        /// <summary>
        /// Initialize the data on form instantiation. Expand the named skeleton.
        /// </summary>
        /// <param name="skelName"></param>
        /// <returns></returns>
        private bool UserInitializeComponent(string skelName)
        {
            this.Text += " " + skelName;
            _nestingGridLabel = this.SkelNestingGridLabel.Text;
            // set the buttons with images in the ImageList passed by the Query form
            BtnExit.ImageList = _IL;
            BtnExit.ImageKey = "pgmReturn.ico";
            BtnExit.ImageAlign = ContentAlignment.TopCenter;
            BtnMap.ImageList = _IL;
            BtnMap.ImageKey = "RoadMap.ico";
            BtnMap.ImageAlign = ContentAlignment.TopCenter;

            _parent.StatusUpdate("Expanding " + skelName);


            if (!Expand(skelName, ref _expansionRoot))
                return false;
            return true;
        }

        /// <summary>
        /// Turn on alternating 3 green/3 white row coloring
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvSkeletonExpansion_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 6 < 3)
                DgvSkeletonExpansion.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            else
                DgvSkeletonExpansion.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(192,255,192);
        }

        /// <summary>
        /// turn on alternating green and white row coloring 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvNesting_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
                DgvNesting.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            else
                DgvNesting.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(192,255,192);

        }

        /// <summary>
        /// turn on alternating green and white row coloring
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvVariables_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
                DgvVariables.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            else
                DgvVariables.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.MintCream;

        }

        /// <summary>
        /// If the selection row changes, look up the command nesting and the variables on that
        /// line. Note: this event is triggered when the filter is turned on. The filter might
        /// return 0 rows (and thus there will be no selected row), so we have to check for that.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvSkeletonExpansion_SelectionChanged(object sender, EventArgs e)
        {
            if (DgvSkeletonExpansion.Rows.Count > 0)
                RefreshVariablesAndNestingGrids();
        }

        /// <summary>
        /// Look up the variables on the line and the command nesting. Populate the appropriate
        /// grids below the main grid.
        /// </summary>
        bool BypassExpansionRepositioning = false;

        /// <summary>
        /// Update the two grids in the lower part of the form.
        /// </summary>
        private void RefreshVariablesAndNestingGrids()
        {
            // don't let the Nesting grid do any automatic repositioning just because
            // this subroutine causes the RowEnter event to fire on the Nesting Grid.
            BypassExpansionRepositioning = true;

            int lineNo = Convert.ToInt32(DgvSkeletonExpansion.CurrentRow.Cells["LineNumber"].Value.ToString());
            int offset = Convert.ToInt32(DgvSkeletonExpansion.CurrentRow.Cells["Offset"].Value.ToString());
            string skelName = DgvSkeletonExpansion.CurrentRow.Cells["Skeleton"].Value.ToString();
            DataGridViewColumn dc;

             var dsLineVariables = (from o in _SDS.Variables.AsEnumerable()
                                   orderby o.Position
                                   where o.Skeleton == skelName
                                   && o.LineNumber == lineNo
                                   select new { Variable = o.Variable, type = o.Type }).ToArray();
            DgvVariables.DataSource = dsLineVariables;
            dc = DgvVariables.Columns[1]; //["Type"]
            dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            var dsCommandNesting = (
                                    from oE in _skelExpansion
                                    join oC in _cnt.AsEnumerable()
                                        on oE.Offset equals (oC.ExpansionCmdOffset == 0 ? oC.ExpansionLineOffset : oC.ExpansionCmdOffset)
                                    orderby oC.ExpansionCmdOffset
                                    where oC.ExpansionLineOffset == offset
                                    select new
                                    {
                                        Offset =
                                            (oC.ExpansionCmdOffset == 0 ? oC.ExpansionLineOffset : oC.ExpansionCmdOffset),
                                        EndOffset = oE.EndOffset,
                                        Skeleton = oC.Skeleton,
                                        Line = oC.LineText
                                    }).ToArray();
            DgvNesting.DataSource = dsCommandNesting;
            DataGridViewCellStyle
                dgvCNT_columnHeaderStyle1 = new DataGridViewCellStyle
                    {
                        Font = new Font("Lucida Console", 10.0F, FontStyle.Regular)
                    },
                dgvCNT_columnHeaderStyle2 = new DataGridViewCellStyle
                    {
                        Font = new Font("Lucida Console", 8.5F, FontStyle.Regular)
                    };

            dc = DgvNesting.Columns[0]; //["Offset"];
            dc.Width = 70;
            DgvNesting.Columns[0].DefaultCellStyle = dgvCNT_columnHeaderStyle1;
            DgvNesting.Columns[0].HeaderCell.Style = dgvCNT_columnHeaderStyle2;
            dc.HeaderText = _cnt.ExpansionLineOffsetColumn.Caption;


            dc = DgvNesting.Columns[1]; //["EndOffset"];
            dc.Width = 70;
            DgvNesting.Columns[1].DefaultCellStyle = dgvCNT_columnHeaderStyle1;
            DgvNesting.Columns[1].HeaderCell.Style = dgvCNT_columnHeaderStyle2;
            dc.HeaderText = _cnt.ExpansionEndLineOffsetColumn.Caption;

            dc = DgvNesting.Columns[2]; //["Skeleton"];
            dc.Width = 100;
            DgvNesting.Columns[2].DefaultCellStyle = dgvCNT_columnHeaderStyle1;
            DgvNesting.Columns[2].HeaderCell.Style = dgvCNT_columnHeaderStyle2;

            dc = DgvNesting.Columns[3]; //["Line"];
            DgvNesting.Columns[3].DefaultCellStyle = dgvCNT_columnHeaderStyle1;
            DgvNesting.Columns[3].HeaderCell.Style = dgvCNT_columnHeaderStyle1;
            dc.HeaderText = _cnt.LineTextColumn.Caption;
            dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //
            // Reenable the nesting grid'buffer RowEnter event'buffer ability to reposition lines in 
            // the expansion grid.
            BypassExpansionRepositioning = false;
        }

        /// <summary>
        /// When the user double-clicks in the Variables grid, open a new form showing 
        /// all of references to a specific variable in all skeletons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvVariables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (DgvVariables.Rows.Count == 0)
                return;
            QueryVariable qv = new QueryVariable(_SDS, _TDS, _IL, "",
                DgvVariables.CurrentRow.Cells["Variable"].Value.ToString(), _xmlParmsReader, _parent);
            qv.Show();

        }

        /// <summary>
        /// If the user enters the Offset or EndOffset cell of a row in the Nesting grid, position the
        /// expansion grid two lines above the line indicated in the cell that was entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvNesting_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (TsmiGotoLine.Enabled == false || BypassExpansionRepositioning)
                return;
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1) // Offset or EndOffset
            {

                int offset = -1;

                bool isNum = int.TryParse(
                    DgvNesting.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out offset);

                if (isNum)
                {
                    offset -= (offset > 2 ? 3 : offset);
                    DgvSkeletonExpansion.FirstDisplayedScrollingRowIndex = offset;
                }
            }

        }

        /// <summary>
        /// Expand the current skeleton using the pre-built SkeletonLines table in the passed
        /// Dataset. Note: this routine is called recursively when an )IM statement is
        /// encountered.
        /// </summary>
        /// <param name="filter">skeleton name that is a conditional query value 
        /// for the SkeletonLines table loaded by the parsing process on program start-up
        /// or configuration switch.
        /// </param>
        /// <returns></returns>
        private bool Expand(string skelName, ref TreeNode parentNode)
        {
            TransientDS.SkeletonExpansionRow
                expansionData,
                expansionData2;
            TreeNode tn;
            Match m, m2;                // working matches
            string s0, s1, s2, s3;      // working strings
            string cmd = "";
            Nesting n0;                 // working command nests
            bool
                stackError = false;
            tn = new TreeNode(skelName);
            if (parentNode == null)
                parentNode = tn;
            else
                parentNode.Nodes.Add(tn);

            TransientDS.CommandNestingRow _cntr = null;

            // get all the lines in the skeleton
            var results = (from o in _TDS.SkeletonLines.AsEnumerable()
                           orderby o.LineNumber
                           where o.Skeleton == skelName
                           orderby o.LineNumber
                           select new
                           {
                               Skeleton = o.Skeleton,
                               lineNumber = o.LineNumber,
                               lineText = o.LineText
                           }
               ).ToArray();

            // process each line
            foreach (var skelInfo in results)
            {
                int savOffset;
                // Create a main expansion transient datasource row for this skelInfo line.
                expansionData = (TransientDS.SkeletonExpansionRow)_skelExpansion.NewRow();
                expansionData.LineNumber = skelInfo.lineNumber;
                expansionData.LineText = skelInfo.lineText;
                expansionData.Skeleton = skelInfo.Skeleton;
                savOffset = expansionData.EndOffset = expansionData.Offset = ++_offset;

                // save the line text to allow processing of multiple )xxx cmds on a single line
                s0 = skelInfo.lineText.Trim();

                // if the line is blank, add it to the expansion and do nothing further
                if (s0.Length == 0)
                {
                    _skelExpansion.AddSkeletonExpansionRow(expansionData);
                    continue;
                }

                // s0 will either be the full line, or a substring of the line in the case of
                // commands on the )IF THEN )cmd and )ELSE )cmd constructs.
                while (s0.Length > 0)
                {
                    m = Regex.Match(s0, SkeletonParserRE.COMMAND_PARSE, RegexOptions.IgnorePatternWhitespace);

                    if (m.Success) // we have a )xxx skeleton command.
                    {
                        cmd = m.Groups["Command"].Value;
                        switch (cmd)
                        {
                            case "IM":
                                #region Process)IM
                                bool doExpand = true;

                                // add the nesting info to the stack. This will be popped off after
                                // the )IM'd skeleton is recursively expanded. This ensures that every line
                                // in the )IM chain that follows will show that this )IM statement was part of its
                                // nesting logic.
                                _NestingStack.Push(n0 = new Nesting(_offset, skelInfo.lineNumber, skelName,
                                    s0, cmd));

                                // Get the imbedded skeleton name and any )IM options
                                m2 = Regex.Match(skelInfo.lineText, SkeletonParserRE.IM_PARSE);

                                s1 = m2.Groups["ImbeddedSkelName"].Value;       //imbedded skel name
                                s2 = m2.Groups["IMModifiers"].Value.Trim();     //options

                                if (s1.Contains("&"))
                                {
                                    doExpand = false; // name of )IM'd skel only known at run time.
                                    m2 = Regex.Match(s2, SkeletonParserRE.IM_PARSE_OPT);
                                    if (!m2.Success)
                                    {
                                        // notify user by adding "missing" text to )IM in expansion
                                        expansionData.LineText = (expansionData.LineText.Trim() + " <-- Missing OPT").PadRight(72);
                                        doExpand = false;
                                    }
                                }
                                // otherwise, check whether this skeleton is already in the 
                                // chain of nested )IM statements (e.g. main: )IM X  X: )IM Y  Y: )IM X)
                                else if (_imbedChain.Contains(s1))
                                {
                                    if (_xmlParmsReader.ImbedRecurseFail) // config file indicates we want to fail on recursion
                                    {
                                        s3 = "";
                                        foreach (string name in _imbedChain)
                                            s3 += name + " ";
                                        s3.TrimEnd();
                                        MessageBox.Show(s1 + "already appears in the nesting:\n" + s3 + "\n");
                                        return false;
                                    }
                                    doExpand = false; // since we've seen this name already in nest chain, bypass expansion
                                }

                                if (doExpand & _fullExpand)
                                {
                                    if (!Expand(s1, ref tn))
                                    {
                                        _NestingStack.Pop(); // pop the )IM from the stack.
                                        _imbedChain.RemoveAt(_imbedChain.Count - 1);
                                        return false;
                                    }
                                }
                                expansionData.EndOffset = _offset;

                                if (n0 != _NestingStack.Peek()) // top of stack is not the )IM of this line
                                {
                                    MessageBox.Show(
                                        "A skeleton in the )IM'd chain had mismatching )DO/)ENDDO, )SEL/)ENDSEL or )DOT/)ENDDOT\n" +
                                        skelInfo.lineText + "(@" + skelInfo.lineNumber.ToString() + ")\n");
                                    return false;
                                }

                                if (_imbedChain.Count > 0)
                                    _imbedChain.RemoveAt(_imbedChain.Count - 1); // last name added.
                                _NestingStack.Pop(); // pop the )IM from the stack.
                                break;
                                #endregion
                            case "SEL":
                                break;
                            case "DO":
                                break;
                            case "DOT":
                                break;
                            case "IF":
                                #region Process )IF
                                m = Regex.Match(s0, SkeletonParserRE.COND_PARSE);
                                if (m.Success)
                                {
                                    s1 = m.Groups["cmd"].Value;
                                    if (s1 != "") // dependent to )IF is on this statement
                                    {
                                        s0 = s1.TrimStart();
                                        // bypass push/pop logic. No subsequent line is
                                        // considered subordinate to this )IF.
                                        continue;
                                    }
                                }
                                break;
                                #endregion
                            case "ELSE":
                                #region Process )ELSE
                                m = Regex.Match(s0, SkeletonParserRE.ELSE_PARSE);
                                if (m.Success)
                                {
                                    s1 = m.Groups["ElseCmd"].Value;
                                    if (s1 != "") // dependent to )ELSE is on this statement
                                    {
                                        s0 = s1.TrimStart();
                                        // bypass push/pop logic. No subsequent line is
                                        // considered subordinate to this )ELSE.
                                        continue;
                                    }
                                }
                                break;
                                #endregion
                            case "ENDSEL":
                            case "ENDDO":
                            case "ENDDOT":
                            case "CM":
                                break;
                            default:
                                break;
                        }

                    }
                    else
                        cmd = "";
                    // add *this* line to the nesting stack so it displays on top
                    _cntr = (TransientDS.CommandNestingRow)_cnt.NewRow();
                    _cntr.Skeleton = skelName;
                    _cntr.SkelLineOffset = skelInfo.lineNumber;
                    _cntr.ExpansionCmdOffset = 0; // <- this will force this line to sort to top of Nesting grid
                    _cntr.ExpansionLineOffset = (cmd == "IM" ? savOffset : _offset);
                    _cntr.Command = "";
                    _cntr.LineText = skelInfo.lineText; // use full line, s0 may contain partial data
                    _cntr.ExpansionEndLineOffset = 0;
                    _cntr.CommandCode = 0;
                    _cnt.AddCommandNestingRow(_cntr);
                    // process the list of )xxx command lines leading to *this* line.
                    foreach (Nesting n2 in _NestingStack.ToArray())
                    {
                        _cntr = (TransientDS.CommandNestingRow)_cnt.NewRow();
                        _cntr.Skeleton = n2.SkelName;
                        _cntr.SkelLineOffset = n2.LineNumber;
                        _cntr.ExpansionCmdOffset = n2.Offset;
                        _cntr.ExpansionLineOffset = (cmd == "IM" ? savOffset : _offset);
                        _cntr.Command = n2.Command;
                        _cntr.LineText = n2.LineText;
                        _cntr.ExpansionEndLineOffset = 0;
                        _cntr.CommandCode = SkeletonParserRE.cKeywords[n2.Command];
                        _cnt.AddCommandNestingRow(_cntr);
                    }

                    // if command is a skeleton comment or formatting command, don't add it to the nesting stack
                    if (cmd == "CM"
                        || cmd == "BLANK"
                        || cmd == "TBA" 
                        || cmd == "TB" 
                        || cmd == "DEFAULT"
                        )
                    {
                        _skelExpansion.AddSkeletonExpansionRow(expansionData);
                        break; // while (s0.length > 0)
                    }

                    // if the prior command was an )IF or )ELSE, clear it from the stack.
                    // )IF and )ELSE are only valid for the following line.
                    if (_NestingStack.Count > 0)
                    {
                        n0 = _NestingStack.Peek();
                        if (n0.Command == "IF" || n0.Command == "ELSE")
                            _NestingStack.Pop();
                    }
                    // if *this* was a )DO, )DOT, )IF, )ELSE or )SEL statment, add it to the nested
                    // command stack.
                    stackError = false;
                    n0 = null;
                    switch (cmd)
                    {
                        case "SEL":
                        case "DO":
                        case "DOT":
                            switch (cmd)
                            {
                                case "SEL":
                                    _selNestingStack.Push(expansionData);
                                    break;
                                case "DO":
                                    _doNestingStack.Push(expansionData);
                                    break;
                                case "DOT":
                                    _dotNestingStack.Push(expansionData);
                                    break;
                                default:
                                    break;
                            }
                            _NestingStack.Push(new Nesting(_offset, skelInfo.lineNumber, skelName, skelInfo.lineText, cmd));
                            break;

                        case "IF":
                        case "ELSE":
                            _NestingStack.Push(new Nesting(_offset, skelInfo.lineNumber, skelName, skelInfo.lineText, cmd));
                            _skelExpansion.AddSkeletonExpansionRow(expansionData);
                            break;
                        case "ENDSEL":
                        case "ENDDOT":
                        case "ENDDO":
                            _skelExpansion.AddSkeletonExpansionRow(expansionData);
                            expansionData2 = null;
                            switch (cmd)
                            {
                                case "ENDSEL":
                                    if (_selNestingStack.Count > 0)
                                    {
                                        expansionData2 = _selNestingStack.Peek();
                                        // if the )SEL on the stack is from a different skeleton, this
                                        // is an error
                                        if (expansionData2.Skeleton != expansionData.Skeleton)
                                            stackError = true;
                                    }
                                    else
                                        stackError = true;
                                    break;
                                case "ENDDO":
                                    if (_doNestingStack.Count > 0)
                                    {
                                        expansionData2 = _doNestingStack.Peek();
                                        // if the )DO on the stack is from a different skeleton, this
                                        // is an error
                                        if (expansionData2.Skeleton != expansionData.Skeleton)
                                            stackError = true;
                                    }
                                    else
                                        stackError = true;
                                    break;
                                case "ENDDOT":
                                    if (_dotNestingStack.Count > 0)
                                    {
                                        expansionData2 = _dotNestingStack.Peek();
                                        // if the )DOT on the stack is from a different skeleton, this
                                        // is an error
                                        if (expansionData2.Skeleton != expansionData.Skeleton)
                                            stackError = true;
                                    }
                                    else
                                        stackError = true;
                                    break;
                                default:
                                    break;
                            }
                            s1 = cmd.Substring(3); // extract DO, SEL, DOT from ENDDO, ENDSEL, ENDDOT
                            if (_NestingStack.Count > 0)
                            {
                                n0 = _NestingStack.Peek();
                                if (n0.Command == s1)
                                    _NestingStack.Pop();
                                else
                                    stackError = true;
                            }
                            else
                                stackError = true;

                            if (stackError)
                            {
                                MessageBox.Show(")" + cmd + " with no matching / Out-of-sequence )" + s1 + "\n" +
                                    skelInfo.lineText + "(@" + skelInfo.lineNumber.ToString() + ")\n" +
                                    (n0 != null ? n0.LineText + "(@" + n0.LineNumber.ToString() + ")\n" : ""));
                                DumpStack(_NestingStack);
                                return false;
                            }
                            else
                            {
                                expansionData2 = 
                                    cmd == "ENDSEL" ? _selNestingStack.Pop() :
                                    cmd == "ENDDO" ? _doNestingStack.Pop() : _dotNestingStack.Pop();
                                expansionData2.EndOffset = expansionData.Offset;
                                _skelExpansion.AddSkeletonExpansionRow(expansionData2);
                            }
                            break;
                        default:
                            _skelExpansion.AddSkeletonExpansionRow(expansionData);
                            break;
                    }
                    s0 = ""; // force while (s0 != "") termination
                }
            }

            return true; // this skeleton expanded with no errors.
        }

        [Conditional("DEBUG")]
        private void DumpStack(Stack<Nesting> ns)
        {
            foreach (Nesting nsI in ns)
                Console.WriteLine(nsI.Offset.ToString("00000 ") + nsI.LineText);
            return;
        }

        /// <summary>
        /// Close this form and exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// common routine, invoked as event processor delegate for grid column added, to turn
        /// off sorting for a grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_ColumnAdded_DisableSort(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        /// <summary>
        /// From the context menu of the Expansion grid, prompt the user for a line
        /// number to "goto" and then position on that line. note: this does *not*
        /// change the CurrentRow index.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiGotoLine_Click(object sender, EventArgs e)
        {
            // if the current expansion grid is empty, there'skelInfo nowhere to go
            if (DgvSkeletonExpansion.Rows.Count == 0)
                return;

            int lineNum = -1;
            bool isNum;

            // initialize the return value of the input box
            string strLineNum = DgvSkeletonExpansion.CurrentRow.Index >= 0 ?
                (DgvSkeletonExpansion.CurrentRow.Index + 1).ToString() : "1";

            if (InputBox.Show("Line number", "Type the line number to go to:",
                ref strLineNum) == DialogResult.Cancel)
                return;

            if ((strLineNum = strLineNum.Trim()) == "")
            {
                // blank return value, treat it as "goto current selected row"
                DgvSkeletonExpansion.FirstDisplayedScrollingRowIndex =
                    DgvSkeletonExpansion.CurrentRow.Index;
            }
            else
            {
                isNum = int.TryParse(strLineNum, out lineNum);
                if (isNum && lineNum > 0)
                    if (lineNum <= DgvSkeletonExpansion.Rows.Count)
                        // postion on user entered line.
                        DgvSkeletonExpansion.FirstDisplayedScrollingRowIndex = lineNum - 1;
                    else
                        // position on last line
                        DgvSkeletonExpansion.FirstDisplayedScrollingRowIndex =
                            DgvSkeletonExpansion.Rows.Count - 1;
                else if (isNum && lineNum <= 0)
                    // for negative line numbers or line number 0, position on first line.
                    DgvSkeletonExpansion.FirstDisplayedScrollingRowIndex = 0;
                else
                    // non-numeric user input
                    MessageBox.Show(strLineNum + " must be an integer between 1 and " +
                        DgvSkeletonExpansion.Rows.Count);
            }
        }
        public void ExternalGotLineRequest(int lineNum)
        {
            DgvSkeletonExpansion.FirstDisplayedScrollingRowIndex = lineNum - 1;
        }
        /// <summary>
        /// Prompt the user for a search string and then show only those lines that
        /// contain that string. Uses the .filter method of the binding source.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiApplyTextFilter_Click(object sender, EventArgs e)
        {
            // Prompt for a filter value. Exit if the user presses the Cancel button.
            if (InputBox.Show("Filter",
                    "Type a string. Only rows containing this string will be shown.",
                    ref _findString) == DialogResult.Cancel)
                return;

            // An empty filter will show all lines, thus it will work like no filter at
            // all.
            if (_findString == "")
            {
                this.TsmiRemoveTextFilter_Click(sender, e);
                return;
            }

            // once filtering is turned on, GoTo Line... is to be disabled. The Remove
            // tool strip menu group must be activated and the Text Filter remove entries 
            // thereon made visible.
            GotoAllowed(false);
            TsmiRemoveFilterAll.Visible = true;
            TsmiRemoveTextFilter.Visible = true;

            // Apply the filter.
            _expansionBindingSource.Filter = BuildFilter();
        }

        /// <summary>
        /// Disable the current active binding source text filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiRemoveTextFilter_Click(object sender, EventArgs e)
        {
            // Clear the filter string. 
            _findString = "";

            // De-activate the remove text filter menu option by making the entry invisible.
            TsmiRemoveTextFilter.Visible = false;

            // Check if there are any skeleton hiding filters active. Disable the Remove All filters
            // entry.
            if (_skelHide.Count == 0) // if no skel hiding filters
            {
                TsmiRemoveFilterAll.Visible = false;
                // there are no filters, disable the Remove Filters... menu item, enable Go to line...
                GotoAllowed(true);
            }

            // Apply the filter.
            _expansionBindingSource.Filter = BuildFilter();
        }

        private void TsmiApplySkelFilter_Click(object sender, EventArgs e)
        {
            string filter = "";
            string[] arrFilter;
            char[] delims = { ';', ' ', ',', ':' };

            // Prompt the user for one or more skeleton names to be hidden.
            if (InputBox.Show("Skeleton name(buffer) to hide...",
                "Type one or more skeleton names. All rows from those skeletons will be hidden.",
                ref filter) == DialogResult.Cancel)
                return;

            // If user'buffer selection was empty, leave.
            if (filter.Trim() == "")
                return;

            // allow for any of the following delimiters: space/,/;/:
            filter = filter.Trim().Replace(',', ' ').Replace(';', ' ').Replace(':', ' ');

            // convert the list to an array of skeleton names.
            arrFilter = filter.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            // add the names to the _skelHide ArrayList. 
            foreach (string s in arrFilter)
            {
                // Validate that the named skeleton exists in the expansion
                var exists = (from o in _skelExpansion
                              where o.Skeleton == s
                              select o.Skeleton).Count();
                if (exists > 0) // the user selected skeleton exists in the expansion
                    if (!_skelHide.Contains(s))
                        _skelHide.Add(s);
            }
            // if the ArrayList is not empty:
            if (_skelHide.Count > 0)
            {
                // disable Go to line..., activate the Remove group item
                GotoAllowed(false);

                // make the Skel filter menu items active by making them visible.
                TsmiRemoveSkelFilter.Visible = true;
                TsmiRemoveSkelFilterAll.Visible = true;
                TsmiRemoveFilterAll.Visible = true;
            }

            _expansionBindingSource.Filter = BuildFilter();
        }

        /// <summary>
        /// Prompt the user for a list of the currently skeletons that are to be redisplayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiRemoveSkelFilter_Click(object sender, EventArgs e)
        {
            string filter = "";

            if (_skelHide.Count > 0)
            {
                // add each of the existing filter objects to the default filter passed to the
                // InputBox.Show method.
                foreach (string s in _skelHide)
                    filter += s + " ";
                filter = filter.TrimEnd();
                InputBox.Show(
                    "Remove skeleton hiding filters",
                    "Enter one or more skeleton names to redisplay...",
                    ref filter);
                filter = filter.Replace(',', ' ').Replace(';', ' ').Replace(':', ' ');

                // convert the filter to an array of strings, then to an ArrayList of strings.
                _skelHide = new ArrayList(
                    filter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }
            TsmiRemoveSkelFilter2();
        }

        /// <summary>
        /// Common routine to remove skeleton name from the list of hidden skeletons. Before this
        /// routine is called, the _skelHide will have been updated either by the
        /// tsmiRemoveSkelFilter_Click method, the TsmiRemoveSkelFilterAll_Click method or 
        /// by the StatusHiddenSkeletons_DropDownItemClicked method.
        /// </summary>
        private void TsmiRemoveSkelFilter2()
        {
            if (_skelHide.Count == 0)
            {
                if (_findString == "")
                {
                    TsmiRemoveFilterAll.Visible = false;
                    // there are no filters, disable the Remove Filters... menu item, enable Go to line...
                    GotoAllowed(true);
                }

                TsmiRemoveSkelFilter.Visible = false;
                TsmiRemoveSkelFilterAll.Visible = false;
            }
            _expansionBindingSource.Filter = BuildFilter();
        }

        /// <summary>
        /// Remove all existing skeleton filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiRemoveSkelFilterAll_Click(object sender, EventArgs e)
        {
            _skelHide.Clear(); // no more filtering on skeleton names
            TsmiRemoveSkelFilter2();
        }

        /// <summary>
        /// Remove all filters: skeleton and text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiRemoveFilterAll_Click(object sender, EventArgs e)
        {
            TsmiRemoveTextFilter_Click(sender, e);
            TsmiRemoveSkelFilterAll_Click(sender, e);
        }

        /// <summary>
        /// The goToLine disable must be done before the binding source filter is
        /// applied. When the filter is applied, the RowEnter event is triggered
        /// and the event handler needs to know if filtering is in effect. The
        /// gotoLineStripMenuItem.Enabled is tested to see if filtering is turned on.
        /// </summary>
        /// <param name="toggleSwitch"></param>
        private void GotoAllowed(bool toggleSwitch)
        {
            // enable/disable Go to line... menu item
            TsmiGotoLine.Enabled = toggleSwitch;
            // enable/disable the filter removal group menu item.
            TsmgRemoveFilters.Enabled = !toggleSwitch;

            if (toggleSwitch)
            {
                TsmiGotoLine.Text = "Go to line...";
                SkelNestingGridLabel.Text = _nestingGridLabel;
            }
            else
            {
                TsmiGotoLine.Text = "Go to line... (disabled while filtering)";
                SkelNestingGridLabel.Text = _nestingGridLabel + " (repositioning disabled while filtering)";
            }
        }

        /// <summary>
        /// Check the global _skelHide and _findString filters, build the filter
        /// statement and return it as a string.
        /// </summary>
        /// <returns></returns>
        private string BuildFilter()
        {
            string filter = "";

            _dropDown.Items.Clear();

            if (_skelHide.Count > 0)
            {
                foreach (string s in _skelHide)
                {
                    filter += "'" + s.ToUpper().Trim() + "' ";
                    _dropDown.Items.Add(s);
                }
                StatusHiddenLabel.Text = "Hidden skeletons (Drop down to list)";
                filter = "(Skeleton NOT IN (" + filter.TrimEnd().Replace(' ', ',') + "))";
                TsmgRemoveFilters.Enabled = true;
            }
            else
                StatusHiddenLabel.Text = "";

            StatusHiddenSkeletons.DropDown = _dropDown;

            if (_filterString != "")
            {
                StatusFilter.Text = "(filter=" + _filterString + ")";
                if (filter != "")
                    filter += " AND ";
                filter += "LineText LIKE '%" + _filterString.Replace("%", "[%]").Replace("*", "[*]") + "%'";
                TsmgRemoveFilters.Enabled = true;
            }
            else
                StatusFilter.Text = "";

            if (_filterString != "" && _skelHide.Count > 0)
                TsmiRemoveSkelSep.Visible = true;
            else
                TsmiRemoveSkelSep.Visible = false;

            return filter;
        }

        /// <summary>
        /// When the user drops down the list of hidden skeletons, selecting any skeleton in the list
        /// will cause that skeleton to be redisplayed wherever it occurs in the expansion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusHiddenSkeletons_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (_skelHide.Contains(e.ClickedItem.Text))
                _skelHide.Remove(e.ClickedItem.Text);

            TsmiRemoveSkelFilter2();
        }

        /// <summary>
        /// Remove this form from the list of open subforms in the parent form(Query).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryExpand2_FormClosing(object sender, FormClosingEventArgs e)
        {
           _parent.RemoveForm(this,true);
            for (int i = 0; i < graphList.Count; ++i)
                graphList[i].Close();
        }

        private List<QueryExpand1> graphList = new List<QueryExpand1>(100);
        public void RemoveForm(QueryExpand1 f)
        {
            graphList.Remove(f);
            BtnMap.Visible = true;
        }
        public void AddForm(QueryExpand1 f)
        {
            graphList.Add(f);
        }
        private void BtnMap_Click(object sender, EventArgs e)
        {
            BtnMap.Visible = false;
            QueryExpand1 qe1 = new QueryExpand1(_skelName, _expansionRoot, _IL, _parent, _SDS,_TDS,_xmlParmsReader);
            qe1.Show();
            BtnMap.Visible = true;
        }


        bool _disposed = false;
        public new void Dispose()
        {
            Dispose(true);
        }

        private void TsmiFindString_Click(object sender, EventArgs e)
        {
            string findStr = "";
            bool matchCase = false;
            bool matchWord = false;
            ExpansionFindParameters efp = new ExpansionFindParameters(_parent);
            DialogResult dr;
            if ((dr = efp.ShowDialog(ref findStr, ref matchCase, ref matchWord)) == DialogResult.OK
                && findStr != "")
            {
                _findString = findStr;
                _matchCase = matchCase;
                _matchWord = matchWord;
            }
            else
                return; // user pressed cancel, or entered a null string
            
            // If user'buffer selection was empty, leave.
            if (findStr.Trim() == "")
                return;
            if ((_findOffset = Find(_findString,_matchCase,_matchWord)) == -1)
            {
                MessageBox.Show(_findString + " was not found");
                return;
            }

            DgvSkeletonExpansion.FirstDisplayedScrollingRowIndex = _findResults[_findOffset] - 1;
        }

        private void QueryExpand2_Load(object sender, EventArgs e)
        {

        }

        private void TsmiFindNext_Click(object sender, EventArgs e)
        {
            if ((_findOffset = FindNext()) == -1)
            {
                MessageBox.Show("No more occurences of " + _findString);
                return;
            }
            DgvSkeletonExpansion.FirstDisplayedScrollingRowIndex = _findResults[_findOffset] - 1;
        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

    }
    class Nesting
    {
        private string _SkelName;
        public string SkelName
        {
            get { return _SkelName; }
            set { _SkelName = value; }
        }

        private string _cmd;
        public string Command
        {
            get { return _cmd; }
            set { _cmd = value; }
        }

        private int _lineNumber;
        public int LineNumber
        {
            get { return _lineNumber; }
            set { _lineNumber = value; }
        }

        private string _lineText;
        public string LineText
        {
            get { return _lineText; }
            set { _lineText = value; }
        }

        private int _offset;
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public Nesting(int offset, int lineNumber, string skelname, string lineText, string command)
        {
            _SkelName = skelname;
            _lineNumber = lineNumber;
            _offset = offset;
            _lineText = lineText;
            _cmd = command;
        }
    }

}