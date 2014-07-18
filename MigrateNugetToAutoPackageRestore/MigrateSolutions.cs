using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MigrateNugetToAutoPackageRestore
{
    public class MigrateSolutions : MigrateBase
    {
        private bool _isWriting;

        protected override IEnumerable<string> FileSearchPatterns
        {
            get { yield return "*.sln"; }
        }

        protected override bool IsSearchAllDirectories
        {
            get { return false; }
        }

        protected override void MigrateEntry(string entry, bool isFile)
        {
            if (!isFile)
                return;

            Console.WriteLine("Migrating solution file {0} ...\n", entry);

            var tempFile = Path.GetTempFileName();

            _isWriting = true;
            using (var sr = new StreamReader(entry))
            {
                using (var sw = new StreamWriter(tempFile))
                {
                    string line;
                    var lineCount = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lineCount++;

                        if (this.HandleLine(line))
                        {
                            sw.WriteLine(line);
                        }
                        else
                        {
                            Console.WriteLine("Removing line {0}: {1}", lineCount, line);
                        }
                    }
                }
            }

            File.Delete(entry);
            File.Move(tempFile, entry);

            var path = Path.GetDirectoryName(entry);
            if (path == null)
                return;

            var solution = new Solution(entry);            
            var projectQuery = solution.Projects.Where(p
                => p.RelativePath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                || p.RelativePath.EndsWith(".vbproj", StringComparison.OrdinalIgnoreCase));
            foreach (var project in projectQuery)
            {
                var projectFile = Path.Combine(path, project.RelativePath);
                if (File.Exists(projectFile))
                {
                    var one = new MigrateOneProject();
                    one.Migrate(projectFile);
                }                    
            }

            Console.WriteLine("Migrating solution file finished.\n");
        }

        private bool HandleLine(string line)
        {
            if (line.StartsWith("Project", StringComparison.OrdinalIgnoreCase))
            {
                if (line.Contains("\".nuget\"")) // disable writing                
                    _isWriting = false;
            }
            else if (line.StartsWith("EndProject", StringComparison.OrdinalIgnoreCase))
            {
                var retval = _isWriting;
                _isWriting = true;
                return retval;
            }
            return _isWriting;
        }
    }
}
