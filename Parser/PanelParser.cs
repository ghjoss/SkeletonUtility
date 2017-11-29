﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ISPFSkeletonParser
{
    public enum AttrType
    {
        NULL,
        INPUT,          // attr type
        OUTPUT,         // attr type
        TEXT,           // attr type
        DATAIN,         // Area type
        DATAOUT,        // Area type
        DYNAMIC,        // Area type
        SCRL,           // Area type
        REFERENCE,      // for & substitution variables
        OTHER           // undefined, incorrect
    }
    public class PanelVariable
    {
        public string _inputTextLine;
        public string _variable;
        public AttrType _typeCode;
        public string _panel;
        public int _lineNumber;
        public short _position;
        public string _panelSection;
    }

    public class PanelParser
    {
        LibraryConcatenation _libCat;
        public LibraryConcatenation LibraryConcatenation
        {
            get { return _libCat; }
            set { _libCat = value; }
        }

        string _defaultAttributes;
        List<PanelVariable> _pvList;
        public class Attribute
        {
            public string _panel;
            public int _lineNo;
            public short _offset;
            public string _attrChar;
            public AttrType _attributeType;
        }

        internal class BodyLine
        {
            public string _text;
            public PanelParserRE.section _section; // BODY or MODEL
            public int _lineNo;

            public BodyLine(string bodyline, PanelParserRE.section bodymodel, int lineno)
            {
                _text = bodyline;
                _section = bodymodel;
                _lineNo = lineno;
            }
        }
        private ArrayList _bodyLines = null;
        private ArrayList _zvars = null;
        private ArrayList _zvarOverrides = null;

        Dictionary<string, AttrType> _attributeLookup  = new Dictionary<string, AttrType>();

        public PanelParser()
        {
            ParsePanel();
        }

        public void ParsePanel()
        {
            _pvList = new List<PanelVariable>();
            string  
                buffer,
         //       bodyRE,
                vari,
                panel="";//,
               // attributesStr;

            Match   m = null;

            char[] buf80 = new char[80];

            Attribute a;
            AttrType aT = AttrType.NULL;

            List<Attribute> attrList = new List<Attribute>();
            int lineNo = 0;

            //short offset;
            StringBuilder attributes = null;
            PanelParserRE.section 
                sect = PanelParserRE.section.IGNORE,
                newSect;

            StreamReader sr = new StreamReader(@"c:\source\rexx\cmnpnls.bin",Encoding.GetEncoding(37));
            
            bool 
                inZvars = false;//,
                //inREXX = false;

            while (sr.Read(buf80,0,80) != 0)
            {
                ++lineNo;
                buffer = (new string(buf80).NoComment().TrimEnd());
                if (buffer.StartsWith("./ ADD"))
                {
                    #region IEBUPDTE control
                    m = Regex.Match(buffer, PanelParserRE.IEBU);
                    if (m.Success)
                    {
                        _bodyLines = null;
                        lineNo = 0;
                        attrList.Clear();
                        panel = m.Groups["panelID"].Value;
                        _defaultAttributes = "%+_";
                        attributes = new StringBuilder();
                        _attributeLookup.Clear();
                        _zvars = new ArrayList();
                    }
                    continue; // get next input stream line.
                    #endregion
                }
                else if (buffer.StartsWith(")"))
                {
                    #region Determine Section Header
                    newSect = PanelParserRE.section.NULL;
                    for (int i = 0; i < PanelParserRE.sreArr.Length; ++i)
                    {
                        m = Regex.Match(buffer, PanelParserRE.sreArr[i]._sectionExpression);
                        if (m.Success)
                        {
                            newSect = PanelParserRE.sreArr[i]._section;
                            break;
                        }
                    }
                    if (newSect != PanelParserRE.section.NULL)
                        sect = newSect;
                    #endregion
                }
                else // data line, i.e. not "./ ADD" or ")xxxx"
                {
                    if (lineNo == 1) // no preceding )xxxx cmds, this must be the first body line
                        sect = PanelParserRE.section.BODY;
                }
                switch (sect)
                {
                    case PanelParserRE.section.IGNORE:
                        break;
                    case PanelParserRE.section.PANEL:
                        break;
                    case PanelParserRE.section.ATTR:
                        #region )Attr
                        if (buffer.StartsWith(")"))
                        {
                            // process defaults, if any
                            string deflt = m.Groups["deflt"].Value;
                            if (deflt != "")
                                _defaultAttributes = deflt;
                        }
                        else
                        {
                            m = Regex.Match(buffer, PanelParserRE.ATTR_DEF, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            if (m.Success)
                            {
                                switch (m.Groups["attrType"].Value)
                                {
                                    case "INPUT":
                                        aT = AttrType.INPUT;
                                        break;
                                    case "OUTPUT":
                                        aT = AttrType.OUTPUT;
                                        break;
                                    case "TEXT":
                                        aT = AttrType.NULL;
                                        break;
                                    case "DATAIN":
                                        aT = AttrType.DATAIN;
                                        break;
                                    case "DATAOUT":
                                        aT = AttrType.DATAOUT;
                                        break;
                                    case "DYNAMIC":
                                        aT = AttrType.DYNAMIC;
                                        break;
                                    case "SCRL":
                                        aT = AttrType.SCRL;
                                        break;
                                    default:
                                        aT = AttrType.OTHER;
                                        break;
                                }
                                if (aT != AttrType.NULL)
                                {
                                    a = new Attribute();
                                    a._attrChar = m.Groups["attribute"].Value;
                                    if (a._attrChar.Length == 2)
                                        a._attrChar = new string((char)Convert.ToInt32(a._attrChar, 16), 1);
                                    a._panel = panel;
                                    a._lineNo = lineNo;
                                    a._offset = 1;
                                    a._attributeType = aT;
                                    attrList.Add(a);
                                    if (!_attributeLookup.ContainsKey(a._attrChar))
                                        _attributeLookup.Add(a._attrChar, a._attributeType);
                                }
                            }
                        }
                        break;
                        #endregion
                    case PanelParserRE.section.BODY:
                        #region )BODY
                        // For the )BODY and )MODEL sections we need to store the lines in
                        // an ArrayList, for processing in the )INIT, )PROC or )REINIT
                        // section. In the )INIT section, we will add any .ZVAR overrides,
                        // as required.
                        // Additionally: the )BODY section header may have a DEFAULT(xxx) 
                        // override. Process this.
                        if (buffer.StartsWith(")"))
                        {
                            m = Regex.Match(buffer, PanelParserRE.BODY_HDR, RegexOptions.IgnoreCase); // )BODY ... DEFAULT(abc)
                            if (m.Success) // process the default attribute overrides, if any.
                            {
                                string deflt = m.Groups["deflt"].Value;
                                if (deflt != "")
                                    _defaultAttributes = deflt;
                            }
                            // process default overrides, either from
                            // )ATTR or )BODY.
                            AddDefaults(_defaultAttributes, attrList, ref attributes, panel);
                        }
                        if (_bodyLines == null)
                            _bodyLines = new ArrayList();

                        _bodyLines.Add(new BodyLine(buffer, PanelParserRE.section.BODY, lineNo));

                        break;
                        #endregion
                    case PanelParserRE.section.MODEL:
                        #region )MODEL
                        if (!buffer.StartsWith(")"))
                        {
                            if (_bodyLines == null)
                                _bodyLines = new ArrayList();

                            _bodyLines.Add(new BodyLine(buffer, PanelParserRE.section.MODEL, lineNo));
                        }
                        break;
                        #endregion
                    case PanelParserRE.section.AREA:
                        #region )AREA
                        break;
                        #endregion
                    case PanelParserRE.section.INIT:
                        #region )INIT
                        if (buffer.StartsWith(")")) // if this is the )INIT line
                        {
                            // get all the variables from the )BODY & )MODEL sections.
                            // .zvars will be unresolved.
                            ProcessBody(attributes, panel);
                            continue;
                        }

                        if (inZvars)
                        {
                            m = Regex.Match(buffer, PanelParserRE.VAR_NAME);
                            while (m.Success)
                            {
                                _zvarOverrides.Add(m.Groups[0].Value);
                                m = m.NextMatch();
                            }
                            if (!buffer.EndsWith("+"))
                            {
                                inZvars = false;
                                ProcessZVars();
                            }
                        }
                        else
                        {
                            m = Regex.Match(buffer, PanelParserRE.ZVARS1);
                            if (m.Success) // this is .ZVARS = ...
                            {
                                _zvarOverrides = new ArrayList();
                                vari = m.Groups["zvars1"].Value;
                                m = Regex.Match(vari, PanelParserRE.VAR_NAME);
                                while (m.Success)
                                {
                                    _zvarOverrides.Add(m.Groups[0].Value);
                                    m = m.NextMatch();
                                }
                                if (!buffer.EndsWith("+"))
                                {
                                    inZvars = false;
                                    ProcessZVars();
                                }
                                else
                                    inZvars = true;
                            }
                            else // this is an )INIT section program line
                            {
                                ProcessInitReinitProc(buffer,panel,lineNo);
                            }
                        }
                        break;
                        #endregion
                    case PanelParserRE.section.REINIT:
                        #region )REINIT
                        // ProcessBody will only have data to work with if
                        // there was no )INIT section. 
                        ProcessBody(attributes, panel);
                        ProcessInitReinitProc(buffer,panel,lineNo);
                        break;
                        #endregion
                    case PanelParserRE.section.PROC:
                        #region )PROC
                        // ProcessBody will only have data to work with if
                        // there was no )INIT or )REINIT section. 
                        ProcessBody(attributes, panel);
                        ProcessInitReinitProc(buffer, panel,lineNo);
                        break;
                        #endregion
                    //case PanelParserRE.section.FIELD:
                    //    #region )FIELD
                    //    break;
                    //    #endregion
                    case PanelParserRE.section.LIST:
                        #region )LIST
                        break;
                        #endregion
                    case PanelParserRE.section.END:
                        #region )END
                        break;
                        #endregion
                    default:
                        break;
                }

            } // --end-- while more input lines
            return;
        }

        /// <summary>
        /// Combine the default attributes (either "%#_" or a set overridden by either the
        /// )ATTR DEFAULT(xxx) or )BODY DEFAULT(xxx)) with the )ATTR section attributes.
        /// </summary>
        /// <param name="dflts"></param>
        /// The program supplied defaults or the panel supplied default overrides for
        /// highlighted text/normal text/input. We only add the third attribute.
        /// <param name="attrList"></param>
        /// The attributes from the )ATTR section.
        /// <param name="attributes"></param>
        /// A ref stringBuilder object. Used when building regular expressions.
        /// <param name="panel"></param>
        /// The panel name
        /// <param name="lineNo"></param>
        /// 
        private void AddDefaults(string dflts, List<Attribute> attrList,
            ref StringBuilder attributes, string panel)
        {
            // process only the input type, the third attribute [2]

            Attribute a = new Attribute();
            a._panel = panel;
            a._lineNo = 0;
            a._offset = 1;
            a._attrChar = dflts.Substring(2,1);
            a._attributeType = AttrType.INPUT;
            attrList.Add(a);
            if (!_attributeLookup.ContainsKey(a._attrChar))
                _attributeLookup.Add(a._attrChar, a._attributeType);
            // allow for substitution variables (&xxx) in the )BODY and )MODEL sections
            // This must be first, otherwise we might have a regexp reserved char ^,],[ or $ at the
            // beginning of the list of characters (if one is defined as an attribute).
            attributes = new StringBuilder("(?<attrChar>[&");
            Debug.Indent();
            Debug.WriteLine("attribute characters in HEX");
            for (int i = 0; i < attrList.Count; ++i)
            {
                Debug.WriteLine(String.Format("{0:X}",(int) attrList[i]._attrChar.ToCharArray()[0]));
                attributes.Append(attrList[i]._attrChar);
            }
            // if the panel contained an attribute "]", add an escape(\) to the RE.
            attributes.Replace("]", @"\]");
            attributes.Append("])");
            Debug.Unindent();
        }

        /// <summary>
        /// For the )BODY and )MODEL sections, we have stored the lines in case
        /// there are "Z" variables that will be overridden in the INIT section.
        /// These lines are processed once the )INIT, )REINIT or )PROC section
        /// is processed. Only the first of these sections process the data.
        /// After that, the _bodyLines ArrayList is set to null.
        /// </summary>
        /// <param name="attributes">
        /// The attributes from the )ATTR section
        /// </param>
        /// <param name="panel">
        /// The panel name currently being processed.
        /// </param>
        private void ProcessBody(StringBuilder attributes, string panel)
        {
            string
                attributesStr,
                bodyRE,
                buffer;
            Match m;
            short offset = 0;
            if (_bodyLines == null)
                return;

            attributesStr = attributes.ToString();
            bodyRE = PanelParserRE.VAR_NAME;
            // if any of the valid dataset name National Characters (#@_$) are in the
            // set of )ATTRs, then remove that National Character from the 
            // variable name parsing regular expression. For example:
            // "#MYVAR" is normally a valid variable name on a panel. But if 
            // "#" is also an attribute character, then "#MYVAR" is not the variable
            // name, rather "MYVAR" is the name. So, remove the "#" from the 
            // regular expression 
            foreach (char c in PanelParserRE.NAT_CHARS)
            {
                int cI = attributesStr.IndexOf(c);
                if (cI >= 0)
                    bodyRE = bodyRE.Replace(PanelParserRE.NAT_CHARS.Substring(PanelParserRE.NAT_CHARS.IndexOf(c), 1), "");
            }

            foreach (BodyLine  bodyLine in _bodyLines)
            {
                offset = 0;
                string variable = "";
                buffer = bodyLine._text.TrimEnd();
                m = Regex.Match(buffer, attributesStr + bodyRE);
                while (m.Success)
                {
                    ++offset;
                    variable = m.Groups[0].Value.ToUpper().Substring(1);
                    PanelVariable pv = new PanelVariable();
                    pv._inputTextLine = buffer;
                    pv._lineNumber = bodyLine._lineNo;
                    pv._variable = variable.ToUpper();
                    pv._position = offset;
                    pv._panel = panel;
                    pv._panelSection = "BODY";
                    string type = m.Groups["attrChar"].Value;
                    pv._typeCode = type == "&" ? AttrType.REFERENCE : _attributeLookup[type];
                    if (variable == "Z")
                        _zvars.Add(pv);
                    else
                        _pvList.Add(pv);
                    m = m.NextMatch();
                }
                
            } // **end** processing lines in )BODY and )MODEL

            // prevent the _bodyLines from being processed in subsequent )REINIT or )PROC sections
            _bodyLines.Clear();
            _bodyLines = null; 

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="panel"></param>
        /// <param name="lineNo"></param>
        private void ProcessInitReinitProc(string buffer, string panel, int lineNo)
        {
            Match m;
            short offset;
            offset = 0;
            string variable = "";
            m = Regex.Match(buffer, "&" + PanelParserRE.VAR_NAME);
            while (m.Success)
            {
                ++offset;
                variable = m.Groups[0].Value.ToUpper().Substring(1);
                PanelVariable pv = new PanelVariable();
                pv._inputTextLine = buffer;
                pv._lineNumber = lineNo;
                pv._variable = variable.ToUpper();
                pv._position = offset;
                pv._panel = panel;
                pv._panelSection = "BODY";
                string type = m.Groups["attrChar"].Value;
                pv._typeCode = AttrType.REFERENCE;
                _pvList.Add(pv);
                m = m.NextMatch();
            }
        }

        /// <summary>
        /// Match ZVARS (in ArrayList _zvars) with their actual variable name overrides
        /// (in ArrayList _zvarOverrides).
        /// Note: unlike the )BODY processing, where the _bodyLines ArrayList is cleared and set
        /// to null, the _zvars ArrayList is left intact. It is permissible to have multiple
        /// .ZVARS statments in an )INIT section. For example:
        /// if (&FROM = PARENT1)
        ///   .ZVARS = '(A, B, C)'
        /// else
        ///   .ZVARS = '(D, E, F)'
        /// </summary>
        private void ProcessZVars()
        {
            int i;
            for (i = 0; i < _zvars.Count; ++i)
                if (_zvarOverrides.Count >= i)
                {
                    ((PanelVariable)_zvars[i])._variable = ((string) _zvarOverrides[i]).ToUpper();
                    _pvList.Add((PanelVariable) _zvars[i]);
                }
        }


        public string EBCDIC_to_ASCII(string s)
        {
         byte[] trantab2 = new byte[256] { 

        //-----------------------------------------------------------------------------------
        //      +0   +1   +2   +3   +4   +5   +6   +7   +8   +9   +a   +bodyLine   +c   +d   +e   +f
        //-----------------------------------------------------------------------------------

              0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0a,0x0b,0x0c,0x0d,0x0e,0x0f,
        // 00
              0x10,0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1a,0x1b,0x1c,0x1d,0x1e,0x1f,
        // 10
              0x20,0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2a,0x2b,0x2c,0x2d,0x2e,0x2f,
        // 20
              0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3a,0x3b,0x3c,0x3d,0x3e,0x3f,
        // 30
	          0x20,0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0x5b,0x2e,0x3c,0x28,0x2b,0x7c,
        // 40   --                                                --   --   --   --   --   --
        //      sp                                                [    .    <    (    +    |
	          0x26,0x51,0x52,0x53,0x54,0x55,0x56,0x57,0x58,0x59,0x21,0x24,0x2a,0x29,0x3b,0x5e,
        // 50   --                                                --   --   --   --   --   --
        //      &                                                 !    $    *    )    ;    ^
	          0x2d,0x2f,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0x7c,0x2c,0x25,0x5f,0x3e,0x3f,
        // 60   --   --                                           --   --   --   --   --   --
        //      -    /                                            |    ,    %    _    >    ?
	          0x70,0x71,0x72,0x73,0x74,0x75,0x76,0x77,0x78,0x60,0x3a,0x23,0x40,0x27,0x3d,0x22,
        // 70                                                --   --   --   --   --   --   --
        //                                                   `    :    #    @    '    =    "

        //------------------------------------------------------------------------------------
        //       +0   +1   +2   +3   +4   +5   +6   +7   +8   +9   +a   +bodyLine   +c   +d   +e   +f 
        //------------------------------------------------------------------------------------

	          0x80,0x61,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0x8a,0x8b,0x8c,0x8d,0x8e,0x8f,
        // 80        --   --   --   --   --   --   --   --   --
        //           a    bodyLine    c    d    e    f    g    h    i
        //
	          0x90,0x6a,0x6b,0x6c,0x6d,0x6e,0x6f,0x70,0x71,0x72,0x9a,0x9b,0x9c,0x9d,0x9e,0x9f,
        // 90        --   --   --   --   --   --   --   --   --
        //           j    k    l    m    n    o    p    q    r
        //
	          0xa0,0x7e,0x73,0x74,0x75,0x76,0x77,0x78,0x79,0x7a,0xaa,0xab,0xac,0xad,0xae,0xaf,
        // a0        --   --   --   --   --   --   --   --   --                  --
        //           ~    s    t    u    v    w    x    y    z                   [
        //
	          0xb0,0xb1,0xb2,0xb3,0xb4,0xb5,0xb6,0xb7,0xb8,0xb9,0xba,0xbb,0xbc,0xbd,0xbe,0xbf,
        // b0                                                                    --
        //                                                                       ]
        //
	          0x7b,0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0xca,0xcb,0xcc,0xcd,0xce,0xcf,
        // c0   --   --   --   --   --   --   --   --   --   --
        //      {    A    B    C    D    E    F    G    H    I
        //
	          0x7d,0x4a,0x4b,0x4c,0x4d,0x4e,0x4f,0x50,0x51,0x52,0xda,0xdb,0xdc,0xdd,0xde,0xdf,
        // d0   --   --   --   --   --   --   --   --   --   --
        //      }    J    K    L    M    N    O    P    Q    R  
        //
	          0x5c,0xe1,0x53,0x54,0x55,0x56,0x57,0x58,0x59,0x5a,0xea,0xeb,0xec,0xed,0xee,0xef,
        // e0   --        --   --   --   --   --   --   --   --
        //      \         S    T    U    V    W    X    Y    Z  
        //
	          0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0xfa,0xfb,0xfc,0xfd,0xfe,0xff};
         // f0   --   --   --   --   --   --   --   --   --   --
         //      0    1    2    3    4    5    6    7    8    9
         //

         //------------------------------------------------------------------------------------
         //       +0   +1   +2   +3   +4   +5   +6   +7   +8   +9   +a   +bodyLine   +c   +d   +e   +f
         //------------------------------------------------------------------------------------
         StringBuilder sb = new StringBuilder();
         foreach (char c in s)
             sb.Append((char) trantab2[(byte) c]);
         return sb.ToString();
        }
    }

    public static class PanelParserExtensions
    {
        /// <summary>
        /// Extension method to remove the comment from a panel line.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NoComment(this string s)
        {
            Match m = Regex.Match(s, @"(?<text>.*)/\*.*");
            if (m.Success)
                return m.Groups["text"].Value.TrimEnd();
            else
                return s;
        }

    }
}