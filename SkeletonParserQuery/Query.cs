using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ISPFSkeletonParser;
using SkeletonParserDSDef;

namespace SkeletonParserQuery
{
    public partial class Query : Form
    {
        public const string NULL_SKEL = " -NULL- ";
        LibraryConcatenation _lc = null;
        SkeletonParser _sp = null;
        public SkeletonParserDS _SDS = null;
        public TransientDS _TDS = new TransientDS();
        private string _label;
        private bool _firstActivation = true;
        private XMLParmsFileReader _xmlRdr;
        private List<Form> _openForms = null;
        public Query()
        {
            InitializeComponent();
            //ISPFSkeletonParser.PanelParser pp = new ISPFSkeletonParser.PanelParser();

        }

        /// <summary>
        /// Get the XML file that is to be used to define the configuration. Verify that it is
        /// an XML file (done in OpenXML()). Disable the buttons, then invoke the parser as a
        /// foreground task (with no window).
        /// </summary>
        /// <returns></returns>
        public bool UserInit()
        {
            if (_openForms != null)
                for (int i = 0; i < _openForms.Count; ++i)
                    _openForms[i].Close();
            _openForms = new List<Form>(100);
            _SDS = new SkeletonParserDS(); // = new SkeletonParserDS();
            string[] results, results2, results3;
            //====================================================


            //====================================================
            btnConfig.Enabled = false;
            btnExit.Enabled = false;
            btnVariables.Enabled = false;
            btnSkeleton.Enabled = false;
            btnDOT.Enabled = false;
            btnPrograms.Enabled = false;
            btnExpand.Enabled = false;
            cboSkeletons.Enabled = false;
            cboSkeletons2.Enabled = false;
            cboSkeletons3.Enabled = false;

            if ((_xmlRdr = OpenXML()) != null)
            {
                string[] EmptyKey = new string[] { NULL_SKEL };
                _label = _xmlRdr.QueryDisplayName;
                StatusUpdate("");
                btnVariables.Enabled = true;
                btnSkeleton.Enabled = true;
                btnDOT.Enabled = true;
                btnPrograms.Enabled = true;
                btnExpand.Enabled = true;
                cboSkeletons.Enabled = true;
                cboSkeletons2.Enabled = true;
                cboSkeletons3.Enabled = true;

                // Set the drop down combo box contents. 
                // Note: we dodge a bullet here since a ChildSkeleton (from second select) that is also 
                // a Skeleton (from first select) does not appear multiple times.
                //results = (from o in _SDS.Skeletons.AsEnumerable()
                //           where !o.Skeleton.Contains("&")              // bypass )IM &XXX types
                //           select o.Skeleton).Distinct().ToArray()
                //           .Union(from o2 in _SDS.Skeletons.AsEnumerable()
                //                  where !o2.ChildSkeleton.Contains("&") // bypass )IM &XXX types
                //                  select o2.ChildSkeleton).Distinct().ToArray();
                //ArrayList resultsS = new ArrayList(results);
                //if (resultsS.Count > 0)
                //{
                //    resultsS.Sort();
                //}
                //else
                //{
                //    btnExpand.Enabled = false;
                //}
                //cboSkeletons.DataSource = resultsS;

                results = (from o in _TDS.SkeletonLines
                           orderby o.Skeleton
                           select o.Skeleton).Distinct().ToArray();
                cboSkeletons.DataSource = results;

                results2 = (from o in _TDS.SkeletonLines
                                select o.Skeleton).Distinct().ToArray()
                                .Union(EmptyKey).ToArray();

                ArrayList results2A = new ArrayList(results2);
                results2A.Sort();
                cboSkeletons2.DataSource = results2A;

                results3 = (from o in _TDS.SkeletonLines
                            select o.Skeleton).Distinct().ToArray()
                                .Union(EmptyKey).ToArray();

                ArrayList results3A = new ArrayList(results3);
                results3A.Sort();
                cboSkeletons3.DataSource = results3A;

                if (results.Length == 0)
                    btnExpand.Enabled = false;
            }

            btnConfig.Enabled = true;
            btnExit.Enabled = true;


            if (_xmlRdr == null)
                return false;
            else
                return true;

        }
        /// <summary>
        /// Terminate the program on the click of the Exit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Invoke the Skeleton-Variables cross reference dialog when the Variables button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVariables_Click(object sender, EventArgs e)
        {
            var qv = new QueryVariable(_SDS, _TDS, QueryImageList, _label, _xmlRdr, this);
            qv.Show();
        }
        /// <summary>
        /// Invoke the Skeleton-)DOT cross reference dialog when the )DOT button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDOT_Click(object sender, EventArgs e)
        {
            var qd = new QueryDOT(_SDS, _TDS, QueryImageList, _label, _xmlRdr, this);
            qd.Show();
        }
        /// <summary>
        /// Invoke the Skeleton-Skeleton cross reference dialog when the Skeleton button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSkeleton_Click(object sender, EventArgs e)
        {
            var qs = new QuerySkeleton(_SDS, _TDS, _lc, QueryImageList, _label, _xmlRdr, this);
            qs.Show();
        }

        /// <summary>
        /// Display an open file dialog to the user, with the execution directory as a default. Let the 
        /// user specify an XML Parms file as input. Verify that the file is a correct file by invoking
        /// the XMLParmsReader class of the parser. Return the XMLParmsReader object if successful, a null
        /// if not.
        /// </summary>
        /// <returns></returns>
        private XMLParmsFileReader OpenXML()
        {
            DialogResult dr;
            bool validParmsFile = true;
            XMLParmsFileReader xmlr = null;
            string tagErrMsg = "";

            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Application.ExecutablePath;
            openFileDialog1.FileName = "*.xml";
            openFileDialog1.Filter = "XML config files|.XML";
            dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {

                try
                {
                    xmlr = new XMLParmsFileReader(openFileDialog1.FileName, false); //false = not a batch run, output file defs not required

                    Library[] libs = new Library[xmlr.DirectoryCount];
                    for (short i = 0; i < xmlr.DirectoryCount; ++i)
                    {
                        short i2 = i;
                        libs[i] = new Library(++i2, xmlr.DirLabels[i], xmlr.DirList[i], xmlr.DirHosts[i]);
                    }
                    StatusUpdate("Parsing skeletons, please wait.");
                    _sp = new SkeletonParser(xmlr.ConfigName, xmlr.ConfigNum, new LibraryConcatenation(libs, xmlr.IgnoreList),
                        xmlr.DebugInputLine, false);
                    _lc = _sp.LibraryConcatenation;

                    _sp.CreateTables(ref _SDS, ref _TDS);
                    _sp.AddUnreferencedSkeletons(ref _SDS, _lc, xmlr);

                }
                catch (ISPFSkeletonParser.XmlParmsFileReaderErrors.XMLTagException e)
                {
                    validParmsFile = false;
                    tagErrMsg = e.Message;
                }
                catch (Exception e)
                {
                    validParmsFile = false;
                    tagErrMsg = e.Message;
                }
            }
            else
                validParmsFile = false;

            if (validParmsFile)
                return xmlr;
            else
            {
                if (dr == DialogResult.OK)
                    MessageBox.Show("The file you specified was not a configuration file for the parser." +
                        tagErrMsg == "" ? "" : "\n" + tagErrMsg,
                        "XML Parms file error.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                    StatusUpdate("No configuration file was selected.");
                return (XMLParmsFileReader)null;
            }

        }

        /// <summary>
        /// Allow the user to specify an new XML parms file from the one selected on program startup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfig_Click(object sender, EventArgs e)
        {
            if (_openForms.Count > 0)
            {
                DialogResult dr = MessageBox.Show("Press OK to close these and open a new configuration.\n" +
                    "Press Cancel to bypass the new configuration selection.",
                    "Warning: There are open query forms.",
                    MessageBoxButtons.OKCancel);
                if (dr == DialogResult.Cancel)
                    return;
            }
            if (UserInit())
                this.Text = _label;
        }

        /// <summary>
        /// On the first display of the form (_firstActivation = true), ask the user for an XML Parms
        /// file and process that file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Query_Activated(object sender, EventArgs e)
        {
            if (_firstActivation)
            {
                _firstActivation = false;
                if (UserInit())
                    // show the QueryDisplayName from the XML file in the window title bar.
                    this.Text = _label;
            }
        }
        //
        // This method is called from the child QueryExpand2 form to re-enable the
        // Expand button after the child form completes the expansion of a skeleton.
        public void btnExpandEnable()
        {
            btnExpand.Enabled = true;
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            bool dispose=false;
            QueryExpand2 qv2 = null;
            btnExpand.Enabled = false;
            string[] FTINCL_Set = new string[] {cboSkeletons.SelectedItem.ToString().ToUpper(), Query.NULL_SKEL,
                Query.NULL_SKEL, Query.NULL_SKEL};
            
            int offset = 1;
            if (cboSkeletons2.SelectedItem.ToString().ToUpper() != Query.NULL_SKEL
                && cboSkeletons2.SelectedItem.ToString() != cboSkeletons.SelectedItem.ToString().ToUpper())
                FTINCL_Set[offset++] = cboSkeletons2.SelectedItem.ToString().ToUpper();

            if (cboSkeletons3.SelectedItem.ToString().ToUpper() != Query.NULL_SKEL
                && cboSkeletons3.SelectedItem.ToString().ToUpper() != cboSkeletons.SelectedItem.ToString().ToUpper()
                && cboSkeletons3.SelectedItem.ToString().ToUpper() != cboSkeletons2.SelectedItem.ToString().ToUpper())
                FTINCL_Set[offset++] = cboSkeletons3.SelectedItem.ToString().ToUpper();
            
            qv2 = new QueryExpand2(out dispose, FTINCL_Set, _SDS, _TDS, QueryImageList, _label, _xmlRdr, this);

            if (!dispose)
                qv2.Show();
            else
            {
                btnExpandEnable();
                this.StatusUpdate("Expansion failed.");
            }
            cboSkeletons.SelectedIndex = 0;
            cboSkeletons2.SelectedIndex = 0;
            cboSkeletons3.SelectedIndex = 0;
        }

        public void RemoveForm(Form f, bool dispose)
        {
            RemoveForm(f);
            if (dispose)
                f.Dispose();
            int gen = GC.GetGeneration(f);
            GC.Collect(gen,GCCollectionMode.Forced);
        }
        public void RemoveForm(Form f)
        {
            _openForms.Remove(f);
        }

        public void AddForm(Form f)
        {
            _openForms.Add(f);
        }
        public void StatusUpdate(string statusText)
        {
            statusStrip1.Items["StatusLabel"].Text = statusText;
            this.Refresh();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About AboutForm = new About(this.QueryImageList);
            AboutForm.ShowDialog();
        }

        private void btnPrograms_Click(object sender, EventArgs e)
        {
            QueryProgram qp = new QueryProgram(_SDS, _TDS, QueryImageList, _label, _xmlRdr,this);
            qp.Show();
        }

        //private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Version thisVersion = GlenwoodUtilities.CheckVersion.GetVersion();
        //    Version webVersion = GlenwoodUtilities.CheckVersion.GetVersion(@"http://www.glenwoodconsulting.com/app_Version.xml");
        //    int comparison = thisVersion.CompareTo(webVersion);
        //    switch (comparison)
        //    {
        //        case -1:
        //            GlenwoodUtilities.MessageBoxHL.Show("You may download the new version here:", title: "There is a newer version of the Skeleton Utility");
        //            break;
        //        case 0:
        //            MessageBox.Show("Your version of the Skeleton Utility is the most current version.");
        //            break;
        //        case 1:
        //            MessageBox.Show("The web version is older than this version");
        //            break;
        //        default:
        //            break;
        //    }

        //}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
