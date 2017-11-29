/* rexx 
**
** This is a sample Regina REXX program for running the ISPF Skeleton Parser and handling its output.
**
*/
/* Each configuration has a separate XML input file. When it is processed successfully a log is written
** with the High Level Qualifier of any output types (XML, CSV, TXT) created. The log file has the
** same name and path as the XML input parameter file, except that the file extension is .LOG instead
** of .XML
*/
parse arg xmlFile
/*
** invoke the .net skeleton parser
*/
address SYSTEM '..\SkeletonParser "'xmlFile'"'
ret = rc
/*
** If the parser trapped an error, it exits with rc=12
*/
if ret = 12 then
  do
    say "Program terminated with errors."
    exit 12
  end
/*
** ret=0: No output files were requested
**     1: CSV
**     2: XML
**     3: CSV & XML
**     4: TXT (fixed format)
**     5: CSV & TXT
**     6: XML & TXT
**     7: CSV & XML & TXT
*/

if ret = 1 | ret = 3 | ret = 5 | ret = 7 then
  do
    say "Program generated a CSV file."
  end
if ret = 2 | ret = 3 | ret = 5 | ret = 7 then
  do
    say "Program generated an XML file."
  end
if ret = 4 | ret = 5 | ret = 6 | ret = 7 then
  do
     say "Program generated a fixed format file."
     /*
     ** convert the XML parameter file name to the logfile name
     */
     logFile = changestr(".xml<",xmlFile"<",".log")
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
             fileV = fileHLQ".Variables.txt"
             fileS = fileHLQ".Skeletons.txt"
             fileK = fileHLQ".Keywords.txt"
             fileF = fileHLQ".Functions.txt"
             fileT = fileHLQ".VarTypes.txt"
             fileO = fileHLQ".FieldOffsets.txt"
             leave
          end
     end
     call processVariables
     call processSkeletons
     call processFunctions
     call processKeywords
     call processVarTypes
  end
return
/*
** process the skeleton-variables cross reference fixed format file
*/
processVariables:
call GetOffsets "Variables"
do while word(value('VariablesLinesLeft',lines(fileV)) VariablesLinesLeft,2) > 0
  l = linein(fileV)

  n = ""
  do i = 1 to offsets.0
      nm = offsets.NAME.i
      if nm = "Configuration" | nm = "CommandCode" | nm = "TypeCode" then iterate /* not interested in these */
      say offsets.NAME.i" => "substr(l,offsets.OFFSET.i,offsets.DISPLAY.i)" Key("offsets.KEY.i") TYPE("offsets.TYPE.i")"
  end
end
x = stream(fileV,'C','CLOSE')
return

/*
** process the skeleton-Skeleton cross reference fixed format file
*/
processSkeletons:
call GetOffsets "Skeletons"
return

/*
** process the skeleton-built-in function cross reference fixed format file
*/
processFunctions:
call GetOffsets "Functions"
return

/*
** process the ISPF commands cross reference fixed format file
*/
processKeywords:
call GetOffsets "Keywords"
return

/*
** process the variable types cross reference fixed format file
*/
processVarTypes:
call GetOffsets "VarTypes"
return

/*  
** get the field offsets for the data in the passed table
*/
GetOffsets:
parse upper arg tableName
drop offsets.
offsets. = ""
o = 0
state = TableNotFound
do while word(value('OffsetLinesLeft',lines(fileO)) offsetLinesLeft,2) > 0
  l = linein(fileO)
  if left(l,1) = "." then iterate
  select
    when state = TableNotFound then
      do
        parse upper var l "TABLE:" tName .
        if tName = tableName then
          state = TableFound
      end
    when state = TableFound then
      do
        parse upper var l "TABLE:" tName .
        if tName \= "" then
          state = TableFinished
        else
          do
            o = O + 1
            parse var l,
              offsets.NAME.o,
              offsets.OFFSET.o,
              offsets.DISPLAY.o,
              offsets.STORAGE.o,
              offsets.TYPE.o,
              offsets.KEY.o,
              .
          end
      end
    when state = TableFinished then
      leave
    otherwise
      nop
   end /* select */
end /* do while ...*/
x = stream(fileO,'C','CLOSE')

offsets.0 = o
return





