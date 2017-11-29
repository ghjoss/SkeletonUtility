using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SkeletonParserDSDef;
//using SkeletonParserRE;

namespace SkeletonParserQuery
{       
 
    public partial class QueryExpand : Form
    {

        private TransientDS _TDS = new TransientDS();
        private SkeletonParserDS _SDS = null;
        private ImageList _IL = null;
#if DEBUG
        private Logger _logger = new Logger();
#endif
        Query _parentForm;
        private TransientDS.CommandNestingDataTable _cnt = null;

        public QueryExpand(string skelName, SkeletonParserDS parserSDS, TransientDS parserTDS, 
            ImageList iL, string displayConfigName, Query parentForm)
        {
            _SDS = parserSDS;
            _TDS = parserTDS;
            _TDS.SkeletonExpansion.Clear();
            _parentForm = parentForm;
            _IL = iL;
            InitializeComponent();
            this.Text = displayConfigName + " - Skeleton Expansion";
            UserInitializeComponent(skelName);
        }

        Stack<Nesting> NestingStack = null;

        private void UserInitializeComponent(string skelName)
        {
            NestingStack = new Stack<Nesting>(100); // 32 SEL + 16 IM + 3 DOT + padding
            DataGridViewColumn dc;
            // set the buttons with images in the ImageList passed by the Query form
            btnExit.ImageList = _IL;
            btnExit.ImageKey = "pgmReturn.ico";
            btnExit.ImageAlign = ContentAlignment.TopCenter;
            _parentForm.StatusUpdate("Expanding " + skelName);

            _cnt = new TransientDS.CommandNestingDataTable();

            Expand(skelName);
#if DEBUG
            _logger.Close();
#endif
            //dataGridView1.Visible = false;
            dataGridView1.DataSource = _TDS.SkeletonExpansion;
            dataGridView1.Refresh();

            DataGridViewCellStyle dgv1_columnHeaderStyle = new DataGridViewCellStyle();

            //dvg1_columnHeaderStyle.BackColor = Color.Beige;
            dgv1_columnHeaderStyle.Font = new Font("Lucida Console", 10, FontStyle.Regular);
            dataGridView1.Columns[3].DefaultCellStyle = dgv1_columnHeaderStyle;
            dataGridView1.Columns[3].HeaderCell.Style = dgv1_columnHeaderStyle;
            //dataGridView1.ColumnHeadersDefaultCellStyle = dgv1_columnHeaderStyle;


            //dataGridView1.Visible = true;
            _parentForm.btnExpandEnable();
            _parentForm.StatusUpdate("Expansion complete.");
            dc = dataGridView1.Columns[0]; //"Skeleton"];
            dc.HeaderText = _TDS.SkeletonExpansion.SkeletonColumn.Caption;
            dc.Width = 100;
            dc = dataGridView1.Columns[1]; //["Offset"];
            dc.HeaderText = _TDS.SkeletonExpansion.OffsetColumn.Caption;
            dc.Width = 50;
            dc = dataGridView1.Columns[2]; //["LineNumber"];
            dc.HeaderText = _TDS.SkeletonExpansion.LineNumberColumn.Caption;
            dc.Width = 50;
            dc = dataGridView1.Columns[3]; //["LineText"];
            dc.HeaderText = _TDS.SkeletonExpansion.LineTextColumn.Caption;
            dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            else
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            _parentForm.StatusUpdate("");
            this.Close();
        }

        private int _Offset = 0;

        /// <summary>
        /// Process the lines of a _skeleton. Whenever an )IM statement is found, recursively call
        /// this routine to read that _skeleton.
        /// </summary>
        /// <param name="SkelName">The name of the _skeleton to read, used as the selection qualification
        /// of the SkeletonLines table.</param>
        private void Expand(string SkelName)
        {
#if DEBUG
            Logger loggerExpand = new Logger(SkelName + ".txt");
#endif

            var results = (from o in _TDS.SkeletonLines.AsEnumerable()
                           orderby o.LineNumber
                           where o.Skeleton == SkelName
                           orderby o.Skeleton, o.LineNumber
                           select new
                           {
                               Skeleton = o.Skeleton,
                               lineNumber = o.LineNumber,
                               lineText = o.LineText
                           }
                           ).Distinct().ToArray();

            foreach (var s in results)
            {
#if DEBUG
                loggerExpand.WriteLine(s.ToString());
#endif
                TransientDS.SkeletonExpansionRow ser =
                      (TransientDS.SkeletonExpansionRow) _TDS.SkeletonExpansion.NewRow();
                ser.LineNumber = s.lineNumber;
                ser.LineText = s.lineText;
                ser.Skeleton = s.Skeleton;
                ser.Offset = ++_Offset;
                _TDS.SkeletonExpansion.AddSkeletonExpansionRow(ser);



              var qry = (from t in _TDS.CommandNesting.AsEnumerable()
                            where t.Skeleton == s.Skeleton
                            orderby t.SkelLineOffset.ToString("0000")
                           select new
                           {
                               Skeleton = t.Skeleton,
                               LineNumber = t.SkelLineOffset.ToString("0000"),
                               EndingLineNumber = t.EndLineNumber,
                               command = t.Command,
                               CommandCode = t.CommandCode,
                               position = t.Position
                           }); 

                foreach (var q in qry)
                {
#if DEBUG
                    _logger.WriteLine(q.ToString());
#endif
                }
                // process an )IM statement (recurse). Note that an )IM could also be part of an
                // )IF or )ELSE statement [e.g. )IF &A = 4 THEN )IM SKELNAME or )ELSE )IM SKELNAME]
                if (s.lineText.StartsWith(")")
                    & !s.lineText.StartsWith(")CM")
                    & s.lineText.Contains(")IM "))
                {
                    string[] sArr = s.lineText.Split(" ".ToCharArray());
                    string str = null,
                        str2 = null;
                    for (int i = 0; i < sArr.Length; ++i)
                    {
                        str2 = sArr[i];
                        if (str2 == ")IM" & i < (sArr.Length - 1))
                        {
                            str = sArr[i + 1];
                            SkeletonParserQuery.Nesting n = 
                                new SkeletonParserQuery.Nesting(_Offset,s.lineNumber,s.Skeleton);
                            n.LineText = s.lineText;
                            NestingStack.Push(n);
                            Expand(str.Trim());
                            n = NestingStack.Pop();
                        }
                    }
                }
                if (s.lineText.StartsWith(")")
                 & !s.lineText.StartsWith(")CM")
                 & (s.lineText.Contains(")SEL ") | s.lineText.Contains(")DO ") |
                     s.lineText.Contains(")DOT "))
                   )
                {
                    Nesting n = new Nesting(_Offset, s.lineNumber, s.Skeleton);
                    n.LineText = s.lineText;
                    NestingStack.Push(n);
                }
                else if (s.lineText.StartsWith(")")
                 & !s.lineText.StartsWith(")CM")
                 & (s.lineText.Contains(")ENDSEL ") | s.lineText.Contains(")ENDDO ") |
                     s.lineText.Contains(")ENDDOT "))
                   )
                {
                    Nesting n = NestingStack.Pop();
                }
            }
            //dataGridView2.DataSource = cnt;
            //dataGridView2.Refresh();
#if DEBUG
            loggerExpand.Close();
#endif
        }
        //private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    SetVarGrid();
        //}

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            SetVarGrid();
        }

        private void SetVarGrid()
        {
            int lineNo = Convert.ToInt32(dataGridView1.CurrentRow.Cells["LineNumber"].Value.ToString());
            string skelName = dataGridView1.CurrentRow.Cells["Skeleton"].Value.ToString();

            var results = (from o in _SDS.Variables.AsEnumerable()
                           orderby o.Position
                           where o.Skeleton == skelName 
                           &&    o.LineNumber == lineNo
                           select new { Variable = o.Variable, type = o.Type }).ToArray();
            dataGridView3.DataSource = results;
        }

        private void dataGridView3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            QueryVariable qv = new QueryVariable(_SDS, _IL, "", 
                dataGridView3.CurrentRow.Cells["Variable"].Value.ToString());
            qv.Show();

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

        private int _lineNumber;

        public int LineNumber
        {
            get { return _lineNumber; }
            set { _lineNumber = value; }
        }
        private int _offset;

        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        private string _lineText;

        public string LineText
        {
            get { return _lineText; }
            set { _lineText = value; }
        }


        public Nesting(int offset, int lineNumber, string skelname)
        {
            _SkelName = skelname;
            _lineNumber = lineNumber;
            _offset = offset;
        }
    }
#if DEBUG
    class Logger
    {
        private string _path;
        private StreamWriter _stream;
        public Logger()
        {
            Init("Log2.txt");
        }
        public Logger(string logName)
        {
            Init(logName);
        }
        public void Init(string logName)
        {
            _path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), logName);
            _stream = new StreamWriter(_path);
        }

        public void WriteLine(string msg)
        {
            _stream.WriteLine(msg);
            _stream.Flush();
        }
        public void Close()
        {
            _stream.Close();
        }
    }
#endif
}