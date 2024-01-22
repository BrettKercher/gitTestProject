timestamp=$(date +%s)
echo $timestamp > release_automation_test
git config user.email "brettk@unity3d.com"
git config user.name "Release Automation"
git commit -am "release automation commit"
git push

exit 1