echo "======================"
echo "Hello World from pre-build!"
echo $JAVA_HOME
ls "/Library/Java/JavaVirtualMachines/"
echo "======================"
echo $UCD_HOST
echo $CCD_BINARY_PATH

"$CCD_BINARY_PATH" auth login 928bd2e7a6bea3cb15202688d2d741ef
"$CCD_BINARY_PATH" config set environment 3d150651-5554-42a2-8c2e-047291db0755 --project=610bae4f-6222-486f-b353-f3b2d12370ec
"$CCD_BINARY_PATH" config set bucket 5175b501-4d96-4064-83be-205da2e44dc4
"$CCD_BINARY_PATH" entries info "addressables_content_state.bin" 2>&1

exit 0