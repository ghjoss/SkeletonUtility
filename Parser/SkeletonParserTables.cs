using System.Collections.Generic;
using SkeletonParserDSDef;
using ISPFSkeletonParser;

namespace ISPFSkeletonParser
{
    partial class SkeletonParser
    {
        public void CreateTables(ref SkeletonParserDS ds, ref TransientDS ts)
        {
            #region Variables Table
            foreach (Variable v in this._varList)
            {
                SkeletonParserDS.VariablesRow r = 
                    (SkeletonParserDS.VariablesRow) ds.Variables.NewRow();
                r.Configuration = this._configurationName;
                r.ConfigNum = this._configurationNumber;
                r.Variable = v._variable;
                r.Skeleton = v._skeleton;
                r.LineNumber = v._lineNumber;
                r.Position = v._position;
                r.Command = SkeletonParserRE.cKeywords.DictReverseLookup(v._commandCode);
                r.CommandCode = v._commandCode;
                r.Type = SkeletonParserRE.cVarTypes.DictReverseLookup(v._typeCode);
                r.TypeCode = v._typeCode;
                ds.Variables.AddVariablesRow(r);
            }
            #endregion
            #region Skeletons Table
            foreach (Skeleton sk in this._skelList)
            {
                SkeletonParserDS.SkeletonsRow r =
                    (SkeletonParserDS.SkeletonsRow) ds.Skeletons.NewRow();
                r.Configuration = this._configurationName;
                r.ConfigNum = this._configurationNumber;
                r.Skeleton = sk._skeleton;
                r.SkelOffset = this._libCat.MemberListDict[sk._skeleton].Offset;
                r.ChildSkeleton = sk._childSkeleton;
                try
                {
                    r.ChildSkelOffset = this._libCat.MemberListDict[sk._childSkeleton].Offset;
                }
                catch (KeyNotFoundException e)
                {
                    r.ChildSkelOffset = ((short)-1);
                }
                r.Amper = sk._amper;
                r.LineNumber = sk._lineNumber;
                r.OPT = sk._opt;
                r.NOFT = sk._noft;
                r.EXT = sk._extended;
                ds.Skeletons.AddSkeletonsRow(r);
            }
            #endregion
            #region )DOT Table
            foreach (DOT d in this._dotList)
            {
                SkeletonParserDS.DOTRow r =
                    (SkeletonParserDS.DOTRow)ds.DOT.NewRow();
                r.Configuration = this._configurationName;
                r.ConfigNum = this._configurationNumber;
                r.Skeleton = d._skeleton;
                r.TableName = d._table;
                r.LineNumber = d._lineNumber;
                ds.DOT.AddDOTRow(r);
            }
            #endregion
            #region Called Programs Table
            foreach (Program p in this._programList)
            {
                SkeletonParserDS.ProgramsRow r =
                    (SkeletonParserDS.ProgramsRow)ds.Programs.NewRow();
                r.Configuration = this._configurationName;
                r.ConfigNum = this._configurationNumber;
                r.Skeleton = p._skeleton;
                r.ProgramName = p._program;
                r.LineNumber = p._lineNumber;
                ds.Programs.AddProgramsRow(r);
            }
            #endregion
            #region Functions Table
            foreach (Built_in bi in this._functionList)
            {
                SkeletonParserDS.FunctionsRow r = 
                    (SkeletonParserDS.FunctionsRow) ds.Functions.NewRow();
                r.Configuration = this._configurationName;
                r.ConfigNum = this._configurationNumber;
                r.Function = bi._function;
                r.Skeleton = bi._skeleton;
                r.LineNumber = bi._lineNumber;
                r.Position = bi._position;
                r.Argument = bi._argument;
                ds.Functions.AddFunctionsRow(r);
            }
            #endregion
            #region Libraries Table

            foreach (Library l in this._libCat.Concatenation)
            {
                SkeletonParserDS.LibrariesRow r =
                    (SkeletonParserDS.LibrariesRow) ds.Libraries.NewRow();
                r.Configuration = this._configurationName;
                r.ConfigNum = this._configurationNumber;
                r.Offset = l.Offset;
                r.UserTag = l.Tag;
                r.HostFileName = l.HostPDS;
                r.PCFileName = l.Folder;
                ds.Libraries.AddLibrariesRow(r);
            }
            #endregion
            #region Configurations Table
            {
                SkeletonParserDS.ConfigurationsRow r = 
                    (SkeletonParserDS.ConfigurationsRow) ds.Configurations.NewRow();
                r.ConfigNum = this._configurationNumber;
                r.Configuration = this._configurationName;
                ds.Configurations.AddConfigurationsRow(r);
            }
            #endregion
            #region Keywords Table
            foreach (KeyValuePair<string, short> kvp in SkeletonParserRE.cKeywords)
            {
                SkeletonParserDS.KeywordsRow r = 
                    (SkeletonParserDS.KeywordsRow) ds.Keywords.NewRow();
                r.CommandCode = kvp.Value;
                r.Command = kvp.Key;
                ds.Keywords.AddKeywordsRow(r);
            }
            #endregion
            #region VarTypes Table
            foreach (KeyValuePair<string, short> kvp in SkeletonParserRE.cVarTypes)
            {
                
                SkeletonParserDS.VarTypesRow r = 
                    (SkeletonParserDS.VarTypesRow) ds.VarTypes.NewRow();
                r.TypeCode = kvp.Value;
                r.Type = kvp.Key;
                ds.VarTypes.AddVarTypesRow(r);
            }
            #endregion
            #region Transient Tables
            ts = _transientDataSet;
            #endregion
            return;
        }
        public void AddUnreferencedSkeletons(ref SkeletonParserDS ds, LibraryConcatenation lc,
                ISPFSkeletonParser.XMLParmsFileReader xmlr)
        {
            foreach (UnimbeddedSkeletons us in xmlr.UnreferencedList)
            {
                SkeletonParserDS.SkeletonsRow r =
                    (SkeletonParserDS.SkeletonsRow)ds.Skeletons.NewRow();
                r.Configuration = xmlr.ConfigName;
                r.ConfigNum = xmlr.ConfigNum;
                r.Skeleton = us.Parent.PadRight(40).Substring(0,40).Trim();
                r.SkelOffset = 0;
                r.ChildSkeleton = us.Skeleton;
                try
                {
                    r.ChildSkelOffset = lc.MemberListDict[us.Skeleton].Offset;
                }
                catch (KeyNotFoundException e)
                {
                    r.ChildSkelOffset = ((short)-1);
                }
                r.Amper = "";
                r.LineNumber = 0;
                r.OPT = us.OPT;
                r.NOFT = us.NOFT;
                r.EXT = us.EXT;
                try
                {
                    ds.Skeletons.AddSkeletonsRow(r);
                }
                catch (System.Data.DataException de)
                {
                }
                catch (System.Exception e)
                {
                }
            }

        }
    }
}
