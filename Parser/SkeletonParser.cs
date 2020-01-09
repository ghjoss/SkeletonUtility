using SkeletonParserDSDef;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace ISPFSkeletonParser
{
    public partial class SkeletonParser
    {
        LibraryConcatenation _libCat;

        public LibraryConcatenation LibraryConcatenation
        {
            get { return _libCat; }
            set { _libCat = value; }
        }

        string _configurationName;
        short _configurationNumber;

        private bool _cVerbose;

        private SkeletonParserDS _parserDataset = new SkeletonParserDS();

        public SkeletonParserDS ParserDataSet
        {
            get { return _parserDataset; }
        }

        private TransientDS _transientDataSet = new TransientDS();

        public TransientDS TransientDataSet
        {
            get { return _transientDataSet; }
        }

        // The ISPF _variable cross-reference tracks the desired information for the variables found when
        // the _skeleton lines are parsed. This is done for each _variable using the Variable class.
        public class Variable
        {
            public string _inputTextLine;
            public string _variable;
            public  short _typeCode;
            public string _skeleton;
            public int _lineNumber;
            public short _position;
            public short _commandCode;
        }

        // ISPF functions and arguments to functions are tracked in the Built_in class
        public class Built_in
        {
            public string _function;
            public string _argument;
            public string _skeleton;
            public int _lineNumber;
            public short _position;
        }

        public class SkeletonCommands
        {
            public string _skeleton;
            public int _lineNumber;
            public int _endLineNumber;
            public short _skeletonCmd;
            public short _position;
        }

        private Stack<SkeletonCommands> 
                _selStack = null,
                _doStack = null,
                _dotStack = null,
                _rexxStack = null;

        private List<SkeletonCommands> _cmdList = null;

        // The _skeleton class defines all of the fields we track for _skeleton-to-_skeleton cross-references
        public class Skeleton
        {
            public string _skeleton;
            public int _concatOffsetparent;
            public string _childSkeleton;
            public int _concatOffsetChild;
            public int _lineNumber;
            public bool _opt;
            public bool _noft;
            public bool _extended;
            public string _amper;
        }
        /// <summary>
        /// The DOT class defines all of the fields we track for skeleton-to-DOT cross-references
        /// </summary>
        public class DOT
        {
            public string _skeleton;
            public string _table;
            public int _lineNumber;
        }
        /// <summary>
        /// The Program class defines all of the fields we track for skeleton-to-Called Program cross-references
        /// </summary>
        public class Program
        {
            public string _skeleton;
            public string _program;
            public int _lineNumber;
            public string _inputTextLine;
        }
        // The printManifest class is used to pass references to the parsed data ArrayLists and Dictionary
        // to the nested print classes. There are no methods for printManifest.
        public class PrintManifest
        {
            public string _configurationName;
            public short _configurationNumber;
            public List<Variable> _varList;           // of Variable objects
            public List<Skeleton> _skelList;          // of Skeleton objects
            public List<DOT> _dotList;               // of DOT objects
            public List<Program> _progList;
            public List<Built_in> _builtInsList;     // of Built_in objects
            public List<Library> _libList;           // of Library  objects
            public Dictionary<string, short> _concatenations;
            public List<string> _nestingErrors;
        }

        PrintManifest _PM = new PrintManifest();

        // _varList is a collection of Variable objects 
        private List<Variable> _varList = new List<Variable>(50000);

        // _skelList is a collection of _skeleton objects.
        private List<Skeleton> _skelList = new List<Skeleton>(5000);

        // _functionList is a collection of Built_in objects.
        private List<Built_in> _functionList = new List<Built_in>(5000);

        // _dotList is a collection of table (DOT) objects
        private List<DOT> _dotList = new List<DOT>(5000);

        // _programList is a collection of called program objects
        private List<Program> _programList = new List<Program>(5000);


        // _embeddedVar:
        // The regular expression used to find variables in _skeleton _commandCode lines cannot
        // be static because the "&" prefix can be changed on the fly by the )DEFAULT _commandCode.
        // _embeddedVar is set to the static value defined in class SkeletonClassRE, but 
        // it is modified whenever a )DEFAULT statement is encountered. It is reset to the
        // original value whenever a new _skeleton is opened.
        // Since the parser does not process skeletons when it encounters an )IM _commandCode, there
        // is no need to worry about toggling this value for )IM.
//        private string _embeddedVar = SkeletonParserRE.EMBEDDED_VARIABLE;
        private string _embeddedVar = SkeletonParserRE.EMBEDDED_VARIABLE;

        // cDbgInput line is set when the invoking program requests that the parsed input line be written
        // to the XML or Excel output files.
        private bool cDbgInputLine = false;

        public bool DebugInputLine
        {
            get { return cDbgInputLine; }
            set { cDbgInputLine = value; }
        }

        // _amp stores the value of the variable indicator. It is either & (the default)
        // or it is set to some other character by a )DEFAULT command code encountered while
        // parsing.
        private string _amp = "&"; // default: use & for _variable indicator

        // _continue stores the value of the line continuation indicator. It is either
        // ? (the default) or it is set to some other character by a )DEFAULT _commandCode
        // encountered while parsing.
        private string _continue = "?"; // default: use ? for the line continuation indicator

        // _position is the relative order of a variable in a line. Thus in the following line:
        // )SEL &X = &Y
        // X has _position=1 and Y has _position=2. If the data is stored in a relational database,
        // _position will have to be part of the key to ensure uniqueness. Otherwise the line
        // )SET X = &X+&X
        // would generate three references to X for the same line. And the second and third
        // references would have the same type (VARIABLE_REFERENCE) and be otherwise
        // indistinguishable.
        // One last note: in order to ensure that ISPF skeleton functions, which start with
        // an &, are not mistaken for variables; all functions are parsed first if found
        // in a line. Thus the functions will have _position values lower than any variables
        // in the same line. Thus:
        // )SETF X = &SUBSTR(&STR(&Y ZZ),3,1)
        // SUBSTR: _position=1
        // STR:    _position=2
        // X:      _position=3
        // Y:      _position=4
        // Due to this anomaly, caused by command code parsing, _position does not really convey
        // useful information to the end user.
        private short _position = 0;

        /// <summary>
        /// VariableCollectionComparer is a class to allow us to sort the cVarsList collection by 
        /// _skeleton (asc) / _variable name (asc) / Line number (asc) / _position (asc) 
        /// </summary>
        public class VariableCollectionComparer : IComparer<Variable>
        {
            int IComparer<Variable>.Compare(Variable left, Variable right)
            {
                return toStr(left).CompareTo(toStr(right));
            }
            private string toStr(Variable v)
            {
                return v._skeleton.PadRight(9, '_') + v._variable.PadRight(9, '_') + v._lineNumber.ToString("000000") +
                    "_" + v._position.ToString("00");
            }
        }

        /// <summary>
        /// DOTCollectionComparer is a class to allow us to sort the cDOTsList collection by 
        /// _skeleton (asc) / _tablee name (asc) / Line number (asc)
        /// </summary>
        public class DOTCollectionComparer : IComparer<DOT>
        {
            int IComparer<DOT>.Compare(DOT left, DOT right)
            {
                return toStr(left).CompareTo(toStr(right));
            }
            private string toStr(DOT v)
            {
                return v._skeleton.PadRight(9, '_') + v._table.PadRight(30, '_') + v._lineNumber.ToString("000000");
            }
        }

        /// <summary>
        /// ProgramCollectionComparer is a class to allow us to sort the cProgramsList collection by 
        /// _skeleton (asc) / _program name (asc) / Line number (asc)
        /// </summary>
        public class ProgramCollectionComparer : IComparer<Program>
        {
            int IComparer<Program>.Compare(Program left, Program right)
            {
                return toStr(left).CompareTo(toStr(right));
            }
            private string toStr(Program v)
            {
                return v._skeleton.PadRight(9, '_') + v._program.PadRight(30, '_') + v._lineNumber.ToString("000000");
            }
        }
        public class SkeletonCommandsComparer : IComparer<SkeletonCommands>
        {
            int IComparer<SkeletonCommands>.Compare(SkeletonCommands left, SkeletonCommands right)
            {
                return toStr(left).CompareTo(toStr(right));
            }
            private string toStr(SkeletonCommands s)
            {
                return s._skeleton + s._lineNumber.ToString("#####") + s._position.ToString("##");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configName">
        /// Configuration name. Used in the fixed file table definitions. All tables defined have the configuration name
        /// as the first field.
        /// </param>
        /// <param name="configNum">
        /// A unique number to define the configuration when loaded into a relational table. Use a key lookup to get the
        /// configuration name.
        /// </param>
        /// <param name="lc">
        /// LibaryConcatenation object containing a List<> of members associated with the mainframe library
        /// concatenation.
        /// </param>
        public SkeletonParser(string configName, short configNum, LibraryConcatenation lc, bool verbose) 
        {
            _libCat = lc;
            _configurationName = configName;
            _configurationNumber = configNum;
#if DEBUG
            _cVerbose = verbose;
#else
            _cVerbose = false;
#endif
            LoadData(lc);
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configName">
        /// Configuration name. Used in the fixed file table definitions. All tables defined have the configuration name
        /// as the first field.
        /// </param>
        /// <param name="configNum">
        /// A unique number to define the configuration when loaded into a relational table. Use a key lookup to get the
        /// configuration name.
        /// </param>
        /// <param name="lc">
        /// LibaryConcatenation object containing a List<> of members associated with the mainframe library
        /// concatenation.
        /// </param>
        /// <param name="printInputLine">
        /// A boolean _variable indicating if the variables output files will contain a copy of the parsed _skeleton
        /// input line.
        /// </param>
        public SkeletonParser(string configName, short configNum, LibraryConcatenation lc, bool printInputLine,
            bool verbose)
        {
            _libCat = lc;
            cDbgInputLine = printInputLine;
            _configurationName = configName;
            _configurationNumber = configNum;
#if DEBUG
            _cVerbose = verbose;
#else
            _cVerbose = false;
#endif
            LoadData(lc);
        }

        void ExpandData(LibraryConcatenation cLc)
        {
        }
        /// <summary>
        /// Iterate over the _skeleton members, looking for _variable references, )IM references and ISPF _skeleton functions.
        /// </summary>
        /// <param name="cLibCat">
        /// LibaryConcatenation object containing a List<> of members associated with the mainframe library
        /// concatenation.
        /// </param>
        void LoadData(LibraryConcatenation cLibCat)
        {
            SkeletonCommands 
                    cmd = null,
                    cmd2 = null;

            string  line,
                    skel,
                    lineForParse,
                    exprLeft,
                    exprRight,
                    cond2,
                    command = "",
                    ifCmd = "",
                    rval = "";
            string[] rexVarArr = null;

            var nestingErrors = new List<string>(50);

            char[]  cArr = null,
                rexxVarDelim = ",&.?".ToCharArray();
            
            Match   match1,
                    match2;
            
            int     lineNumber = 0,
                    prevLineNumber = 0,
                    p = 0,
                    p2 = 0;
            StreamReader sr;

            Variable variable;
            Skeleton skeleton;
            Built_in function;
            DOT dot;
            Program prgm;

            short   
                nestingKeyIdentity = 0, // forces unique key for cmdnesting table.
                kw;

            bool
                bLvalIndirect = false,
                lineContinue = false,
                inLineREXX = false,
                doPush = false;
            TransientDS.SkeletonLinesRow slr;

            _embeddedVar = SkeletonParserRE.EMBEDDED_VARIABLE;

            _cmdList = new List<SkeletonCommands>(5000);
            
            foreach (LibraryConcatenation.SkelLibFile libConcatFile in cLibCat.MemberList)
            {
               CheckUnderflow(nestingErrors);

               _selStack = new Stack<SkeletonCommands>(20);
               _doStack = new Stack<SkeletonCommands>(100);
               _dotStack = new Stack<SkeletonCommands>(10);
               _rexxStack = new Stack<SkeletonCommands>(2);
            
                sr = new StreamReader(libConcatFile.filePath);
                skel = libConcatFile.fileName;
                if (_cVerbose)
                    Console.WriteLine("File: " + libConcatFile.filePath);

                line = sr.ReadLine();

                _position = 0;
                nestingKeyIdentity = 0; // reset function position. Increased for each skel command on a line.
                lineNumber = 1;
                lineContinue = false;

                while (line != null)
                {
                    ++nestingKeyIdentity;
                    // remove any line number or other data in columns 73-80 by truncating longer
                    // lines
                    if (line.Length > 72)
                        line = line.Substring(0, 72);

                    //
                    // Get rid of & pairs (&& and &&&&). do not remove free standing pairs, such as
                    // might occur in a )SEL <e.g. )SEL &A = X && &B = Y>
                    //
                    p = line.IndexOf("&&");
                    if (p > 0)
                        p2 = line.IndexOf(" && ", p - 1); // not to be replaced in sel (could be AND)

                    while(p >= 0)
                    {
                        if (p == 0)
                            line = "  " + line.Substring(2);
                       else
                            if (p2 != p-1)
                                line = line.Substring(0,p-1)+"  "+line.Substring(p+2);

                        p = line.IndexOf("&&",p+1);
                        if (p > 0)
                            p2 = line.IndexOf(" && ",p-1);
                    }

                    // Substring throws an exception if the range overflows the string length. When looking for
                    // continuation characters, only look in lines that are 72 characters long.
                    if (lineContinue & command != "")
                    {
                        doPush = false;
                        lineForParse = ")" + command + line;
                    }
                    else
                    {
                        doPush = true;
                        lineForParse = line;
                    }

                    lineContinue = line.Length == 72 ? (line.Substring(71,1) == _continue ? true : false) : false;

                    if (lineNumber != prevLineNumber)
                    {
                        slr = (TransientDS.SkeletonLinesRow)_transientDataSet.SkeletonLines.NewRow();
                        slr.LineNumber = lineNumber;
                        slr.Skeleton = libConcatFile.fileName;
                        slr.LineText = line;
                        try
                        {
                            _transientDataSet.SkeletonLines.AddSkeletonLinesRow(slr);
                        }
                        catch (System.Data.ConstraintException e)
                        {
                            prevLineNumber = lineNumber;
                        }
                    }
                    #region Built-in _commandCode scanning
                    match2 = Regex.Match(lineForParse, SkeletonParserRE.BUILT_INS2);
                    if (match2.Success)
                    {

                        cArr = line.ToCharArray();
                        while (match2.Success)
                        {
                            string  sWork = match2.Groups["function"].Value,
                                    sWork2 = match2.Groups["arg"].Value;
                            int l = match2.Groups["builtin"].Value.Length;
                            sWork = match2.Groups["function"].Value;
                            // VarListAdd changes _position, which will be used when Built_in function is filled with values.
                            VarListAdd(line, lineNumber, sWork, 
                                SkeletonParserRE.cVarTypes[SkeletonParserRE.FUNCTION], libConcatFile.fileName, SkeletonParserRE.cKeywords[sWork]);
                            function = new Built_in();
                            function._skeleton = libConcatFile.fileName;
                            function._function = sWork;
                            function._lineNumber = lineNumber;
                            function._position = _position;
                            function._argument = match2.Groups["arg"].Value;
                            _functionList.Add(function);
                            // Overlay the _commandCode and parentheses in the character array with spaces
                            (new string(' ', l)).ToCharArray().CopyTo(cArr,match2.Index);
                            // replace closing paren with a space
                            cArr[match2.Index+l+sWork2.Length] = ' ';
                            // Look for another function on this line
                            match2 = Regex.Match(new string(cArr), SkeletonParserRE.BUILT_INS2);
                        }
                        // replace the line with the data that has the functions removed
                        lineForParse = new string(cArr);
                    }
                    #endregion

                    // extract _skeleton commands
                    match1 = Regex.Match(lineForParse, SkeletonParserRE.COMMAND_PARSE);
                    if (match1.Success)
                    {
                        command = match1.Groups["Command"].Value;
                        switch (command)
                        #region _skeleton _commandCode processing
                        {
                            // bypass tab, and other non-_variable dependent lines
                            case "TB":
                            case "CM":
                            case "TBA":
                            case "BLANK":
                                break; // not "continue" because we need to read a new line from the file

                            case "SET":
                            case "SETF":
                                #region )SET and )SETF processing
                                cmd = new SkeletonCommands();
                                cmd._lineNumber = lineNumber;
                                cmd._position = nestingKeyIdentity;
                                cmd._endLineNumber = lineNumber;
                                cmd._skeleton = libConcatFile.fileName;

                                if (command == "SET")
                                {
                                    kw = SkeletonParserRE.cKeywords["SET"];
                                    cmd._skeletonCmd = kw;
                                }
                                else
                                {
                                    kw = SkeletonParserRE.cKeywords["SETF"];
                                    cmd._skeletonCmd = kw;
                                }
                                _cmdList.Add(cmd);

                                match1 = Regex.Match(lineForParse, SkeletonParserRE.SET_PARSE);
                                while(match1.Success)
                                {
                                    variable = new Variable();
                                    variable._lineNumber = lineNumber;
                                    variable._position = ++_position;
                                    rval = match1.Groups["VariableName"].Value;
                                    if (rval.StartsWith(_amp))
                                    {
                                        rval = rval.Substring(1);
                                        bLvalIndirect = true;
                                    }
                                    else
                                        bLvalIndirect = false;

                                    variable._variable = rval;
                                    if (rval.Equals(_amp + "Z"))
                                    {
                                        if (bLvalIndirect)
                                            variable._typeCode = SkeletonParserRE.cVarTypes[SkeletonParserRE.LVAL_INDIRECT_NULL_ASSIGNMENT];
                                        else
                                            variable._typeCode = SkeletonParserRE.cVarTypes[SkeletonParserRE.LVAL_NULL_ASSIGNMENT];
                                        variable._skeleton = libConcatFile.fileName;
                                        variable._commandCode = kw;
                                        _varList.Add(variable);
                                    }
                                    else
                                    {
                                        if (bLvalIndirect)
                                            variable._typeCode = SkeletonParserRE.cVarTypes[SkeletonParserRE.LVAL_INDIRECT_ASSIGNMENT];
                                        else
                                            variable._typeCode = SkeletonParserRE.cVarTypes[SkeletonParserRE.LVAL_ASSIGNMENT];
                                        variable._skeleton = libConcatFile.fileName;
                                        variable._commandCode = kw;
                                        _varList.Add(variable);

                                    }                                    

                                    string s = match1.Groups["exprL"].Value;
                                    short localType;
                                    if (s.StartsWith(_amp))
                                        localType = SkeletonParserRE.cVarTypes[SkeletonParserRE.RVAL_VARIABLE_ASSIGNMENT];
                                    else
                                        localType = SkeletonParserRE.cVarTypes[SkeletonParserRE.CONSTANT_REFERENCE];

                                    EmbeddedVariableParser(line, s, lineNumber, libConcatFile.fileName,
                                        kw, localType);
                                    s = match1.Groups["exprR"].Value;
                                    match1 = Regex.Match(s,SkeletonParserRE.SET_PARSE);
                                }
                                #endregion
                                break;

                            case "SEL":
                            case "IF":
                                #region )IF & )SEL processing
                                kw = command == "SEL" ? SkeletonParserRE.cKeywords["SEL"] : SkeletonParserRE.cKeywords["IF"];
                                // strip offset the )SEL or )IF / THEN parts of the statement
                                match1 = Regex.Match(lineForParse, SkeletonParserRE.COND_PARSE);
                                cmd = new SkeletonCommands();
                                cmd._lineNumber = lineNumber;
                                cmd._position = nestingKeyIdentity;
                                cmd._skeleton = libConcatFile.fileName;
                                if (kw == SkeletonParserRE.cKeywords["IF"])
                                {
                                    cmd._endLineNumber = lineNumber;
                                    cmd._skeletonCmd = kw;
                                    _cmdList.Add(cmd);
                                }
                                else if(doPush)
                                {
                                    cmd._endLineNumber = 0;
                                    cmd._skeletonCmd = kw;
                                    _selStack.Push(cmd);
                                }


                                if (match1.Success)
                                {
                                    if (kw == SkeletonParserRE.cKeywords["IF"])
                                        ifCmd = match1.Groups["cmd"].Value.Trim();

                                    match1 = Regex.Match(match1.Groups["Conditionals"].Value, SkeletonParserRE.COND_PHRASE);
                                    while (match1.Success) // exprL, exprR, COND2
                                    {
                                        exprLeft = match1.Groups["exprL"].Value;     // left side of first conditional phrase
                                        exprRight = match1.Groups["exprR"].Value;    // right side of first conditional phrase
                                        cond2 = match1.Groups["COND2"].Value;        // the rest of the phrase (minus the conjunction)
                                        // parse the left side for variables. There may be more than 1
                                        EmbeddedVariableParser(line, exprLeft, lineNumber, libConcatFile.fileName, kw, SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_TEST]);
                                        // parse the right side for variables. There may be more than one
                                        EmbeddedVariableParser(line, exprRight, lineNumber, libConcatFile.fileName, kw, SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_TEST]);
                                        // shift to the next conditional sub-phrase (if any)
                                        match1 = Regex.Match(cond2, SkeletonParserRE.COND_PHRASE);
                                    }
                                }
                                if (kw == SkeletonParserRE.cKeywords["IF"] & ifCmd != "")
                                {
                                    line = ifCmd;
                                    prevLineNumber = lineNumber;
                                    continue;
                                }
                                #endregion
                                break;

                            case "DO":
                                #region )DO Processing
                                cmd = new SkeletonCommands();
                                cmd._lineNumber = lineNumber;
                                cmd._position = nestingKeyIdentity;
                                cmd._endLineNumber = 0;
                                cmd._skeleton = libConcatFile.fileName;
                                cmd._skeletonCmd = SkeletonParserRE.cKeywords["DO"];
                                _doStack.Push(cmd);

                                match1 = Regex.Match(lineForParse, SkeletonParserRE.DO_PARSE, RegexOptions.IgnorePatternWhitespace);
                                if (match1.Success)
                                {
                                    #region DO <count>
                                    rval = match1.Groups["DoLoopCt"].Value.Trim();
                                    if (rval != "")
                                    {
                                        VarListAdd(line,lineNumber, rval, rval.Contains("&") ? SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_REFERENCE] : SkeletonParserRE.cVarTypes[SkeletonParserRE.CONSTANT_REFERENCE], libConcatFile.fileName, SkeletonParserRE.cKeywords["DO"]);
                                        break;
                                    }
                                    #endregion
                                    #region Do <var = start TO end BY inc FOR count>
                                    rval = match1.Groups["LoopIter"].Value.Trim();
                                    if (rval != "")
                                    {
                                        VarListAdd(line,lineNumber, rval, SkeletonParserRE.cVarTypes[SkeletonParserRE.LVAL_ASSIGNMENT], libConcatFile.fileName, SkeletonParserRE.cKeywords["DO"]);
                                        rval = match1.Groups["LoopStart"].Value.Trim();
                                        if (rval != "")
                                        {
                                            VarListAdd(line,lineNumber, rval, rval.Contains("&") ? SkeletonParserRE.cVarTypes[SkeletonParserRE.RVAL_VARIABLE_ASSIGNMENT] : SkeletonParserRE.cVarTypes[SkeletonParserRE.CONSTANT_REFERENCE], libConcatFile.fileName, SkeletonParserRE.cKeywords["DO"]);
                                        }
                                        rval = match1.Groups["LoopEnd"].Value.Trim();
                                        if (rval != "")
                                        {
                                            VarListAdd(line,lineNumber, rval, rval.Contains("&") ? SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_REFERENCE] : SkeletonParserRE.cVarTypes[SkeletonParserRE.CONSTANT_REFERENCE], libConcatFile.fileName, SkeletonParserRE.cKeywords["DO"]);
                                        }
                                        rval = match1.Groups["LoopInc"].Value.Trim();
                                        if (rval != "")
                                        {
                                            VarListAdd(line,lineNumber, rval, rval.Contains("&") ? SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_REFERENCE] : SkeletonParserRE.cVarTypes[SkeletonParserRE.CONSTANT_REFERENCE], libConcatFile.fileName, SkeletonParserRE.cKeywords["DO"]);
                                        }
                                        rval = match1.Groups["LoopCnt"].Value.Trim();
                                        if (rval != "")
                                        {
                                            VarListAdd(line,lineNumber, rval, rval.Contains("&") ? SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_REFERENCE] : SkeletonParserRE.cVarTypes[SkeletonParserRE.CONSTANT_REFERENCE], libConcatFile.fileName, SkeletonParserRE.cKeywords["DO"]);
                                        }
                                    }
                                    #endregion
                                    #region Do <ConditionalClause>
                                    rval = match1.Groups["Conditionals"].Value.Trim();
                                    if (rval != "")
                                    {
                                        kw = match1.Groups["WhileUntil"].Value == "WHILE" ? SkeletonParserRE.cKeywords["DOWHILE"] : SkeletonParserRE.cKeywords["DOUNTIL"];
                                        match1 = Regex.Match(rval, SkeletonParserRE.COND_EXPRESSION);
                                        if (match1.Success)
                                        {
                                            while (match1.Success) // exprL, exprR, COND2
                                            {
                                                exprLeft = match1.Groups["exprL"].Value;     // left side of first conditional phrase
                                                exprRight = match1.Groups["exprR"].Value;    // right side of first conditional phrase
                                                cond2 = match1.Groups["COND2"].Value;        // the rest of the phrase (minus the conjunction)
                                                // parse the left side for variables. There may be more than 1
                                                EmbeddedVariableParser(line, exprLeft, lineNumber, libConcatFile.fileName, kw, SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_TEST]);
                                                // parse the right side for variables. There may be more than one
                                                EmbeddedVariableParser(line, exprRight, lineNumber, libConcatFile.fileName, kw, SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_TEST]);
                                                // shift to the next conditional sub-phrase (if any)
                                                match1 = Regex.Match(cond2, SkeletonParserRE.COND_PHRASE);
                                            }
                                        }
                                    }
                                    #endregion
                                }
#endregion
                                break;

                            case "DOT":
                                #region )DOT Processing
                                cmd = new SkeletonCommands();
                                cmd._lineNumber = lineNumber;
                                cmd._position = nestingKeyIdentity;
                                cmd._endLineNumber = 0;
                                cmd._skeleton = libConcatFile.fileName;
                                cmd._skeletonCmd = SkeletonParserRE.cKeywords["DOT"];
                                _dotStack.Push(cmd);

                                kw = SkeletonParserRE.cKeywords["DOT"];
                                match1 = Regex.Match(lineForParse, SkeletonParserRE.DOT_PARSE);
                                if (match1.Success)
                                {
                                    rval = match1.Groups["TableName"].Value;
                                    EmbeddedVariableParser(line, rval, lineNumber, libConcatFile.fileName, kw, SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_REFERENCE]);
                                    dot = new DOT();
                                    dot._skeleton = libConcatFile.fileName;
                                    dot._table = rval;
                                    dot._lineNumber = lineNumber;
                                    _dotList.Add(dot);
                                }

                                #endregion
                                break;

                            case "IM":
                                #region )IM processing
                                cmd = new SkeletonCommands();
                                cmd._lineNumber = lineNumber;
                                cmd._position = nestingKeyIdentity;
                                cmd._skeleton = libConcatFile.fileName;
                                cmd._skeletonCmd = SkeletonParserRE.cKeywords["IM"];
                                cmd._endLineNumber = lineNumber + 1;
                                _cmdList.Add(cmd);

                                kw = SkeletonParserRE.cKeywords["IM"];
                                match1 = Regex.Match(lineForParse, SkeletonParserRE.IM_PARSE);
                                if (match1.Success)
                                {
                                    rval = match1.Groups["ImbeddedSkelName"].Value;
                                    EmbeddedVariableParser(line, rval, lineNumber, libConcatFile.fileName, kw, SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_REFERENCE]);
                                }
                                skeleton = new Skeleton();
                                skeleton._skeleton = libConcatFile.fileName;
                                skeleton._childSkeleton = rval;
                                skeleton._lineNumber = lineNumber;
                                skeleton._opt = false;
                                skeleton._noft = false;
                                skeleton._extended = false;
                                skeleton._amper = _amp;

                                rval = match1.Groups["IMModifiers"].Value;
                                if (rval.Trim() != "")
                                {
                                match1 = Regex.Match(rval, SkeletonParserRE.IM_PARSE_OPT);
                                if (match1.Success)
                                    skeleton._opt = match1.Groups["_opt"].Value == "OPT";

                                match1 = Regex.Match(rval, SkeletonParserRE.IM_PARSE_NT);
                                if (match1.Success)
                                    skeleton._noft = match1.Groups["nt"].Value == "NT";

                                match1 = Regex.Match(rval, SkeletonParserRE.IM_PARSE_EXT);
                                if (match1.Success)
                                    skeleton._extended = match1.Groups["ext"].Value == "EXT";

                                }

                                _skelList.Add(skeleton);
                                #endregion
                                break;

                            case "REXX":
                                #region )REXX processing
                                cmd = new SkeletonCommands();
                                cmd._lineNumber = lineNumber;
                                cmd._position = nestingKeyIdentity;
                                cmd._skeleton = libConcatFile.fileName;
                                cmd._skeletonCmd = SkeletonParserRE.cKeywords["REXX"];
                                cmd._endLineNumber = 0;
                                
                                match1 = Regex.Match(lineForParse, SkeletonParserRE.REXX_PARSE);
                                if (match1.Success)
                                {
                                    rval = match1.Groups["varlist"].Value;
                                    rexVarArr = rval.Split(rexxVarDelim);
                                    foreach (string s in rexVarArr)
                                    {
                                        rval = s.Trim();
                                        if (rval != "")
                                            VarListAdd(line, lineNumber, rval, SkeletonParserRE.cVarTypes[SkeletonParserRE.REXX_CALL_VARIABLE_REFERENCE],
                                                libConcatFile.fileName, SkeletonParserRE.cKeywords["REXX"]);
                                    }
                                    if (!lineContinue)
                                    {
                                        rval = match1.Groups["rexxProcedure"].Value;
                                        if (rval == "")
                                        {
                                            _rexxStack.Push(cmd);
                                            rval = "EX )REXX";
                                            inLineREXX = true;
                                        }
                                        else
                                            _cmdList.Add(cmd);
                                        VarListAdd(line, lineNumber, rval, SkeletonParserRE.cVarTypes[SkeletonParserRE.REXX_PROCEDURE], libConcatFile.fileName, SkeletonParserRE.cKeywords["REXX"]);
                                    }
                                }
                                #endregion
                                break;

                            case "ENDREXX":
                                #region )ENDREXX processing
                                if (_rexxStack.Count > 0) 
                                {
                                    cmd = _rexxStack.Pop();
                                    cmd._endLineNumber = lineNumber;
                                    _cmdList.Add(cmd);
                                }
                                else
                                    nestingErrors.Add(")ENDREXX without )REXX in " + libConcatFile.fileName + 
                                        " at line " + lineNumber.ToString());


                                inLineREXX = false;
                                #endregion
                                break;

                            case "ELSE":
                                #region )ELSE processing
                                cmd = new SkeletonCommands();
                                cmd._lineNumber = lineNumber;
                                cmd._position = nestingKeyIdentity;
                                cmd._endLineNumber = lineNumber;
                                cmd._skeleton = libConcatFile.fileName;
                                cmd._skeletonCmd = SkeletonParserRE.cKeywords["ELSE"];
                                _cmdList.Add(cmd);

                                match1 = Regex.Match(lineForParse, SkeletonParserRE.ELSE_PARSE);
                                if (match1.Success)
                                {
                                    line = match1.Groups["ElseCmd"].Value;
                                    prevLineNumber = lineNumber;
                                    continue;
                                }
                                #endregion
                                break;

                            case "ENDSEL":
                                #region )ENDSEL processing
                                if (_selStack.Count > 0)
                                {
                                    cmd = _selStack.Pop();
                                    cmd._endLineNumber = lineNumber;
                                    _cmdList.Add(cmd);
                                }
                                else
                                    nestingErrors.Add(")ENDSEL without )SEL in " + libConcatFile.fileName +
                                        " at line " + lineNumber.ToString());
                                #endregion
                                break;
                            
                            case "ENDDO":
                                #region )ENDDO processing
                                if (_doStack.Count > 0)
                                {
                                    cmd = _doStack.Pop();
                                    cmd._endLineNumber = lineNumber;
                                    _cmdList.Add(cmd);
                                }
                                else
                                    nestingErrors.Add(")ENDDO without )DO in " + libConcatFile.fileName +
                                        " at line " + lineNumber.ToString());
                                #endregion
                                break;

                            case "ENDDOT":
                                #region )ENDDOT processing
                                if (_dotStack.Count > 0)
                                {
                                    cmd = _dotStack.Pop();
                                    cmd._endLineNumber = lineNumber;
                                    _cmdList.Add(cmd);
                                }
                                else
                                    nestingErrors.Add(")ENDDOT without )DOT in " + libConcatFile.fileName +
                                        " at line " + lineNumber.ToString());
                                #endregion
                                break;

                            case "LEAVE":
                                #region )LEAVE processing
                                cmd = null;
                                if (line.Contains("DOT"))
                                    if (_dotStack.Count > 0)
                                        cmd = _dotStack.Peek();
                                    else
                                        nestingErrors.Add(")LEAVE DOT without a matching )DOT in " + libConcatFile.fileName + 
                                            " at line " + lineNumber.ToString());
                                else if (_doStack.Count > 0)
                                    cmd = _doStack.Peek();
                                else
                                    nestingErrors.Add(")LEAVE without a matching )DO in " + libConcatFile.fileName + 
                                        " at line " + lineNumber.ToString());
                                if (cmd != null)
                                {
                                    cmd2 = new SkeletonCommands();
                                    cmd2._skeleton = cmd._skeleton;
                                    cmd2._lineNumber = cmd._lineNumber;
                                    cmd2._position = ++nestingKeyIdentity;
                                    cmd2._endLineNumber = lineNumber;
                                    cmd2._skeletonCmd = SkeletonParserRE.cKeywords["LEAVE"];
                                    _cmdList.Add(cmd2);
                                }
                                #endregion
                                break;

                            case "ITERATE":
                                #region )ITERATE processing
                                if (_doStack.Count > 0)
                                {
                                    cmd = _doStack.Peek();
                                    cmd2 = new SkeletonCommands();
                                    cmd2._skeleton = cmd._skeleton;
                                    cmd2._lineNumber = cmd._lineNumber;
                                    cmd2._position = ++nestingKeyIdentity;
                                    cmd2._endLineNumber = lineNumber;
                                    cmd2._skeletonCmd = SkeletonParserRE.cKeywords["ITERATE"];
                                    _cmdList.Add(cmd2);
                                }
                                else
                                    nestingErrors.Add(")ITERATE without matching )DO in " + libConcatFile.fileName + 
                                        " at line " + lineNumber.ToString());
                                #endregion
                                break;

                            case "DEFAULT":
                                #region )DEFAULT processing
                                match1 = Regex.Match(lineForParse, SkeletonParserRE.DEFAULT);
                                if (match1.Success)
                                {
                                    _amp = match1.Groups["ampOver"].Value;
                                    _continue = match1.Groups["continueOver"].Value;
                                    _embeddedVar = SkeletonParserRE.L_VARIABLE + _amp + SkeletonParserRE.R_VARIABLE;
                                }
                                #endregion
                                break;

                            default:
                                break;

                        }
                        #endregion
                    }
                    else if (!inLineREXX)
                    {
                        #region DATALINE processing
                        command = "";
                        match1 = Regex.Match(lineForParse, _embeddedVar);
                        while (match1.Success)
                        {
                            string sWork = match1.Groups["VariableName"].Value;
                            string rest = match1.Groups["postData"].Value;
                            VarListAdd(line,lineNumber, sWork, SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_REFERENCE], libConcatFile.fileName, SkeletonParserRE.cKeywords["DATALINE"]);
                            match1 = Regex.Match(match1.Groups["postData"].Value, _embeddedVar);
                        }
                        match1 = Regex.Match(lineForParse, SkeletonParserRE.PGM_PARSE);
                        if (match1.Success)
                        {
                            prgm = new Program();

                            prgm._program = match1.Groups["ProgramName"].Value; 
                            prgm._skeleton = libConcatFile.fileName;
                            prgm._lineNumber = lineNumber;
                            prgm._inputTextLine = line;
                            _programList.Add(prgm);
                        }
                        #endregion
                    }

                    if ((line = sr.ReadLine()) != null)
                    {
                        prevLineNumber = lineNumber;
                        ++lineNumber;
                        _position = 0;
                        nestingKeyIdentity = 0;
                    }
                }

            }
            
            CheckUnderflow(nestingErrors);

            var vlcomp = new VariableCollectionComparer();
            _varList.Sort(vlcomp);
            var sCmdsComp = new SkeletonCommandsComparer();
            _cmdList.Sort(sCmdsComp);

            _PM._builtInsList = _functionList;
            _PM._varList = _varList;
            _PM._skelList = _skelList;
            _PM._dotList = _dotList;
            _PM._progList = _programList;
            _PM._libList = cLibCat.Concatenation;
            _PM._concatenations = cLibCat.MemberConcatenation;
            _PM._configurationName = _configurationName;
            _PM._configurationNumber = _configurationNumber;
            _PM._nestingErrors = nestingErrors;
        }
        
        /// <summary>
        /// Check the stacks for left-over values. These are CMDS without a
        /// matching )END...
        /// </summary>
        /// <param name="_nestingErrors"></param>
        private void CheckUnderflow(List<string> nestingErrors)
        {
            SkeletonCommands cmd = null;
            if (_selStack == null)
                return;
            while (_selStack.Count > 0)
            {
                cmd = _selStack.Pop();
                nestingErrors.Add(")SEL without matching )ENDSEL in " + cmd._skeleton +
                    " at line " + cmd._lineNumber.ToString());
            }
            while (_doStack.Count > 0)
            {
                cmd = _doStack.Pop();
                nestingErrors.Add(")DO without matching )ENDDO in " + cmd._skeleton +
                    " at line " + cmd._lineNumber.ToString());
            }
            while (_doStack.Count > 0)
            {
                cmd = _dotStack.Pop();
                nestingErrors.Add(")DOT without matching )ENDDOT in " + cmd._skeleton +
                    " at line " + cmd._lineNumber.ToString());
            }
            while (_rexxStack.Count > 0)
            {
                cmd = _rexxStack.Pop();
                nestingErrors.Add("imbedded )REXX without matching )ENDREXX in " + cmd._skeleton +
                    " at line " + cmd._lineNumber.ToString());
            }

        }

        /// <summary>
        /// iterate over a subset of a line, looking for all embedded variables
        /// </summary>
        /// <param name="line">full line, for output purposes if DebugOutputLine is set</param>
        /// <param name="s">substring of full line</param>
        /// <param name="_lineNumber">current line number in current _skeleton</param>
        /// <param name="fileName">_skeleton name</param>
        /// <param name="_commandCode">Current _commandCode being processed</param>
        /// <param name="vartyp">_variable type represented by this subset</param>
        private void EmbeddedVariableParser(string line, string s, int lineNumber, string fileName, short command, short vartyp)
        {
            Match match1;
            short lvartyp = 0;
            bool bConst = false;
            string firstChar = "";

            if (s.StartsWith(_amp))
            {
                match1 = Regex.Match(s, _embeddedVar);
                lvartyp = vartyp;
                bConst = false;
            }
            else
            {
                match1 = Regex.Match(s, SkeletonParserRE.CONSTANT);
                lvartyp = CheckEmbeddedVartyp(vartyp);
                bConst = true;
            }
            while (match1.Success)
            {
                string varName = match1.Groups["VariableName"].Value;
                /* check if non-letter and non-digit is the first char, then this is not a variable or const */
                firstChar = varName.Substring(0, 1);
//                if (firstChar == _amp | !Regex.Match(firstChar, @"\W").Success)
                if (firstChar == _amp | !Regex.Match(firstChar, @"[^A-Z#$@_]").Success)
                {
                    if (varName == "Z" && !bConst)
                        varName = "NULL(&Z)";
                    VarListAdd(line, lineNumber, varName, lvartyp, fileName, command);
                }
                if ((s = match1.Groups["postData"].Value) == "")
                    break;

                if (s.StartsWith(_amp))
                {
                    match1 = Regex.Match(s, _embeddedVar);
                    lvartyp = SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_REFERENCE];
                    bConst = false;
                }
                else
                {
                    match1 = Regex.Match(s, SkeletonParserRE.CONSTANT);
                    lvartyp = CheckEmbeddedVartyp(vartyp);
                    bConst = true;
                }
            }
            return;
        }

        short CheckEmbeddedVartyp(short vartyp)
        {
            if (vartyp == SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_TEST])
                return SkeletonParserRE.cVarTypes[SkeletonParserRE.CONSTANT_TEST];
            else if (vartyp == SkeletonParserRE.cVarTypes[SkeletonParserRE.VARIABLE_REFERENCE])
                return SkeletonParserRE.cVarTypes[SkeletonParserRE.CONSTANT_REFERENCE];
            else if (vartyp == SkeletonParserRE.cVarTypes[SkeletonParserRE.RVAL_VARIABLE_ASSIGNMENT])
                return SkeletonParserRE.cVarTypes[SkeletonParserRE.CONSTANT_REFERENCE];
            else
                return vartyp;

        }
        /// <summary>
        /// Add a _variable structure to the _varList List<>
        /// </summary>
        /// <param name="line">
        /// text of input line</param>
        /// <param name="_lineNumber">
        /// offset of line in file</param>
        /// <param name="name">
        /// _variable name or 1st 8 chars of a constant reference</param>
        /// <param name="type">type (e.g. left side of assignment, constant, test (if,sel)...</param>
        /// <param name="skel">_skeleton in which _variable is</param>
        /// <param name="func">ISPF commnad [)SEL, )SET, )IF, )DO ...</param>
        private void VarListAdd(string line,int lineNumber, string name, short type, string skel, short func)
        {
            var v = new Variable();
            v._inputTextLine = line;
            v._lineNumber = lineNumber;
            v._position = ++_position;
            v._variable = name;
            v._typeCode = type;
            v._skeleton = skel;
            v._commandCode = func;
            _varList.Add(v);
            return;
        }

        /// <summary>
        /// Exposed method for printing the fixed format files.
        /// </summary>
        /// <param name="fixedOutputFileHLQ"></param>
        public void FixedVarsWriter(string fixedOutputFileHLQ)
        {
            var x = new SkeletonParserFixedFileWriter(_PM, fixedOutputFileHLQ);
        }

        /// <summary>
        /// Exposed method for printing the XML format files.
        /// </summary>
        /// <param name="xmlOutputFileHLQ"></param>
        public void XMLVarsWriter(string xmlOutputFileHLQ)
        {
            var x = new SkeletonParserXMLFileWriter(_PM, xmlOutputFileHLQ, cDbgInputLine);
        }

        /// <summary>
        /// Exposed method for printing Excel Comma Separated Value files.
        /// </summary>
        /// <param name="ExcelOutputFileHLQ"></param>
        public void ExcelVarsWriter(string ExcelOutputFileHLQ)
        {
           var x = new SkeletonParserExcelFileWriter(_PM, ExcelOutputFileHLQ,cDbgInputLine);
        }

        public void ErrorsLogger(StreamWriter sw)
        {
            if (_PM._nestingErrors.Count > 0)
            {
                sw.WriteLine("Command nesting errors encountered.");
                foreach (string s in _PM._nestingErrors)
                    sw.WriteLine(s);
            }
            return;
        }

    }
}
