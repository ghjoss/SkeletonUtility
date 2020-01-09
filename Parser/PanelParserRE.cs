namespace ISPFSkeletonParser
{
    public static class PanelParserRE
    {
        public const string ZVARS1 =
            @"\.ZVARS\s*=\s*(?:'\()?(?<zvars1>(?:[A-Za-z#@_$][A-Za-z#@_$0-9]{1,7}\s*,?\s*)+)(?:'\))?(?<plus>\+$)?";
        public const string NAT_CHARS = "#$@_";
        public const string VAR_NAME =
            @"[A-Z" + NAT_CHARS + "]{1}[A-Z" + NAT_CHARS + @"0-9]{0,7}";
        public const string REXX_VARS =
            @"\*REXX\s*" +
            @"(?:" +
                @"\(" +
                    @"(?<vars>" +
                        @"(?<varAll>\*)?" + 
                        @"(?(varAll),)" +
                        @"(?<varList>[^()]+)*" +
                    @")?" +
                    @"(?:" +
                        @"(?(vars),)" +
                        @"\(" +
                            @"(?<rexxProg>.*)" +
                        @"\)" +
                     @")?" +
                @"\)" +
            @")?";

        public const string ASSIGN =
            @"\s*&(?<lval>\S{1,8})\s*=\s*(?<rval>((?:')(?<quoted>.*)(?:')|(?<null>&[zZ])|(?<lit>.*)|(?<var>&?\S*\s*)))";
        public const string IEBU =
            @"^\./ ADD NAME=(?<panelID>.+)\s*";
        public const string ATTR_HDR =
            @"^\)ATTR(?:\s+DEFAULT\((?<deflt>...)\))?";
        public const string ATTR_DEF =
            @"(?m-snx:)^\s*(?<attribute>(?<hexAttr>[0-9A-Fa-f]{2})|(?<charAttr>.))\s*(?:.*(?:TYPE|type|AREA|area)\((?<attrType>\S*)\))";
        public const string BODY_HDR =
            @"^\)BODY(?:.*DEFAULT\((?<deflt>...)\))*";
        // ignored sections: )CCSID, )ABC, )ABCINIT, )ABCPROC, )HELP, )PNTS
        public const string IGNORED_HDRS =
            @"^\)(?:CCSID|ABC.*|HELP|PNTS|FIELD).*";
        public const string AREA_HDR =
            @"^\)AREA.*";
        public const string PANEL_HDR =
            @"^\)PANEL.*";
        public const string FIELD_HDR =
            @"^\)FIELD.*";
        public const string LIST_HDR =
            @"^\)LIST.*";
        public const string MODEL_HDR =
            @"^\)MODEL.*";
        public const string PROC_HDR =
            @"^\)PROC.*";
        public const string INIT_HDR =
            @"^\)INIT.*";
        public const string REINIT_HDR =
            @"^\)REINIT.*";
        public const string END_HDR =
            @"^\)END.*";
        public enum section
        {
            NULL,
            IGNORE,
            CCSID,
            PANEL,
            ATTR,
            ABC,
            ABCINIT,
            ABCPROC,
            BODY,
            MODEL,
            AREA,
            INIT,
            REINIT,
            PROC,
            FIELD,
            HELP,
            LIST,
            PNTS,
            END,
            REXXARG
        }
        public struct SectionRE
        {
            public string _sectionExpression;
            public section _section;

            public SectionRE(string str, section sect)
            {
                _sectionExpression = str;
                _section = sect;
            }
        }
        static public SectionRE[] sreArr = new SectionRE[] {
            new SectionRE(IGNORED_HDRS, section.IGNORE),
            new SectionRE(PANEL_HDR, section.PANEL),
            new SectionRE(ATTR_HDR, section.ATTR),
            new SectionRE(BODY_HDR,section.BODY), // **must** follow HDR1 & HDR2
            new SectionRE(MODEL_HDR, section.MODEL),
            new SectionRE(AREA_HDR, section.AREA),
            new SectionRE(INIT_HDR, section.INIT),
            new SectionRE(REINIT_HDR, section.REINIT),
            new SectionRE(PROC_HDR, section.PROC),
            new SectionRE(LIST_HDR, section.LIST),
            new SectionRE(END_HDR, section.END)
        };


    }
}
