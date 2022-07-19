echo "======================"
echo "Hello World from post-build"
echo "Workspace: $WORKSPACE"
cd $WORKSPACE
echo "Contents of Workspace:"
ls -la
echo "======================"
echo "Param 1: $1"
echo "Param 2: $2"
echo "Param 3: $3"
echo "======================"
exit 0