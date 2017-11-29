using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ISPFSkeletonParser
{

    public class LibraryConcatenation
    {
        private List<Library> cConcatenation = new List<Library>(10);

        public List<Library> Concatenation
        {
            get { return cConcatenation; }
        }

        private List<SkelLibFile> cMemberList = new List<SkelLibFile>(1000);
        private Dictionary<string, Library> cMemberListD = new Dictionary<string, Library>();
        private Dictionary<string, short> cMemberConcatenation = new Dictionary<string, short>();

        public Dictionary<string, short> MemberConcatenation
        {
            get { return cMemberConcatenation; }
            set { cMemberConcatenation = value; }
        }

        private Hashtable cMemberHash = new Hashtable(1000);
        public class SkelLibFile
        {
            public short offset;
            public string fileName;
            public string filePath;
        }

        public class MemberCompare : IComparer<SkelLibFile>
        {
            int IComparer<SkelLibFile>.Compare(SkelLibFile left, SkelLibFile right)
            {
                return left.fileName.CompareTo(right.fileName);
            }
        }
        public LibraryConcatenation(Library[] libs)
        {
            Init(libs, (Hashtable) null);
        }

        public LibraryConcatenation(Library[] libs, Hashtable ignoreList)
        {
            Init(libs, ignoreList);
        }

        private void Init(Library[] libs, Hashtable ignoreList)
        {
            // strip the extension offset the file name
            string sWork = "";
            string[] sArr;
            char[] delims = ".".ToCharArray();
            short ord = 0;
            foreach (Library l in libs)
            {
                ++ord;
                cConcatenation.Add(l);
                foreach (FileInfo fi in l.LibraryFile)
                {
                    sArr = fi.Name.Split(delims);
                    sWork = sArr[0];
                    if (ignoreList != null)
                        if (ignoreList.Contains(sWork))
                            continue;
                    if (!cMemberListD.ContainsKey(sWork))
                    {
                        cMemberListD.Add(sWork,l);
                        SkelLibFile t = new SkelLibFile();
                        t.fileName = sWork;
                        t.filePath = fi.DirectoryName+@"\"+fi.Name;
                        cMemberHash.Add(sWork, t.filePath);
                        cMemberList.Add(t);
                        cMemberConcatenation.Add(sWork, ord);
                    }
                }
                
            }
            MemberCompare mbrComp = new MemberCompare();
            cMemberList.Sort(mbrComp);
        }
        public List<SkelLibFile> MemberList
        {
            get { return cMemberList;} //.Clone(); }
        }

        public Dictionary<string, Library> MemberListDict
        {
            get { return cMemberListD; }
        }

    }
}

