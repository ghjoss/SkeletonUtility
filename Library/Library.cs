using System;
using System.IO;

namespace ISPFSkeletonParser
{

    [Serializable]
    public class LibraryFileNotFoundError:SystemException {
        public  LibraryFileNotFoundError(string location, string folder)
        {
            throw new System.Exception("The specified path does not exist: " + folder);
        }
    }

    [Serializable]
    public class LibraryIsNotAFolderError : SystemException
    {
        public LibraryIsNotAFolderError(string location, string folder)
        {
            throw new System.Exception("The specified path is not a directory: " + folder);
        }
    }

    /// <summary>
    /// Library: models a mainframe partitioned data set that is part of a concatenation of PDSs. 
    /// 
    /// A PDS is represented by a folder and the PDS members are modeled by the datasets in that 
    /// folder.
    /// </summary>
    public class Library
    {
        private string cTag;
        /// <summary>
        /// The folder that represents the PDS
        /// </summary>
        private string cFolder;
        /// <summary>
        /// The string representing the host dataset name.
        /// </summary>
        private string cHostPDS;
        /// <summary>
        /// Offset of the dataset in the concatenation.
        /// </summary>
        private short cOffset;

        FileInfo[] cFiles;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="tag"></param>
        /// <param name="folder"></param>
        /// <param name="hostPDS"></param>
 
        public Library(short offset, string tag, string folder, string hostPDS) 
        {
            LibrarySetup(offset, tag, folder, hostPDS);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="folder"></param>
        public Library(short offset, string folder) 
        {
            LibrarySetup(offset, "",folder,"");
        }

        /// <summary>
        /// Constructor helper function
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="tag"></param>
        /// <param name="folder"></param>
        /// <param name="hostPDS"></param>
        private void LibrarySetup(short offset, string tag, string folder, string hostPDS)
        {
            if (!Directory.Exists(folder)) 
                throw new LibraryFileNotFoundError(
                    "Library::LibrarySetup\n",folder);

            FileInfo fi = new FileInfo(folder);
            if ((fi.Attributes & FileAttributes.Directory) == 0) 
                throw new LibraryIsNotAFolderError(
                    "Library::LibrarySetup\n",folder);
            cOffset = offset;
            cFolder = folder;
            DirectoryInfo di = new DirectoryInfo(folder);
            cFiles = di.GetFiles();
            
            cHostPDS = hostPDS;
            if (tag.Trim() == "")
                cTag = di.Name + di.Extension;
            else
                cTag = tag;
        }

        public string Tag 
        {
            get { return cTag; }
            set {cTag = value;}
        }
        public string Folder
        {
            get { return cFolder; }
        }

        public string HostPDS
        {
            get { return cHostPDS; }
            set { cHostPDS = value; }
        }

        public FileInfo[] LibraryFile
        {
            get { return cFiles; }
        }
        public short Offset { get { return cOffset; } }
    }
}
