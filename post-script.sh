echo "======================"
echo "Hello World from post-build"
echo "======================"
ls $PROJECT_DIRECTORY

cat $PROJECT_DIRECTORY/build_manifest_old.json
echo "=============="
cat $PROJECT_DIRECTORY/build_manifest.json
exit 0