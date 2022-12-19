echo "======================"
echo "Hello World from pre-build!"
echo $JAVA_HOME
ls "/Library/Java/JavaVirtualMachines/"
echo "======================"
echo $UCD_HOST
echo $CCD_BINARY_PATH
"$CCD_BINARY_PATH" auth login 928bd2e7a6bea3cb15202688d2d741ef

exit 0