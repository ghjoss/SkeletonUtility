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
tblspc = 0

do while word(value('OffsetLinesLeft',lines(fileO)) offsetLinesLeft,2) > 0
  l = linein(fileO)
  if left(l,1) = "." then iterate /* line ruler: ....+....0....+ */

  parse var l "Table:" tName .
  if tName \= "" then
    do
      /* write the last field from the previously processed table */
      if (fld \= "") then
		do
		 call AddCreate fld")"
                 tblspc = tblspc + 1
		 call AddCreate " IN dbase.tblspc"right(tblspc,2,"0")";"
		 call AddLoad lodFld")"
		end
      fld = ""
      call AddCreate "CREATE TABLE "tname" ("
      call AddLoad "LOAD DATA INDDN "translate(left(tname,8))" REPLACE LOG NO NOCOPYPEND"
      call AddLoad "       WORKDDN(SYSUT1,SORTOUT)"
      call AddLoad "       INTO TABLE owner."tname" ("
      startOff = 1

      iterate
    end

  /* write the previously processed field, with an added comma */
  if fld \= "" then
    do
		call AddCreate fld","
		call AddLoad lodFld","
	end
  /* get the next field's data */ 
  parse var l fName fOffset fDisplay fStorage fType fKey .
  /*
  ** there are four types of data in the tables:
  ** 2 byte integers, 4 byte integers, boolean and character strings.
  ** In the text files, the boolean data is written as Y or N, not
  ** 1/0 or True/False.
  */
  posDef = "POSITION("right(startOff,3,"0")":"right(startOff+fDisplay-1,3,"0")")"
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
        fDisplay = right(fDisplay,3,"0")
		DB2Type = "VARCHAR("fDisplay")"
		LODType = "CHAR("fDisplay")"
	  end
    when fType = "Boolean" then 
      do
        DB2Type = "CHAR(001)"
        LODType = "CHAR(001)"
      end  
    otherwise nop
  end
  fName = left(fName,18," ")
  fld = "   "fName||DB2Type" NOT NULL"
  lodFld = "  "fName||posDef" "LODType
end /* do while ... input file processing*/

/* write the last field in the last table with an added ")" */
if fld \= "" then
  do
    call AddCreate fld")"
    tblspc = tblspc + 1
    call AddCreate " IN dbase.tblspc"right(tblspc,2,"0")";"
    call AddLoad fld")"
  end
/* close the input and output files */

do i = 1 to cre0
  x = lineout(outFile, create.i)
end
x = lineout(outfile, "")
do i = 1 to lod0
  x = lineout(outfile, load.i)
end

x = stream(fileO,'C','CLOSE')
x = stream(outFile,'C','CLOSE')

return

AddCreate:
parse arg _fld
cre0 = cre0 + 1
create.cre0 = _fld
return

AddLoad:
parse arg _fld
lod0 = lod0 + 1
load.lod0 = _fld
return

