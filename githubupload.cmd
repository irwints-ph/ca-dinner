@ECHO OFF
@REM C:\_MyFiles\_Codes\_Aral2025\_Samples\_CleanArc\_Code\_DinnerApp\Docs\githubupload.cmd 5-d-ErrorOr
IF "%1" == "" GOTO :END
SET CURVID=video%1
SET SRCDIR=C:\_MyFiles\_Codes\_Aral2025\_Samples\_CleanArc\_Code\_Backup\_%CURVID%

if exist %SRCDIR%\ (
  cd %SRCDIR%
  git init
  git remote add origin https://github.com/irwints-ph/ca-dinner.git
  git checkout -b %CURVID%
  git add .

  git commit -m "Initial Commit"
  git push origin %CURVID%

) else (
  echo %SRCDIR% Does not exit
)

:END