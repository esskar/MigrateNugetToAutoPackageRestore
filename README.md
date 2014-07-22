# MigrateNugetToAutoPackageRestore

Migrating MSBuild-Integrated solutions to use Automatic Package Restore

## Building the source

    git clone https://github.com/esskar/MigrateNugetToAutoPackageRestore.git

After cloning the repository, run 

    build.cmd

__NOTE__: Opening the solution requires VS 2012.

## Performing the migration

### Close down Visual Studio
If the solution you are trying to migrate is open in Visual Studio, then changes may be lost. Visual Studio may overwrite/ignore your changes in some cases and the NuGet extension will also try to re-enable Package Restore when it sees some projects in the solution are missing it. There is also a chance of getting access-denied-exception, so closing visual studio should be you first thing to do.

### Running this tool
Open a command line (`cmd.exe`) and change the directory to the build output folder (`target\build\MigrateNugetToAutoPackageRestore`) and run the following command

    MigrateNugetToAutoPackageRestore.exe -f solution/folder/of/project/to/migrate
    
For more details, check the command line output of

    MigrateNugetToAutoPackageRestore.exe -h
    
__WARNING__: Please make a backup of your solution directory to avoid unrevertable damage.

## Help and Support
If you have a feature request, a bug or any other question, just create an [issue][1].

[1]: https://github.com/esskar/MigrateNugetToAutoPackageRestore/issues

## Disclaimer

THERE IS NO WARRANTY FOR THE PROGRAM, TO THE EXTENT PERMITTED BY APPLICABLE LAW. EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR OTHER PARTIES PROVIDE THE PROGRAM “AS IS” WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE PROGRAM IS WITH YOU. SHOULD THE PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY SERVICING, REPAIR OR CORRECTION.
