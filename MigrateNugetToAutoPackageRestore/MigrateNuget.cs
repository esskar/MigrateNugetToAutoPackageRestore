using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MigrateNugetToAutoPackageRestore
{
    class MigrateNuget : MigrateBase
    {
        private readonly string[] _keepNugetFiles = { "NuGet.exe", "NuGet.Config" };

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
            if (!isFile) 
                return;

            if (_keepNugetFiles.Contains(entry, StringComparer.OrdinalIgnoreCase))
                return;

            Console.WriteLine("Deleting file: {0}", entry);
            File.Delete(entry);
        }
    }
}
