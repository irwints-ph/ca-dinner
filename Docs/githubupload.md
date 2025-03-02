```bash
git init
git remote add origin https://github.com/irwints-ph/ca-dinner.git
git config user.email "irwints@yahoo.com"
git config user.name "Erwin Santos"

git add .
git commit -m "first commit"
git push --set-upstream origin master
```

## github initial create Video 1
```bash
..\_Video1
git init
git remote add origin https://github.com/irwints-ph/ca-dinner.git
git checkout -b video1
git add .

git commit -m "Initial Commit"
git push origin video1
```


## Delete Git Branch
```bash
mkdir video3
cd video3
git init
git remote add origin https://github.com/irwints-ph/ca-dinner.git
git push origin -df video3
cd ..
rd /s/q video3
```