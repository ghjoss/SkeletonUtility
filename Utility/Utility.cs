using System;
using System.Data;
using System.IO;


namespace ISPFSkeletonParser
{
    [Serializable]
    class NoParmException : System.Exception {
        string _message;
        public override string Message { get { return _message; } }
        public NoParmException(string msg) 
        {
            _message = msg;
        }
    }

    class Utility
    {
        static int Main(string[] args)
        {
            bool err = false;
            bool verbose = true;
            int rc = 0;

            XMLParmsFileReader xmlr = null;
            try
            {
                if (args.Length == 0)
                    throw new NoParmException("Utility::Main\n" +
                    "You must specify an XML parameter file on the command line when invoking this program.\n");
                xmlr = new XMLParmsFileReader(args[0], true); // true=batch run, ergo at least one output file type is required
                if (args.Length == 2)
                    if (args[1].ToUpper() == "N")
                        verbose = false;
            }
            catch (NoParmException e)
            {
                err = true;
                Console.WriteLine(e.Message);
            }
            catch (ISPFSkeletonParser.LibraryFileNotFoundError e)
            {
                err = true;
                Console.WriteLine(e.Message);
            }
            catch (ISPFSkeletonParser.LibraryIsNotAFolderError e)
            {
                err = true;
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                err = true;
                Console.WriteLine(
                    "Utility::Main\n\n" + e.Message);
            }
            finally
            {
                if (err)
                {
                    Console.WriteLine("\n\nProgram terminating with error. Please press the <enter> key.");
                    Console.Read();
                }
            }
            if (!err)
            {
                Library[] libs = new Library[xmlr.DirectoryCount];
                for (short i = 0; i < xmlr.DirectoryCount; ++i)
                {
                    short i2 = i; 
                    libs[i] = new Library(++i2, xmlr.DirLabels[i], xmlr.DirList[i], xmlr.DirHosts[i]);
                }

                //try
                //{
                    Build(xmlr.ConfigName, xmlr.ConfigNum, new LibraryConcatenation(libs, xmlr.IgnoreList), xmlr, verbose);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine("Utility\nMain\n"+e.Message);
                //    err = true;
                //}
                //finally
                //{
                //    if (err)
                //    {
                //        Console.WriteLine("\n\nProgram terminating due to error. Please press the <enter> key.");
                //        Console.Read();
                //    }
                //}
            }
            if (err)
                rc = 12;
            else
            {
                if (xmlr.ExcelFile != "")
                    rc = rc + 1;
                if (xmlr.XMLFile != "")
                    rc = rc + 2;
                if (xmlr.FixedFile != "")
                    rc = rc + 4;
            }
            return rc;
        }

        static void Build(string configName, short configNum, LibraryConcatenation lc, XMLParmsFileReader xmlr,
                bool verbose)
        {
            // parse all of the variables and )IM references in the skeletons
            SkeletonParser sp = new SkeletonParser(configName, configNum, lc, xmlr.DebugInputLine, verbose);

            StreamWriter sw = new StreamWriter(xmlr.XMLParmsFileFullPath + ".log");
            if (xmlr.ExcelFile != "")
            {
                sp.ExcelVarsWriter(xmlr.ExcelFile);
                sw.WriteLine("EXCEL " + xmlr.ExcelFile);
            }
            if (xmlr.XMLFile != "")
            {
                sp.XMLVarsWriter(xmlr.XMLFile);
                sw.WriteLine("XML " + xmlr.XMLFile);
            }
            if (xmlr.FixedFile != "")
            {
                sp.FixedVarsWriter(xmlr.FixedFile);
                sw.WriteLine("FIXED " + xmlr.FixedFile);
            }
            
            sw.Close();
            SkeletonParserDSDef.SkeletonParserDS ds = sp.ParserDataSet;
            SkeletonParserDSDef.TransientDS ts = sp.TransientDataSet;
            sp.CreateTables(ref ds, ref ts);
            sp.AddUnreferencedSkeletons(ref ds, lc, xmlr);
            sw = new StreamWriter(xmlr.XMLParmsFileFullPath + ".DBDump.xml");
            ds.WriteXml(sw,XmlWriteMode.WriteSchema);
            sw.Close();

            sw = new StreamWriter(xmlr.XMLParmsFileFullPath + ".TransientDump.xml");
            ts.WriteXml(sw,XmlWriteMode.WriteSchema);
            sw.Close();
            DataSet ds2 = new DataSet("SkelParmsX");
        } 
    }
}
