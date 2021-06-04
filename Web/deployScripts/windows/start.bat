echo the server is starting
set ASPNETCORE_ENVIRONMENT=Production
dotnet web.dll --urls http://*:5000
echo the server is started
pause