IF "%1" == "" GOTO :END
SET CURVID=video%1
cd C:\_MyFiles\_Codes\_Aral2025\_Samples\_CleanArc\_Code\_Backup\_%CURVID%

git init
git remote add origin https://github.com/irwints-ph/ca-dinner.git
git checkout -b %CURVID%
git add .

git commit -m "Initial Commit"
git push origin %CURVID%

:END