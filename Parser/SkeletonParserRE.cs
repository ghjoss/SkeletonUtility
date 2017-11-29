using System.Collections.Generic;
using System.Linq;


namespace ISPFSkeletonParser
{
    public static class SkeletonParserRE
    {
        #region skeleton parsing Regular expressions
        public const string COMMAND_PARSE = 
            @"^(?:\))" +
            @"(?:"+
                @"(?<Command>SEL|IF|SETF|SET|ELSE|DOT|DO|IM|ENDSEL|ENDDOT|ENDDO|LEAVE|ITERATE|NOP|REXX|ENDREXX|CM|BLANK|TBA|TB|DEFAULT)" +
                @"\s*" +
            @")" +
            @"(?<data>.*)";

        public const string COND_PARSE =
            @"\)(?:(?<sel>SEL)|(?<if>IF(?=\b.*THEN\b))){1}"
            + @"\s+(?<Conditionals>.*)(?(if)THEN\b*)(?<cmd>.*)";
        
        public const string ELSE_PARSE =
            @"(?<else>\)ELSE)\s*(?<ElseCmd>.*)";
        
        public const string VAR_NAME =
            @"[A-Z#$@_]{1}[A-Z#$@0-9]{0,7}";
        
        public const string L_VARIABLE = 
            @"(?<amp>[";
        
        private const string EVSUFFIX = 
            @"(?<postData>.*)";
        
        public const string R_VARIABLE_CONST = 
            "])(?>(?<VariableName>" + VAR_NAME + @"))(?(amp)\.)?" + EVSUFFIX;
        
        public const string R_VARIABLE = 
            "])(?>(?<VariableName>" + VAR_NAME + @"))(?(amp)\.)?" + EVSUFFIX;

        public const string CONSTANT = 
            @"(?<VariableName>[^& ]+)" + EVSUFFIX + "?";
        //
        // EMBEDDED_VARIABLE_CONST looks for a _variable or constant in a string
        // (?<amp>[&])(?>(?<VariableName>[A-Z#$@_]{1}[A-Z#$@_0-9]{0,7}))(?(amp)\.)?(?<postData>.*)
        //
        public const string EMBEDDED_VARIABLE_CONST = L_VARIABLE + "&" + R_VARIABLE_CONST;

        //
        // (?<amp>[&])(?>(?<VariableName>[A-Z#$@_]{1}[A-Z#$@_0-9]{0,7}))(?(amp)\.)?(?<postData>.*)
        //
        public const string EMBEDDED_VARIABLE = L_VARIABLE + "&" + R_VARIABLE;
        
        public const string LOGICAL_EXPRESSION =
            @"(?:\s+(EQ|=|NE|\^=|\<|LT|\<=|LE|\>|GT|\>=|GE)\s+)";
        
        public const string COND_EXPRESSION =
            @"(?<exprL>\S+)" + LOGICAL_EXPRESSION + @"(?<exprR>\S+)";
        
        public const string CONJUNCTION =
            @"(?:\s+(AND|OR|&&|\|\|)\s+)";
        
        public const string COND_EXPRESSION2 =
            CONJUNCTION + @"(?<COND2>\S+" + LOGICAL_EXPRESSION + @"\S+(\s+.+)*)";
        
        public const string COND_PHRASE =
            COND_EXPRESSION + "(" + COND_EXPRESSION2 + ")*";
        
        #region )DO parsing
        //)DO CNT
        //)DO var = n [TO m] [BY incr] [FOR cnt] [WHILE expression] | [UNTIL expression]
        //)DO FOREVER
        public const string DO_PARSE =
            @"\)DO\s+"
            + @"(?:"
            + @"   (?<WhileUntil>WHILE|UNTIL)\s+(?<Conditionals>.+)?"
            + @"   |"
            + @"   (?<LoopClause>"
            + @"     (?:"
            + @"       (?<LoopIter>[&]?\S+)"
            + @"       \s+=\s+"
            + @"       (?<LoopStart>(\d+|[&]?\S+))"
            + @"     ) "
            + @"     (?:"
            + @"       \s+TO\s+(?<LoopEnd>(?:\d+|(?:[&]?\S+)))"
            + @"       (\s+BY\s+(?<LoopInc>(?:\d+|(?:[&]?\S+))))?"
            + @"     )?"
            + @"     (?:\s+FOR\s+(?<LoopCnt>(?:\d+|(?:[&]\S+))))?"
            + @"   )?"
            + @"   |"
            + @"   (?<DoLoopCt>[&]?\S+)?"
            + @"   |"
            + @"   (?:FOREVER)?"
            + @")?"
            ;
        #endregion
        public const string IM_PARSE = @"^(?:\)IM)\s+(?<ImbeddedSkelName>[\S]+)(?<IMModifiers>\s+.*)*";
        public const string IM_PARSE_OPT = @"\s*(?<_opt>OPT)";
        public const string IM_PARSE_EXT = @"\s*(?<ext>(NO)*EXT)";
        public const string IM_PARSE_NT = @"\s*(?<nt>NT)";
        public const string DOT_PARSE = @"(?:\)DOT)\s+(?<TableName>[^\s]+)";
        public const string ARITHMETIC_FUNCTION = @"(?:\s+[+-]{1}\s+)";
        public const string SET_EXPRESSION = @"(?<exprL>\S+)" + "(" + ARITHMETIC_FUNCTION + @"(?<exprR>\S+)" + ")?";
        public const string SET_PARSE = @"(?:(\)SETF|\)SET))\s+(?<VariableName>[^\s]+)\s+=\s+" + SET_EXPRESSION;
        public const string PGM_PARSE = @"^//[^\s^\*]*\s+(?:EXEC\s+){1}[^\s]*PGM=(?<ProgramName>[A-Z@$#&]{1}[A-Z0-9@$#&]*)[,\s].*";
        public const string DEFAULT = @"(?:\)DEFAULT)\s+(?:.{1})(?<ampOver>.{1})(?<continueOver>.{1})(?:.{4})\s*";
        public const string REXX_PARSE = @"(?<=\)REXX\s*)(?:(?<varlist>.*)(?=(?:\s*REXX=[%]?(?<rexxProcedure>\S*)))|(?<varlist>.*))";
        #endregion
        #region Built-in Functions
        public const string BUILT_INS = @"(?<builtin>&(?<function>EVAL|LEFT|LENGTH|RIGHT|STRIP|STR|SUBSTR|VSYM|SYMDEF)\()";
        public const string BUILT_INS2 = BUILT_INS + @"(?<arg>.*?" + @")" + @"\)";
        #endregion
        #region ReferenceTypes Lookup Dictionary
        public const string VARIABLE_REFERENCE = "VARIABLE_REFERENCE";
        public const string CONSTANT_REFERENCE = "CONSTANT_REFERENCE";
        public const string CONSTANT_TEST = "CONSTANT_TEST";
        public const string VARIABLE_TEST = "VARIABLE_TEST";
        public const string LVAL_ASSIGNMENT = "LVAL_ASSIGNMENT";
        public const string LVAL_INDIRECT_ASSIGNMENT = "LVAL_INDIRECT_ASSIGNMENT";
        public const string RVAL_CONSTANT_ASSIGNMENT = "RVAL_CONSTANT_ASSIGNMENT";
        public const string RVAL_VARIABLE_ASSIGNMENT = "RVAL_VARIABLE_ASSIGNMENT";
        public const string LVAL_NULL_ASSIGNMENT = "LVAL_NULL_ASSIGNMENT";
        public const string LVAL_INDIRECT_NULL_ASSIGNMENT = "LVAL_INDIRECT_NULL_ASSIGNMENT";
        public const string REXX_CALL_CONSTANT_REFERENCE = "REXX_CALL_CONSTANT_REFERENCE";
        public const string REXX_CALL_VARIABLE_REFERENCE = "REXX_CALL_VARIABLE_REFERENCE";
        public const string REXX_PROCEDURE = "REXX_PROCEDURE";
        public const string FUNCTION = "FUNCTION";
        public static Dictionary<string, short> cVarTypes = new Dictionary<string, short> 
        {
            {VARIABLE_REFERENCE, 5},
            {CONSTANT_REFERENCE, 10},
            {CONSTANT_TEST, 15},
            {VARIABLE_TEST,20},
            {LVAL_ASSIGNMENT,25},
            {LVAL_INDIRECT_ASSIGNMENT,30},
            {RVAL_CONSTANT_ASSIGNMENT,35}, 
            {RVAL_VARIABLE_ASSIGNMENT,40}, 
            {LVAL_NULL_ASSIGNMENT,45},
            {LVAL_INDIRECT_NULL_ASSIGNMENT,50},
            {REXX_CALL_CONSTANT_REFERENCE,55},
            {REXX_CALL_VARIABLE_REFERENCE,60},
            {REXX_PROCEDURE, 65},
            {FUNCTION, 70}
        };
        #endregion
        #region Keywords lookup dictionary
        public static Dictionary<string, short> cKeywords = new Dictionary<string,short>
        {
           {"SETF",5},
           {"SET",10},
           {"SEL",15},
           {"ENDSEL",20},
           {"DOT",25},
           {"ENDDOT",30},
           {"IM",35},
           {"DO",40},
           {"ENDDO",45},
           {"IF",50},
           {"ELSE",55},
           {"DOWHILE",60},
           {"DOUNTIL",65},
           {"REXX",70},
           {"EVAL",75},
           {"LEFT",80},
           {"LENGTH",85},
           {"RIGHT",90},
           {"STR",95},
           {"STRIP",100},
           {"SUBSTR",105},
           {"VSYM",110},
           {"SYMDEF",115},
           {"DATALINE",120},
           {"OTHER",125},
           {"LEAVE",130},
           {"ITERATE",135}
        };
        #endregion
        public static string DictReverseLookup(this Dictionary<string,short> d, int revVal)
        {
            return (from itm in d
                          where itm.Value == revVal
                          select itm.Key).First<string>();

        }
    }
}
