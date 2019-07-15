print "Switching to 'script' volume...".
switch to "scripts".
print "Switch to 'script volume.'".

print "Compiling 'test.ks'...".
compile "test.ks" to "test.ksm".
print "Compiled 'test.ks'".

print "Running 'test.ksm'...".
runpath("test.ksm").
print "Ran 'test.ksm'".