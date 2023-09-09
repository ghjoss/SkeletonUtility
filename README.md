This is the source code to a Windows based utility that reads mainframe ISPF skeletons and cross-references all skeletons, variables and tables. I originally wrote this in the early 2000's and only distributed the final executable, documentation and installation routines at that time, through a web site that I managed. The utility was free to use, but the source code was not made available.
Around 2009, at the request of a partner, I took the web site down. The site contained the utiltity and some additional software that we were using to compete for z/OS contracts and she deemed a competitive edge for us.
I have since retired and the business grew enough that I convinced her to allow me to make the C# source freely available. Savvy users will note how dated the interface is. Feel free to download and modify the code. I have added an MIT license.

The original version of the program was the simple parser which read the (up to) three skeleton directories and built cross reference files. A GUI user interface was added later. The GUI interface, called SkeletonParserQuery, called the parser "under the covers".

I last successfully compiled and committed all changes using Visual Studio 2019. When I tried to load the solution in later versions of VS, the installation project generated errors. I have modified the installation to work with Visual Studio 2022.
Suggestions for improvement:
- Update the interface to something more modern. It is dated and really looks so.
- The current utility requires the parsed skeleton files to be downloaded to a local machine. It would be better to be able to access the files (the utility is READ ONLY for the skeletons) directly on the mainframe. That way any changes to the skeletons would not require downloading them again.

Please see the ISPFSkeletonParser.PDF file in the project directory. The MS Word .DOCX file and an OpenOffice .ODF file are also in this directory.
