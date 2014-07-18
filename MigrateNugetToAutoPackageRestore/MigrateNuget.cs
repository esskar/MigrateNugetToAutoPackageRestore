using System;
using System.Collections.Generic;
using System.IO;

namespace MigrateNugetToAutoPackageRestore
{
    class MigrateNuget : MigrateBase
    {
        protected override IEnumerable<string> FileSearchPatterns
        {
            get { yield return ".nuget\\"; }
        }

        protected override bool IsSearchAllDirectories
        {
            get { return false; }
        }

        protected override bool IsCreateBackup
        {
            get { return false; }
        }

        protected override void MigrateEntry(string entry, bool isFile)
        {
            if (isFile)
            {
                Console.WriteLine("Deleting file: {0}", entry);
                File.Delete(entry);
            }
            else
            {
                Console.WriteLine("Deleting directory: {0}", entry);
                Directory.Delete(entry);
            }
        }
    }
}
