/* rexx 
**
** This is a sample Regina REXX template for processing the fields offset file from the ISPF skeleton parser
** program. Its output is a set of DB2 CREATE TABLE control cards.
**
*/
/* Each configuration has a separate XML input file. When it is processed successfully a log is written
** with the High Level Qualifier of any output types (XML, CSV, TXT) created. The log file has the
** same name and path as the XML input parameter file, except that the file extension is .LOG instead
** of .XML
*/
parse arg xmlFile
/*
** convert the XML parameter file name to the logfile name and output
** file name.
**   XXX.XML becomes xxx.log and xxxcreate.Txt in the same directory.
*/
logFile = changestr(".xml<",xmlFile"<",".log")
outFile = changestr(".xml<",xmlFile"<","create.txt")
create. = ""
cre0 = 0
load. = ""
lod0 = 0
/*
** find the FIXED file HLQ in the log file. Then set the
** names of the files generated.
*/
do while word(value('lleft',lines(logFile)) lleft,2)>0
  l = linein(logFile)
  parse var l fileType fileHLQ
  fileHLQ = strip(fileHLQ,'B')
  if fileType = "FIXED" then
    do
       fileO = fileHLQ".FieldOffsets.txt"
       leave
    end
end

x = stream(logFile,"C","CLOSE")
/*
** reset the end-of-file pointer to the first record of the output file.
*/
x = lineout(outfile,,1)

fld = ""
lodFld = ""

do while word(value('OffsetLinesLeft',lines(fileO)) offsetLinesLeft,2) > 0
  l = linein(fileO)
  if left(l,1) = "." then iterate /* line ruler: ....+....0....+ */

  parse var l "Table:" tName .
  if tName \= "" then
    do
      /* write the last field from the previously processed table */
      if (fld \= "") then
		do
		 cre0 = cre0+1; create.cre0 = fld")"
		 cre0 = cre0+1; create.cre0 = " IN dbase.tblspace;"
		 lod0 = lod0+1; load.lod0 = lodFld")"
		end
      fld = ""
      cre0 = cre0+1; create.cre0 = "CREATE TABLE "tname" ("
      lod0 = lod0+1; load.lod0 = "LOAD DATA INDDN "tname" REPLACE LOG NO NOCOPYPEND"
      lod0 = lod0+1; load.lod0 = "       WORKDDN(SYSUT1,SORTOUT)"
      lod0 = lod0+1; load.lod0 = "       INTO TABLE owner."tname" ("
      startOff = 1

      iterate
    end

  /* write the previously processed field, with an added comma */
  if fld \= "" then
    do
		cre0 = cre0+1; create.cre0 = fld","
		lod0 = lod0+1; load.lod0 = lodFld","
	end
  /* get the next field's data */ 
  parse var l fName fOffset fDisplay fStorage fType fKey .
  /*
  ** there are four types of data in the tables:
  ** 2 byte integers, 4 byte integers, boolean and character strings.
  ** In the text files, the boolean data is written as Y or N, not
  ** 1/0 or True/False.
  */
  posDef = "POSITION("startOff":"startOff+fDisplay-1")"
  startOff = startOff + fDisplay
  select
    when fType = "Int16" then 
      do
		DB2Type = "SMALLINT"
		LODType = "INT EXTERNAL"
	  end
    when fType = "Int32" then 
	  do
		DB2Type = "INTEGER"
		LODType = "INT EXTERNAL"
	  end
    when fType = "String" then 
      do
		DB2Type = "VARCHAR("fDisplay+0")"
                LODType = "CHAR("fDisplay+0")"
	  end
    when fType = "Boolean" then 
      do
        DB2Type = "CHAR(1)"
        LODType = "CHAR(1)"
      end
    otherwise nop
  end
  fld = "   "strip(fName)" "DB2Type" NOT NULL"
  lodFld = "  "strip(fName)" "posDef" "LODType
end /* do while ... input file processing*/

/* write the last field in the last table with an added ")" */
if fld \= "" then
  do
    cre0 = cre0 + 1; create.cre0 = fld")"
    lod0 = lod0 + 1; load.lod0 = fld")"
  end
/* close the input and output files */

do i = 1 to cre0
  x = lineout(outFile, create.i)
end
do i = 1 to lod0
  x = lineout(outfile, load.i)
end

x = stream(fileO,'C','CLOSE')
x = stream(outFile,'C','CLOSE')

return





