@echo off
start cmd /k "cd AngularStandaloneDemo && dotnet run --urls=http://localhost:5000"
start cmd /k "cd ClientApp && ng serve --port 4200 --open"