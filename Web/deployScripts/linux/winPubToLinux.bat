cd ../../
dotnet publish -c Release
scp -r ./bin/Release/netcoreapp3.1/publish root@8.129.118.41:/home/snailweb
pause