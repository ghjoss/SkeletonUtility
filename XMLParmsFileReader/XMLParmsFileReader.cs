using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace ISPFSkeletonParser
{
    public class XmlParmsFileReaderErrors
    {
        [Serializable]
        public class XMLTagException : System.Exception
        {
            public XMLTagException(string message)
            {
                throw new Exception("Error(s) in XML file.\n" + message);
            }
        }
        [Serializable]
        public class NoDirectoriesSpecifedException : System.Exception
        {
            public NoDirectoriesSpecifedException()
            {
                throw new Exception("There were no input directory names specified in the XML parameter file\n");
            }
        }
        [Serializable]
        public class NoOutputFileSpecifiedException : System.Exception
        {
            public NoOutputFileSpecifiedException()
            {
                throw new Exception("There were no output files specified in the XML parameter file\n");
            }
        }
        [Serializable]
        public class XMLFileNotFoundException : System.Exception
        {
            public XMLFileNotFoundException(string fileName)
            {
                throw new Exception("The specified XML parameter file(" + fileName + ") does not exist.\n");
            }
        }
    }
    public class UnimbeddedSkeletons
    {
        private string _skeleton;
        public string Skeleton
        {
            get { return _skeleton; }
            set { _skeleton = value; }
        }
        private string _parent;

        public string Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        private bool _opt;
        public bool OPT
        {
            get { return _opt; }
            set { _opt = value; }
        }

        private bool _ext;
        public bool EXT
        {
            get { return _ext; }
            set { _ext = value; }
        }

        private bool _noft;
        public bool NOFT
        {
            get { return _noft; }
            set { _noft = value; }
        }

        public UnimbeddedSkeletons() { }
    }
    public class XMLParmsFileReader
    {
        #region class data
        private const int MAXDIRCOUNT = 128; // previously 20
        public const int MAXCONFIGURATIONLENGTH = 30;
        private const int NOCONFIG = -32767;

        private string
            _FullExpand = "",
            _XMLFileHLQ = "",
            _excelFileHLQ = "",
            _fixedFileHLQ = "",
            _XMLFileFolder = "",
            _excelFileFolder = "",
            _fixedFileFolder = "",
            _XMLParmsFileFullPath = "",
            _XMLParmsFilePath = "",
            _XMLParmsFileName = "",
            _QueryDisplayName = "";

        public string FullExpand
        {
            get { return _FullExpand; }
            set { _FullExpand = value; }
        }

        public string XMLFileHLQ
        {
            get { return _XMLFileHLQ; }
        }
        //private string XMLFileHLQ
        //{
        //    set { _XMLFileHLQ = value;}
        //}

        public string CSVFileHLQ
        { 
            get { return _excelFileHLQ; } 
        }
        //private string CSVFileHLQ
        //{
        //    set { _excelFileHLQ = value; }
        //}

        public string TXTFileHLQ { 
            get { return _fixedFileHLQ; }
        }
        //private string TXTFileHLQ
        //{
        //    set { _fixedFileHLQ = value; }
        //}

        public string XMLFileFolder
        {
            get { return _XMLFileFolder; }
        }
        //private string XMLFileFolder
        //{
        //    set { _XMLFileFolder = value; }
        //}

        public string CSVFileFolder
        {
            get { return _excelFileFolder; }
        }
        //private string CSVFileFolder
        //{
        //    set { _excelFileFolder = value; }
        //}
        
        public string TXTFileFolder
        {
            get { return _fixedFileFolder; }
        }
        //private string TXTFileFolder
        //{
        //    set { _fixedFileFolder = value; }
        //}
        
        public string QueryDisplayName
        {
            get { return _QueryDisplayName; }
        }
        //private string QueryDisplayName
        //{
        //    set { _QueryDisplayName = value; }
        //}

        public string XMLParmsFileName
        {
            get { return _XMLParmsFileName; }
        }
        //private string XMLParmsFileName
        //{
        //    set { _XMLParmsFileName = value; }
        //}

        public string XMLParmsFilePath
        {
            get { return _XMLParmsFilePath; }
        }
        //private string XMLParmsFilePath
        //{
        //    set { _XMLParmsFilePath = value; }
        //}

        public string XMLParmsFileFullPath
//        private string XMLParmsFileFullPath
        {
            get { return _XMLParmsFileFullPath; }
            set { _XMLParmsFileFullPath = value; }
        }

        public string FixedFile
        {
            get { return _fixedFileFolder + _fixedFileHLQ; }
        }
        //private string FixedFile
        //{
        //    set { _fixedFileFolder + _fixedFileHLQ = value; }
        //}

        public string XMLFile
        {
            get { return _XMLFileFolder + _XMLFileHLQ; }
        }
        //private string XMLFile
        //{
        //    set { _XMLFileFolder + _XMLFileHLQ = value; }
        //}

        public string ExcelFile
        {
            get { return _excelFileFolder + _excelFileHLQ; }
        }
        //private string ExcelFile
        //{
        //    set { _excelFileFolder + _excelFileHLQ = value; }
        //}

        private string[] _dirList = new string[MAXDIRCOUNT],
                        _dirHosts = new string[MAXDIRCOUNT],
                        _dirLabels = new string[MAXDIRCOUNT];

        public string[] DirLabels
        {
            get { return _dirLabels; }
        }
//        private string[] DirLabels
//        {   
//            set {
//                _dirList = 
//              for (string s in value)
//                    _dirLabels = value; }
//        }
        
        public string[] DirHosts
        {
            get { return _dirHosts; }
        }
        
        public string[] DirList
        {
            get { return _dirList; }
        }

        private Hashtable _ignoreList = new Hashtable(100);
        public Hashtable IgnoreList
        {
            get { return _ignoreList; }
        }

        private List<UnimbeddedSkeletons> _unreferencedList = new List<UnimbeddedSkeletons>(1000);
        public List<UnimbeddedSkeletons> UnreferencedList
        {
            get { return _unreferencedList; }
        }

        private short _configNum = NOCONFIG;
        public short ConfigNum
        {
            get { return _configNum; }
        }

        private string _configName = null;
        public string ConfigName
        {
            get { return _configName; }
        }

        int _directoryCount = 0;
        public int DirectoryCount
        {
            get { return _directoryCount; }
        }

        private bool _debugInputLine = false;
        public bool DebugInputLine
        {
            get { return _debugInputLine; }
        }

        private bool _imbedRecurseFail = false;
        public bool ImbedRecurseFail
        {
            get { return _imbedRecurseFail; }
            set { _imbedRecurseFail = value; }
        }
#endregion
        private XMLParmsFileReader()
        {

        }
        /// <summary>
        /// Read an XML configuration file. Throw errors on missing or invalidly constructed tags. Throw an error on a file not found.
        /// If a batch run, throw an error if there is not output file specified (otherwise why are we running at all?).
        /// </summary>
        /// <param name="xmlInputFile">XML configuration parameters file.</param>
        public XMLParmsFileReader(string xmlInputFile, bool batchFile)
        {
            if (!File.Exists(xmlInputFile))
                throw new XmlParmsFileReaderErrors.XMLFileNotFoundException(xmlInputFile);
            Init(xmlInputFile);
            if (_directoryCount == 0)
                throw new XmlParmsFileReaderErrors.NoDirectoriesSpecifedException();
            if (_excelFileFolder != "" && !_excelFileFolder.EndsWith(@"\"))
                _excelFileFolder = _excelFileFolder + @"\";
            if (_XMLFileFolder != "" && !_XMLFileFolder.EndsWith(@"\"))
                _XMLFileFolder = _XMLFileFolder + @"\";
            if (_fixedFileFolder != "" && !_fixedFileFolder.EndsWith(@"\"))
                _fixedFileFolder = _fixedFileFolder + @"\";
            //
            //There must be at least one requested output file type for the batch utility
            //
            if (batchFile && (ExcelFile == "" && XMLFile == "" && FixedFile == ""))
                throw new XmlParmsFileReaderErrors.NoOutputFileSpecifiedException();
        }

        //   Process the XML input, storing values in the class private variables.
        //
        //   Throws errors if unknown XML <xxx> elements are found or if invalid
        //   values are given for some of the <xxx> elements, e.g. the match pattern
        //   is missing or not a valid regular expression.
        private enum SkeletonParserXMLParms
        {
            ConfigurationName,          //
            ConfigurationNumber,        //
            DebugInputLine,             // for Variable & Program data output files: Print input skeleton line
            Directory,                  // Group element tag for DirectoryName,DirectoryLable,DirectoryHostName
            DirectoryName,              // PC Folder
            DirectoryLabel,             // Generic label for this folder: e.g. Test, Production, User
            DirectoryHostName,          // Mainframe PDS name
            XMLOuputFolder,             // Directory to which XML data output is written
            XMLOutputHLQ,               // Fixed, user defined part of XML file name
            excelOutputFolder,          // Directory to which CSV data output is written
            excelOutputHLQ,             // Fixed, user defined part of CSV file name
            fixedOutputFolder,          // Directory to which TXT (fixed format) data output is written
            fixedOutputHLQ,             // Fixed, user defined part of TXT file name
            Ignore,                     // Tag for files that are not to be parsed as skeletons
            ImbedRecurseFail,           // Method for handling recursive imbeds: A imbeds B imbeds C imbeds A ...
            NoType,
            QueryDisplayName,           // Title line for the Query GUI
            Unimbedded                  // Unimbedded tag
        };

        private void Init(string XMLFullFileName)
        {
            XmlTextReader xmlReader = null;

            bool xmlReaderOpen = false;

            SkeletonParserXMLParms nodeT = SkeletonParserXMLParms.NoType;

            bool bScriptError = false;

            StringBuilder errMsg = new StringBuilder(1000);

            string __dir = "";


            try
            {
                string FILE_EXT = @"(?<fName>.+)\..*";

                System.Text.RegularExpressions.Match m;
                FileInfo f = new FileInfo(XMLFullFileName);
                if ((f.Extension) != "")
                {
                    m = System.Text.RegularExpressions.Regex.Match(f.Name, FILE_EXT);
                    _XMLParmsFilePath = f.DirectoryName;
                    _XMLParmsFileName = m.Groups["fName"].Value;
                    _XMLParmsFileFullPath = f.DirectoryName + @"\" + m.Groups["fName"].Value;
                }
                else
                {
                    _XMLParmsFilePath = f.DirectoryName;
                    _XMLParmsFileName = f.Name;
                    _XMLParmsFileFullPath = f.DirectoryName + @"\" + f.Name;
                }
                xmlReader = new XmlTextReader(XMLFullFileName);
                xmlReaderOpen = true;

                xmlReader.WhitespaceHandling = WhitespaceHandling.None;

                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {

                        case XmlNodeType.Element: //Display beginning of element.
                            switch (xmlReader.Name.ToUpper())
                            {
                                case "ISPFSKELETONPARSER":
                                    break;
                                case "QUERYDISPLAYNAME":
                                    nodeT = SkeletonParserXMLParms.QueryDisplayName;
                                    break;
                                case "CONFIGURATIONNAME":
                                    nodeT = SkeletonParserXMLParms.ConfigurationName;
                                    break;
                                case "CONFIGURATIONNUMBER":
                                    nodeT = SkeletonParserXMLParms.ConfigurationNumber;
                                    break;
                                case "DIRECTORY":
                                    nodeT = SkeletonParserXMLParms.Directory;
                                    if (_directoryCount >= MAXDIRCOUNT)
                                    {
                                        bScriptError = true;
                                        errMsg.Append("Directory count > " + MAXDIRCOUNT.ToString() + ": " + XmlNodeType.Text.ToString() + "\n");
                                    }
                                    else
                                    {
                                        _dirList[_directoryCount] = "";
                                        _dirHosts[_directoryCount] = "";
                                        _dirLabels[_directoryCount] = "";
                                    }
                                    break;
                                case "DIRECTORYNAME":
                                    nodeT = SkeletonParserXMLParms.DirectoryName;
                                    break;
                                case "DIRECTORYLABEL":
                                    nodeT = SkeletonParserXMLParms.DirectoryLabel;
                                    break;
                                case "DIRECTORYHOSTNAME":
                                    nodeT = SkeletonParserXMLParms.DirectoryHostName;
                                    break;
                                case "XMLOUTPUTFOLDER":
                                    nodeT = SkeletonParserXMLParms.XMLOuputFolder;
                                    break;
                                case "XMLOUTPUTHLQ":
                                    nodeT = SkeletonParserXMLParms.XMLOutputHLQ;
                                    break;
                                case "EXCELOUTPUTFOLDER":
                                    nodeT = SkeletonParserXMLParms.excelOutputFolder;
                                    break;
                                case "EXCELOUTPUTHLQ":
                                    nodeT = SkeletonParserXMLParms.excelOutputHLQ;
                                    break;
                                case "FIXEDOUTPUTFOLDER":
                                    nodeT = SkeletonParserXMLParms.fixedOutputFolder;
                                    break;
                                case "FIXEDOUTPUTHLQ":
                                    nodeT = SkeletonParserXMLParms.fixedOutputHLQ;
                                    break;
                                case "IGNORESKEL":
                                    nodeT = SkeletonParserXMLParms.Ignore;
                                    break;
                                case "UNIMBEDDED":
                                    nodeT = SkeletonParserXMLParms.Unimbedded;
                                    UnimbeddedSkeletons us;
                                    if (xmlReader.HasAttributes && xmlReader.AttributeCount >= 2 && xmlReader.AttributeCount <= 5)
                                    {
                                        nodeT = SkeletonParserXMLParms.NoType;
                                        string work;
                                        bool tryparse, boolWork;
                                        us = new UnimbeddedSkeletons
                                        {
                                            Parent = xmlReader.GetAttribute("Parent"),
                                            Skeleton = xmlReader.GetAttribute("Skeleton")
                                        };
                                        
                                        work = xmlReader.GetAttribute("EXT");
                                        if (work == null)
                                            us.EXT = false;
                                        else
                                        {
                                            tryparse = Boolean.TryParse(work, out boolWork);
                                            if (!tryparse)
                                            {
                                                bScriptError = true;
                                                errMsg.Append("Line " + xmlReader.LineNumber.ToString("####") +
                                                    ": XML Node EXT value not \"true\" or \"false\"");
                                            }
                                            else
                                                us.EXT = boolWork;
                                        }
                                        work = xmlReader.GetAttribute("OPT");
                                        if (work == null)
                                            us.OPT = false;
                                        else
                                        {
                                            tryparse = Boolean.TryParse(work, out boolWork);
                                            if (!tryparse)
                                            {
                                                bScriptError = true;
                                                errMsg.Append("Line " + xmlReader.LineNumber.ToString("####") +
                                                    ": XML Node OPT value not \"true\" or \"false\"");
                                            }
                                            else
                                                us.OPT = boolWork;
                                        }
                                        work = xmlReader.GetAttribute("NOFT");
                                        if (work == null)
                                            us.NOFT = false;
                                        else
                                        {
                                            tryparse = Boolean.TryParse(work, out boolWork);
                                            if (!tryparse)
                                            {
                                                bScriptError = true;
                                                errMsg.Append("Line " + xmlReader.LineNumber.ToString("####") +
                                                    ": XML Node NOFT value not \"true\" or \"false\"");
                                            }
                                            else
                                                us.NOFT = boolWork;
                                        }
                                        if (us.Skeleton == null || us.Parent == null)
                                        {
                                            bScriptError = true;
                                            errMsg.Append("Line " + xmlReader.LineNumber.ToString("####") + 
                                                ": XML Node missing parent and/or skeleton name\n");
                                        }
                                        else
                                            _unreferencedList.Add(us);
                                    }    
                                    break;
                                case "DEBUGINPUTLINE":
                                    nodeT = SkeletonParserXMLParms.DebugInputLine;
                                    break;
                                case "XMLOUTPUT":
                                case "EXCELOUTPUT":
                                case "FIXEDOUTPUT":
                                case "CONFIGURATION":
                                case "IMBEDCHECK":
                                    break;
                                case "IMBEDRECURSEFAIL":
                                    nodeT = SkeletonParserXMLParms.ImbedRecurseFail;
                                    break;
                                default:
                                    bScriptError = true;
                                    errMsg.Append(xmlReader.Name + " is not a valid element type.\n");
                                    break;
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (xmlReader.Name.ToUpper() == "DIRECTORY")
                                ++_directoryCount;
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            switch (nodeT)
                            {
                                case SkeletonParserXMLParms.QueryDisplayName:
                                    _QueryDisplayName = xmlReader.Value.ToString();
                                    break;
                                case SkeletonParserXMLParms.excelOutputFolder:
                                    _excelFileFolder = __dir = xmlReader.Value.ToString();
                                    if (!Directory.Exists(__dir))
                                    { 
                                        bScriptError = true;
                                        errMsg.Append("Tag=<ExcelOutputFolder>, Error=" + _excelFileFolder + " does not exist.\n");
                                    }
                                    break;
                                case SkeletonParserXMLParms.ConfigurationName:
                                    if ((_configName = xmlReader.Value.ToString()).Length > MAXCONFIGURATIONLENGTH)
                                    {
                                        bScriptError = true;
                                        errMsg.Append("Tag=<Configuration>, Error=" + _configName + " has more than " +
                                            MAXCONFIGURATIONLENGTH.ToString() + "characters.\n");
                                    }
                                    break;
                                case SkeletonParserXMLParms.ConfigurationNumber:
                                    try
                                    {
                                        _configNum = System.Convert.ToInt16(xmlReader.Value.ToString());
                                    }
                                    catch (OverflowException e)
                                    {
                                        bScriptError = true;
                                        errMsg.Append("Tag=<ConfigurationNumber>, Error=Config out of range(1-32767)\n");
                                        errMsg.Append(e.Message + "\n");
                                    }
                                    break;
                                case SkeletonParserXMLParms.excelOutputHLQ:
                                    _excelFileHLQ = xmlReader.Value.ToString();
                                    break;
                                case SkeletonParserXMLParms.XMLOuputFolder:
                                    _XMLFileFolder = __dir = xmlReader.Value.ToString();
                                    if (! Directory.Exists(_XMLFileFolder))
                                    {
                                        bScriptError = true;
                                        errMsg.Append("Tag=<XMLOutputFolder>, Error=" + _XMLFileFolder + " does not exist.\n");
                                    }
                                    break;
                                case SkeletonParserXMLParms.XMLOutputHLQ:
                                    _XMLFileHLQ = xmlReader.Value.ToString();
                                    break;
                                case SkeletonParserXMLParms.fixedOutputFolder:
                                    _fixedFileFolder = __dir = xmlReader.Value.ToString();
                                    if (!Directory.Exists(_fixedFileFolder))
                                    {
                                        bScriptError = true;
                                        errMsg.Append("Tag=<FixedOutputFolder>, Error=" + _fixedFileFolder + " does not exist.\n");
                                    }
                                    break;
                                case SkeletonParserXMLParms.fixedOutputHLQ:
                                    _fixedFileHLQ = xmlReader.Value.ToString();
                                    break;
                                case SkeletonParserXMLParms.DirectoryHostName:
                                    _dirHosts[_directoryCount] = xmlReader.Value.ToString(); ;
                                    break;
                                case SkeletonParserXMLParms.DirectoryLabel:
                                    _dirLabels[_directoryCount] = xmlReader.Value.ToString(); ;
                                    break;
                                case SkeletonParserXMLParms.DirectoryName:
                                    _dirList[_directoryCount] = __dir = xmlReader.Value.ToString();
                                    if (!Directory.Exists(__dir))
                                    {
                                        errMsg.Append("Tag=<DirectoryName>, Error=" + __dir + " does not exist.\n");
                                        bScriptError = true;
                                    }
                                    break;
                                case SkeletonParserXMLParms.Ignore:
                                    string ign = xmlReader.Value.ToString();
                                    if (!_ignoreList.Contains(ign))
                                        _ignoreList.Add(ign, ign);
                                    break;
                                case SkeletonParserXMLParms.DebugInputLine:
                                    string dbi = xmlReader.Value.ToString().ToUpper();
                                    if (dbi == "Y" | dbi == "YES" | dbi == "TRUE")
                                        _debugInputLine = true;
                                    break;

                                case SkeletonParserXMLParms.ImbedRecurseFail:
                                    string rif = xmlReader.Value.ToString().ToUpper();
                                    if (rif == "Y" | rif == "YES" | rif == "TRUE")
                                        _imbedRecurseFail = true;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                } // end while
            } // end try
            catch (Exception ex)
            {
                errMsg.Append(ex.Message);
                bScriptError = true;
            }
            finally
            {
                if (xmlReaderOpen)
                {
                    xmlReader.Close();
                    xmlReaderOpen = false;
                }
                if (_configNum == NOCONFIG)
                {
                    bScriptError = true;
                    errMsg.Append("Tag <ConfigurationNumber> was missing. You must specify a configuration ID number.\n");
                }

                if (bScriptError)
                    throw new XmlParmsFileReaderErrors.XMLTagException(errMsg.ToString());
            }


        }

        public XMLParmsFileReader Clone()
        {
            XMLParmsFileReader cloneXMLReader = (XMLParmsFileReader) this.MemberwiseClone();
            return cloneXMLReader;
        }

        public override string ToString()
        {
            return "XMLParmsFileReader/XMLFILE: " + this.XMLFile;
        }

    }
}

