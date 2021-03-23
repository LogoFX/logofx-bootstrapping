cd ../build
call build.bat
cd ../test
call test-all
cd ../pack
call ./pack.bat
cd ../publish
call ./copy.bat LogoFX.Bootstrapping 2.2.0-rc2 %1