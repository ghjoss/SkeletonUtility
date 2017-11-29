REM This CMD file executes the skeleton parser program, passing it a hard-coded XML Parameter file
REM Note that the XML file as coded here should be provided without the .XML extension
REM Since this batch file is installed in the SAMPLES directory, it references the program and
REM the XML configuration file one directory up.

@echo off
..\SkeletonParser "..\%1.xml"
