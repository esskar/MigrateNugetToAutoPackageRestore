@echo off
".nuget\NuGet.exe" install -OutputDirectory packages .\packages.config
".nuget\NuGet.exe" restore MigrateNugetToAutoPackageRestore.sln
"packages\Sake.0.2\tools\sake.exe" %*