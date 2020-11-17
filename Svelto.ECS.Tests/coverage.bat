dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./reports/
reportgenerator -reports:./reports/coverage.cobertura.xml -targetdir:./reports/reportviz