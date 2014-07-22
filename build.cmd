@echo off
".nuget\NuGet.exe" install -OutputDirectory packages .\packages.config
".nuget\NuGet.exe" restore -OutputDirectory MigrateNugetToAutoPackageRestore\packages .\MigrateNugetToAutoPackageRestore\packages.config
"packages\Sake.0.2\tools\sake.exe" %*