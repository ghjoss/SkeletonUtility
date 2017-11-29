using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using SkeletonParserDSDef;


namespace ISPFSkeletonParser
{
    public partial class SkeletonParser
    {
        public static readonly string varDSN = ".Variables";
        public static readonly string skelDSN = ".Skeletons";
        public static readonly string dotDSN = ".DOT";
        public static readonly string progDSN = ".Programs";
        public static readonly string funcDSN = ".Functions";
        public static readonly string libDSN = ".Libraries";
        public static readonly string kwDSN = ".Keywords";
        public static readonly string vtDSN = ".Vartypes";


        public class SkeletonParserExcelFileWriter
        {
            public SkeletonParserExcelFileWriter(PrintManifest pm, string csvHLQ, bool dbgInputLine)
            {
                ExcelCSVVarsWriter(pm._varList, csvHLQ + SkeletonParser.varDSN + ".csv", dbgInputLine);
                ExcelCSVSkelsWriter(pm._skelList, pm._concatenations, csvHLQ + SkeletonParser.skelDSN + ".csv");
                ExcelCSVDOTsWriter(pm._dotList, csvHLQ + SkeletonParser.dotDSN + ".csv");
                ExcelCSVProgsWrite(pm._progList, csvHLQ + SkeletonParser.progDSN + ".csv", false);
                ExcelCSVFunctionsWriter(pm._builtInsList, csvHLQ + SkeletonParser.funcDSN + ".csv");
                ExcelCSVLibsWriter(pm._libList, csvHLQ + SkeletonParser.libDSN + ".csv");
            }
            private void ExcelCSVLibsWriter(List<Library> libsList, string excelFileName)
            {
                StreamWriter sw = new StreamWriter(excelFileName);
                sw.WriteLine("Offset in Concatenation, HostName, PC Name, User Tag");
                sw.WriteLine("-1,Unknown,?,?");
                int ord = 0;
                foreach (Library l in libsList)
                {
                    sw.WriteLine(
                        ord++.ToString("d") + "," +
                        l.HostPDS + "," +
                        l.Folder + "," +
                        l.Tag);
                }
                sw.Close();
            }
            static public void ExcelCSVSkelsWriter(List<Skeleton> skelList, Dictionary<string, short> concatenations, string excelFileName)
            {
                int imbedLib = 0;
                StreamWriter sw = new StreamWriter(excelFileName);

                sw.WriteLine("Parent Skel, Concatenation Offset, Imbedded Skel, Concatenated Offset,Line Number, Optional, No File Tailoring, Extended");
                foreach (SkeletonParser.Skeleton sk in skelList)
                {
                    if (sk._childSkeleton.Contains(sk._amper) || !concatenations.ContainsKey(sk._childSkeleton))
                        imbedLib = -1;
                    else
                        imbedLib = concatenations[sk._childSkeleton];
                    sw.WriteLine(
                        sk._skeleton + "," +
                        concatenations[sk._skeleton].ToString("d") + "," +
                        sk._childSkeleton + "," +
                        imbedLib.ToString("d") + "," +
                        sk._lineNumber.ToString("####0") + "," +
                        (sk._opt ? "OPT" : "") + "," +
                        (sk._noft ? "NOFT" : "") + "," +
                        (sk._extended ? "EXT" : "NOEXT"));
                }
                sw.Close();
            }
            static public void ExcelCSVDOTsWriter(List<DOT> dotList, string excelFileName)
            {
                StreamWriter sw = new StreamWriter(excelFileName);
                sw.WriteLine("Skeleton,Table,Line Number");
                foreach (SkeletonParser.DOT d in dotList)
                {
                    sw.WriteLine(
                        d._skeleton + "," +
                        d._table + "," +
                        d._lineNumber.ToString("####0")
                        );
                }
                sw.Close();
            }
            static public void ExcelCSVProgsWrite(List<Program> progList, string excelFileName, bool dbgInputLine)
            {
                StreamWriter sw = new StreamWriter(excelFileName);
                sw.WriteLine("Skeleton,Program,Line Number" + (dbgInputLine ? ",Line Data": ""));
                foreach (SkeletonParser.Program p in progList)
                {
                    sw.WriteLine(
                        p._skeleton + "," +
                        p._program + "," +
                        p._lineNumber.ToString("####0") +
                        (dbgInputLine ? ",\"" + p._inputTextLine + "\"" : "")
                        );
                }
                sw.Close();
            }
            static public void ExcelCSVVarsWriter(List<Variable> varList, string excelFileName, bool dbgInputLine)
            {
                StreamWriter sw = new StreamWriter(excelFileName);
                sw.WriteLine("Skeleton,Variable,Line Number,Position,Command(N),Command(A),Type(N),Type(A),Line Data");
                foreach (SkeletonParser.Variable v in varList)
                {
                    sw.WriteLine(
                            v._skeleton + "," +
                            v._variable + "," +
                            v._lineNumber.ToString("####0") + "," +
                            v._position.ToString("#0") + "," +
                            v._commandCode.ToString("d") + "," +
                            SkeletonParserRE.cKeywords.DictReverseLookup(v._commandCode) + "," +
                            v._typeCode.ToString("d") + "," +
                            SkeletonParserRE.cVarTypes.DictReverseLookup(v._typeCode) +
                            (dbgInputLine ? ",\"" + v._inputTextLine + "\"" : ""));

                }
                sw.Close();
            }
            private void ExcelCSVFunctionsWriter(List<Built_in> built_insList, string excelFileName)
            {
                StreamWriter sw = new StreamWriter(excelFileName);
                sw.WriteLine("Skeleton,_function,Line Number,Function Argument List");
                foreach (SkeletonParser.Built_in bi in built_insList)
                {
                    sw.WriteLine(
                            bi._skeleton + "," +
                            bi._function + "," +
                            bi._lineNumber.ToString("####0") + "," +
                            bi._argument);
                }
                sw.Close();
            }
        }

        public class SkeletonParserXMLFileWriter
        {
            public SkeletonParserXMLFileWriter(PrintManifest pm, string XMLOutputFileHLQ, bool dbgInputLine)
            {
                XMLVarsWriter(pm._varList, XMLOutputFileHLQ + SkeletonParser.varDSN + ".xml", dbgInputLine);
                XMLSkelsWriter(pm._skelList, pm._concatenations, XMLOutputFileHLQ + SkeletonParser.skelDSN + ".xml");
                XMLDOTsWriter(pm._dotList, XMLOutputFileHLQ + SkeletonParser.dotDSN + ".xml");
                XMLProgsWriter(pm._progList, XMLOutputFileHLQ + SkeletonParser.progDSN + ".xml", false);
                XMLFunctionsWriter(pm._builtInsList, XMLOutputFileHLQ + SkeletonParser.funcDSN + ".xml");
                XMLLibsWriter(pm._libList, XMLOutputFileHLQ + SkeletonParser.libDSN + ".xml");
            }

            private void XMLLibsWriter(List<Library> libsList, string xmlFileName)
            {
                int ord = 0;

                XmlTextWriter xw = new XmlTextWriter(xmlFileName, System.Text.Encoding.UTF8);
                xw.Formatting = Formatting.Indented;
                xw.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xw.WriteStartElement("libraries");
                xw.WriteStartElement("library");
                xw.WriteAttributeString("HostName", "Unknown");
                xw.WriteAttributeString("PCName", "?");
                xw.WriteAttributeString("UserTag", "?");
                xw.WriteAttributeString("Offset", "-1");
                xw.WriteEndElement();

                foreach (Library l in libsList)
                {
                    xw.WriteStartElement("library");
                    xw.WriteAttributeString("HostName", l.HostPDS);
                    xw.WriteAttributeString("PCName", l.Folder);
                    xw.WriteAttributeString("UserTag", l.Tag);
                    xw.WriteAttributeString("Offset", ord++.ToString("d"));
                    xw.WriteEndElement();
                }
                xw.WriteEndElement(); //libraries
                xw.Close();
            }

            public static void XMLVarsWriter(List<Variable> varList, string xmlFileName, bool dbgInputLine)
            {
                string currentSkel = "";
                XmlTextWriter xw = new XmlTextWriter(xmlFileName, System.Text.Encoding.UTF8);
                xw.Formatting = Formatting.Indented;
                xw.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xw.WriteStartElement("variables");
                foreach (SkeletonParser.Variable v in varList)
                {
                    if (v._skeleton != currentSkel)
                    {
                        string m = new string('=', v._skeleton.Length);
                        currentSkel = v._skeleton;
                        xw.WriteComment("===" + m + "===");
                        xw.WriteComment("== " + v._skeleton + " ==");
                        xw.WriteComment("===" + m + "===");
                    }
                    xw.WriteStartElement("_variable");
                    xw.WriteAttributeString("Name", v._variable);
                    xw.WriteAttributeString("Skeleton", v._skeleton);
                    xw.WriteAttributeString("Command", SkeletonParserRE.cKeywords.DictReverseLookup(v._commandCode));
                    xw.WriteAttributeString("CommandCode", v._commandCode.ToString("d"));
                    xw.WriteAttributeString("LineNumber", v._lineNumber.ToString("####0"));
                    xw.WriteAttributeString("Position", v._position.ToString("#0"));
                    xw.WriteAttributeString("Type", SkeletonParserRE.cVarTypes.DictReverseLookup(v._typeCode));
                    xw.WriteAttributeString("TypeCode", v._typeCode.ToString("d"));
                    if (dbgInputLine)
                    {
                        xw.WriteStartElement("inputLine");
                        xw.WriteAttributeString("Text", v._inputTextLine);
                        xw.WriteEndElement(); //Input Line
                    }
                    xw.WriteEndElement(); // _variable
                }
                xw.WriteEndElement(); // Variables
                xw.Close();
            }
            public static void XMLSkelsWriter(List<Skeleton> skelList, Dictionary<string, short> concatenations, string xmlFileName)
            {
                string currentSkel = "";
                int imbedLib = 0;
                XmlTextWriter xw = new XmlTextWriter(xmlFileName, System.Text.Encoding.UTF8);
                xw.Formatting = Formatting.Indented;
                xw.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xw.WriteStartElement("skeletons");
                foreach (SkeletonParser.Skeleton sk in skelList)
                {
                    if (sk._childSkeleton.Contains(sk._amper) || !concatenations.ContainsKey(sk._childSkeleton))
                        imbedLib = -1;
                    else
                        imbedLib = concatenations[sk._childSkeleton];
                    if (sk._skeleton != currentSkel)
                    {
                        if (currentSkel != "")
                            xw.WriteEndElement(); // _skeleton
                        string m = new string('=', sk._skeleton.Length);
                        currentSkel = sk._skeleton;
                        xw.WriteComment("===" + m + "===");
                        xw.WriteComment("== " + sk._skeleton + " ==");
                        xw.WriteComment("===" + m + "===");
                        xw.WriteStartElement("_skeleton");
                        xw.WriteAttributeString("Name", sk._skeleton);
                        xw.WriteAttributeString("Offset", concatenations[sk._skeleton].ToString("d"));
                    }
                    xw.WriteStartElement("imbed");
                    // redundancy: write parent _skeleton name in detail line to make for easier processing
                    //              by programs.
                    xw.WriteAttributeString("Name", sk._childSkeleton);
                    xw.WriteAttributeString("ParentName", sk._skeleton);
                    xw.WriteAttributeString("Offset", imbedLib.ToString("00000"));
                    xw.WriteAttributeString("LineNumber", sk._lineNumber.ToString("####0"));
                    xw.WriteAttributeString("OPT", sk._opt ? "Yes" : "No");
                    xw.WriteAttributeString("NOFT", sk._noft ? "Yes" : "No");
                    xw.WriteAttributeString("EXT", sk._extended ? "Yes" : "No");

                    xw.WriteEndElement(); // imbed
                }
                if (skelList.Count > 0)
                    xw.WriteEndElement(); // last _skeleton processed.
                xw.WriteEndElement(); // _skeleton
                xw.Close();
            }
            public static void XMLDOTsWriter(List<DOT> dotList, string xmlFileName)
            {
                string currentSkel = "";
                XmlTextWriter xw = new XmlTextWriter(xmlFileName, System.Text.Encoding.UTF8);
                xw.Formatting = Formatting.Indented;
                xw.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xw.WriteStartElement("DOTs");
                foreach (SkeletonParser.DOT d in dotList)
                {
                    if (d._skeleton != currentSkel)
                    {
                        if (currentSkel != "")
                            xw.WriteEndElement(); // _skeleton
                        string m = new string('=', d._skeleton.Length);
                        currentSkel = d._skeleton;
                        xw.WriteComment("===" + m + "===");
                        xw.WriteComment("== " + d._skeleton + " ==");
                        xw.WriteComment("===" + m + "===");
                        xw.WriteStartElement("skeleton");
                        xw.WriteAttributeString("Name", d._skeleton);
                    }
                    xw.WriteStartElement("tables");
                    // redundancy: write parent _skeleton name in detail line to make for easier processing
                    //              by programs.
                    xw.WriteAttributeString("Name", d._table);
                    xw.WriteAttributeString("Skeleton", d._skeleton);
                    xw.WriteAttributeString("LineNumber", d._lineNumber.ToString("####0"));

                    xw.WriteEndElement(); // tables
                }
                if (dotList.Count > 0)
                    xw.WriteEndElement(); // last _skeleton processed.
                xw.WriteEndElement(); // DOTS
                xw.Close();
            }
            public static void XMLProgsWriter(List<Program> progList, string xmlFileName,bool dbgInputLine)
            {
                string currentSkel = "";
                XmlTextWriter xw = new XmlTextWriter(xmlFileName, System.Text.Encoding.UTF8);
                xw.Formatting = Formatting.Indented;
                xw.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xw.WriteStartElement("Programs");
                foreach (SkeletonParser.Program p in progList)
                {
                    if (p._skeleton != currentSkel)
                    {
                        if (currentSkel != "")
                            xw.WriteEndElement(); // _skeleton
                        string m = new string('=', p._skeleton.Length);
                        currentSkel = p._skeleton;
                        xw.WriteComment("===" + m + "===");
                        xw.WriteComment("== " + p._skeleton + " ==");
                        xw.WriteComment("===" + m + "===");
                        xw.WriteStartElement("skeleton");
                        xw.WriteAttributeString("Name", p._skeleton);
                    }
                    xw.WriteStartElement("programs");
                    // redundancy: write parent _skeleton name in detail line to make for easier processing
                    //              by programs.
                    xw.WriteAttributeString("Name", p._program);
                    xw.WriteAttributeString("Skeleton", p._skeleton);
                    xw.WriteAttributeString("LineNumber", p._lineNumber.ToString("####0")); 
                    if (dbgInputLine)
                    {
                        xw.WriteStartElement("inputLine");
                        xw.WriteAttributeString("Text", p._inputTextLine);
                        xw.WriteEndElement(); //Input Line
                    }


                    xw.WriteEndElement(); // programs
                }
                if (progList.Count > 0)
                    xw.WriteEndElement(); // last _skeleton processed.
                xw.WriteEndElement(); // DOTS
                xw.Close();
            }
            private void XMLFunctionsWriter(List<Built_in> built_insList, string xmlFileName)
            {
                string currentSkel = "";
                XmlTextWriter xw = new XmlTextWriter(xmlFileName, System.Text.Encoding.UTF8);
                xw.Formatting = Formatting.Indented;
                xw.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xw.WriteStartElement("functionUsage");
                foreach (SkeletonParser.Built_in bi in built_insList)
                {
                    if (bi._skeleton != currentSkel)
                    {
                        if (currentSkel != "")
                            xw.WriteEndElement(); // _skeleton
                        string m = new string('=', bi._skeleton.Length);
                        currentSkel = bi._skeleton;
                        xw.WriteComment("===" + m + "===");
                        xw.WriteComment("== " + bi._skeleton + " ==");
                        xw.WriteComment("===" + m + "===");
                        xw.WriteStartElement("_skeleton"); xw.WriteAttributeString("Name", bi._skeleton);
                    }
                    xw.WriteStartElement("_function");
                    // redundancy: write _skeleton name in _function elements for easier processing by programs.
                    xw.WriteAttributeString("name", bi._function);
                    xw.WriteAttributeString("_skeleton", bi._skeleton);
                    xw.WriteAttributeString("_argument", bi._argument);
                    xw.WriteAttributeString("_lineNumber", bi._lineNumber.ToString("####0"));
                    xw.WriteEndElement(); // _function
                }
                if (built_insList.Count > 0)
                    xw.WriteEndElement(); // last _skeleton processed.
                xw.WriteEndElement(); // FunctionUsage
                xw.Close();
            }
        }

        private class SkeletonParserFixedFileWriter
        {
            private string
                cVariablesFilePath = null,
                cSkeletonsFilePath = null,
                cDOTsFilePath = null,
                cProgsFilePath = null,
                cFunctionsFilePath = null,
                cFieldsOffsetFile = null,
                cLibrariesFilePath = null,
                cVarTypesFilePath = null,
                cKeywordsFilePath = null;


            /// <param name="vars">A List<Variable> of the _skeleton-variables cross reference data</param>
            /// <param name="skels">a List<Skeleton> of the _skeleton-_skeleton cross reference data</param>
            /// <param name="funcs">A List<Built_in> of the _skeleton-ISPF _commandCode cross reference data</param>
            /// <param name="filesHighLevelQualifier">The high level qualifier for the four output files. To this will be appended:
            /// .Variables.txt, ._skeleton.txt, .Functions.txt and .FieldOffsets.txt
            /// </param>
            public SkeletonParserFixedFileWriter(PrintManifest pm, string filesHighLevelQualifier)
            {
                cVariablesFilePath = filesHighLevelQualifier + SkeletonParser.varDSN + ".txt";
                cSkeletonsFilePath = filesHighLevelQualifier + SkeletonParser.skelDSN + ".txt";
                cDOTsFilePath = filesHighLevelQualifier + SkeletonParser.dotDSN + ".txt";
                cProgsFilePath = filesHighLevelQualifier + SkeletonParser.progDSN + ".txt";
                cFunctionsFilePath = filesHighLevelQualifier + SkeletonParser.funcDSN + ".txt";
                cLibrariesFilePath = filesHighLevelQualifier + SkeletonParser.libDSN + ".txt";
                cKeywordsFilePath = filesHighLevelQualifier + SkeletonParser.kwDSN + ".txt";
                cVarTypesFilePath = filesHighLevelQualifier + SkeletonParser.vtDSN + ".txt";
                cFieldsOffsetFile = filesHighLevelQualifier + ".FieldOffsets.txt";
                WriteFiles(pm);
            }

            private void WriteFiles(PrintManifest pm)
            {
                StreamWriter sw;
                int offset;
                SkeletonParserDS ds = new SkeletonParserDS();

                try
                {
                    string imbedLib = "";
                    #region Write fixed format _skeleton/variables cross reference data
                    sw = new StreamWriter(cVariablesFilePath);
                    foreach (SkeletonParser.Variable v in pm._varList)
                    {
                        sw.WriteLine(
                            ColumnPad(pm._configurationName, ds.Variables.ConfigurationColumn) +
                            ColumnPad(pm._configurationNumber, ds.Variables.ConfigNumColumn) +
                            ColumnPad(v._variable, ds.Variables.VariableColumn) +
                            ColumnPad(v._skeleton, ds.Variables.SkeletonColumn) +
                            ColumnPad(v._lineNumber, ds.Variables.LineNumberColumn) +
                            ColumnPad(v._position, ds.Variables.PositionColumn) +
                            ColumnPad(SkeletonParserRE.cKeywords.DictReverseLookup(v._commandCode),
                                ds.Variables.CommandColumn) +
                            ColumnPad(v._commandCode, ds.Variables.CommandCodeColumn) +
                            ColumnPad(SkeletonParserRE.cVarTypes.DictReverseLookup(v._typeCode),
                                ds.Variables.TypeColumn) +
                            ColumnPad(v._typeCode, ds.Variables.TypeCodeColumn)
                        );
                    }
                    sw.Close();
                    #endregion
                    #region Write fixed format library lookup table data
                    sw = new StreamWriter(cLibrariesFilePath);

                    sw.WriteLine(
                            ColumnPad(pm._configurationName, ds.Libraries.ConfigurationColumn) +
                            ColumnPad(pm._configurationNumber, ds.Libraries.ConfigNumColumn) +
                            "-0001" +
                            ColumnPad("?", ds.Libraries.UserTagColumn) +
                            ColumnPad("Unknown", ds.Libraries.HostFileNameColumn) +
                            ColumnPad("?", ds.Libraries.PCFileNameColumn)
                    );

                    int ord = 0;
                    foreach (Library l in pm._libList)
                    {
                        sw.WriteLine(
                            ColumnPad(pm._configurationName, ds.Libraries.ConfigurationColumn) +
                            ColumnPad(pm._configurationNumber, ds.Libraries.ConfigNumColumn) +
                            ColumnPad(ord++, ds.Libraries.OffsetColumn) +
                            ColumnPad(l.Tag, ds.Libraries.UserTagColumn) +
                            ColumnPad(l.HostPDS, ds.Libraries.HostFileNameColumn) +
                            ColumnPad(l.Folder, ds.Libraries.PCFileNameColumn)
                        );
                    }
                    sw.Close();
                    #endregion
                    #region Write fixed format _skeleton/imbedded _skeleton cross reference data
                    sw = new StreamWriter(cSkeletonsFilePath);
                    foreach (SkeletonParser.Skeleton sk in pm._skelList)
                    {
                        if (sk._childSkeleton.Contains(sk._amper) || !pm._concatenations.ContainsKey(sk._childSkeleton))
                            imbedLib = "-0001";
                        else
                            imbedLib = ColumnPad(pm._concatenations[sk._childSkeleton], ds.Skeletons.ChildSkelOffsetColumn);

                        sw.WriteLine(
                            ColumnPad(pm._configurationName, ds.Skeletons.ConfigurationColumn) +
                            ColumnPad(pm._configurationNumber, ds.Skeletons.ConfigNumColumn) +
                            ColumnPad(sk._skeleton, ds.Skeletons.SkeletonColumn) +
                            ColumnPad(pm._concatenations[sk._skeleton], ds.Skeletons.SkelOffsetColumn) +
                            ColumnPad(sk._childSkeleton, ds.Skeletons.ChildSkeletonColumn) +
                            imbedLib +
                            ColumnPad(sk._lineNumber, ds.Skeletons.LineNumberColumn) +
                            (sk._opt ? "Y" : "N") +
                            (sk._noft ? "Y" : "N") +
                            (sk._extended ? "Y" : "N")
                        );
                    }
                    sw.Close();
                    #endregion
                    #region Write fixed format _skeleton/ISPF _function _commandCode cross reference data
                    sw = new StreamWriter(cFunctionsFilePath);
                    foreach (SkeletonParser.Built_in bi in pm._builtInsList)
                    {
                        sw.WriteLine(
                            ColumnPad(pm._configurationName, ds.Functions.ConfigurationColumn) +
                            ColumnPad(pm._configurationNumber, ds.Functions.ConfigNumColumn) +
                            ColumnPad(bi._function, ds.Functions.FunctionColumn) +
                            ColumnPad(bi._skeleton, ds.Functions.SkeletonColumn) +
                            ColumnPad(bi._lineNumber, ds.Functions.LineNumberColumn) +
                            ColumnPad(bi._position, ds.Functions.PositionColumn) +
                            ColumnPad(bi._argument, ds.Functions.ArgumentColumn)
                        );
                    }
                    sw.Close();
                    #endregion
                    #region Write fixed format _skeleton/)DOT cross reference data
                    sw = new StreamWriter(cDOTsFilePath);
                    foreach (SkeletonParser.DOT d in pm._dotList)
                    {
                        sw.WriteLine(
                            ColumnPad(pm._configurationName, ds.Functions.ConfigurationColumn) +
                            ColumnPad(pm._configurationNumber, ds.Functions.ConfigNumColumn) +
                            ColumnPad(d._table, ds.DOT.TableNameColumn) +
                            ColumnPad(d._skeleton, ds.DOT.SkeletonColumn) +
                            ColumnPad(d._lineNumber, ds.DOT.LineNumberColumn)
                        );
                    }
                    sw.Close();
                    #endregion
                    #region Write fixed format _skeleton/called program cross reference data
                    sw = new StreamWriter(cProgsFilePath);
                    foreach (SkeletonParser.Program p in pm._progList)
                    {
                        sw.WriteLine(
                            ColumnPad(pm._configurationName, ds.Functions.ConfigurationColumn) +
                            ColumnPad(pm._configurationNumber, ds.Functions.ConfigNumColumn) +
                            ColumnPad(p._program, ds.DOT.TableNameColumn) +
                            ColumnPad(p._skeleton, ds.DOT.SkeletonColumn) +
                            ColumnPad(p._lineNumber, ds.DOT.LineNumberColumn)
                        );
                    }
                    sw.Close();
                    #endregion
                    #region Write fixed format lookup tables for keywords and _variable types
                    sw = new StreamWriter(cKeywordsFilePath);
                    foreach (KeyValuePair<string, short> kvp in SkeletonParserRE.cKeywords)
                    {
                        sw.WriteLine(
                            ColumnPad(kvp.Value, ds.Keywords.CommandCodeColumn) +
                            ColumnPad(kvp.Key, ds.Keywords.CommandColumn)
                        );
                    }
                    sw.Close();
                    sw = new StreamWriter(cVarTypesFilePath);
                    foreach (KeyValuePair<string, short> kvp in SkeletonParserRE.cVarTypes)
                    {
                        sw.WriteLine(
                            ColumnPad(kvp.Value, ds.VarTypes.TypeCodeColumn) +
                            ColumnPad(kvp.Key, ds.VarTypes.TypeColumn)
                        );
                    }
                    sw.Close();
                    #endregion
                    #region Write the field-name/field-offset reference data
                    sw = new StreamWriter(cFieldsOffsetFile);

                    const string re = @"System\.(?<type>.+)";
                    int dispL = 0, storL = 0;
                    foreach (DataTable d in ds.Tables)
                    {
                        string result;
                        Match m;
                        offset = 1;
                        sw.WriteLine("Table: " + d.TableName);
                        sw.WriteLine("....+....0....+....0....+....0....+....0....+....0....+....0....+....0....+....0");
                        foreach (DataColumn c in d.Columns)
                        {
                            m = Regex.Match(c.DataType.ToString(), re);
                            result = m.Groups["type"].Value.ToString();
                            if (c.MaxLength == -1)
                            {
                                if (result == "Int16")
                                {
                                    dispL = 5;
                                    storL = 2;
                                }
                                else if (result == "Int32")
                                {
                                    dispL = 6;
                                    storL = 4;
                                }
                                else if (result == "Boolean")
                                {
                                    dispL = 1;
                                    storL = 1;
                                }
                            }
                            else
                            {
                                dispL = c.MaxLength;
                                storL = dispL;
                            }
                            sw.WriteLine(
                                c.ColumnName.PadRight(32, ' ').Substring(0, 32) + " " +
                                offset.ToString().PadLeft(3, '0') + " " +
                                dispL.ToString().PadLeft(3, '0') + " " +
                                storL.ToString().PadLeft(3, '0') + " " +
                                result.PadRight(15, ' ') + " " +
                                c.Unique.ToString().PadRight(5, ' ')
                                );
                            offset += dispL;
                        }
                    }

                    sw.Close();
                    #endregion
                }
                catch (DirectoryNotFoundException d)
                {
                    throw new DirectoryNotFoundException(
                        "SkeletonParserFixedFileWriter\n" +
                        "WriteFiles\n" +
                        "The directory information in the output file path specified in your XML parameter file(" +
                            d.Message + " is not valid.");

                }
            }
            private string ColumnPad(string s, DataColumn c)
            {
                return s.PadRight(c.MaxLength, ' ').Substring(0, c.MaxLength);
            }
            private string ColumnPad(int i, DataColumn c)
            {
                return i.ToString("d").PadLeft(GetPad(c), '0');
            }
            private string ColumnPad(short i, DataColumn c)
            {
                return i.ToString("d").PadLeft(GetPad(c), '0');
            }

            /// <summary>
            /// Passed a column name, determine the display length for that column. 
            /// Int16: 5 characters is sufficient
            /// Int32: only the line number is Int32. Allow up to 6 display characters. No skel should have
            /// over 100,000 lines. But this allows up to 999,999 lines.
            /// Boolean: Y or N is stored, so use 1 byte
            /// String: Use the actual defined MaxLength in the DataColumn definition.
            /// </summary>
            /// <param name="c">A DataColumn object.</param>
            /// <returns></returns>
            private int GetPad(DataColumn c)
            {
                int rv = 0;
                string dt;
                switch (c.MaxLength)
                {
                    case -1:
                        dt = c.DataType.ToString();
                        rv = -1;
                        if (dt == "System.Int16")
                            rv = 5;
                        else if (dt == "System.Int32")
                            rv = 6;
                        else if (dt == "System.Boolean")
                            rv = 1;
                        break;
                    default:
                        rv = c.MaxLength;
                        break;
                }
                return rv;
            }
        }
    }
}
