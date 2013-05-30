ECHO =================================================
ECHO Copying Leap DLLs to bin dir
ECHO -------------------------------------------------
ECHO Current Dir: %CD%
ECHO Batch Dir: %~dp0

Set BATCHFILE_DIR=%~dp0

IF (%1) EQU (x86) (
  xcopy /Y /Q /D "%BATCHFILE_DIR%\x86\Leap.dll"
  xcopy /Y /Q /D "%BATCHFILE_DIR%\x86\LeapCSharp.dll"
  GOTO :EOF
)

IF (%1) EQU (AnyCPU) (
  xcopy /Y /Q /D "%BATCHFILE_DIR%\x86\Leap.dll"
  xcopy /Y /Q /D "%BATCHFILE_DIR%\x86\LeapCSharp.dll"
  GOTO :EOF
)

IF (%1) EQU (x64) (
  xcopy /Y /Q /D "%BATCHFILE_DIR%\x64\Leap.dll"
  xcopy /Y /Q /D "%BATCHFILE_DIR%\x64\LeapCSharp.dll"
  GOTO :EOF
)

ECHO Unknown platform (%1).  Failed to copy files

ECHO =================================================
