using System;
using System.Collections.Generic;
using System.IO;

namespace MigrateNugetToAutoPackageRestore
{
    public abstract class MigrateBase
    {
        protected abstract IEnumerable<string> FileSearchPatterns { get; }

        protected abstract bool IsSearchAllDirectories { get; }

        protected virtual bool IsCreateBackup
        {
            get { return true; }
        }

        public void Migrate(string path)
        {
            foreach (var searchPattern in this.FileSearchPatterns)
                this.Migrate(path, searchPattern);            
        }

        private void Migrate(string path, string searchPattern)
        {
            if (string.IsNullOrEmpty(searchPattern))
                throw new ArgumentException("FileSearchPattern is invalid.", "searchPattern");

            var searchOption = this.IsSearchAllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            try
            {
                foreach (var file in Directory.EnumerateFiles(path, searchPattern, searchOption))
                {
                    if (this.IsCreateBackup)
                        this.CreateBackup(file);
                    this.MigrateEntry(file, true);
                }
            }
            catch (DirectoryNotFoundException)
            {
                // ignore
            }

            if (searchPattern.EndsWith("\\"))
            {
                var subPath = Path.Combine(path, searchPattern);

                if (File.Exists(subPath))
                    this.MigrateEntry(subPath, true);
                else if (Directory.Exists(subPath))
                    this.MigrateEntry(subPath, false);
            }
        }

        protected abstract void MigrateEntry(string entry, bool isFile);

        private void CreateBackup(string file)
        {
            File.Copy(file, file + ".migrate", true);
        }
    }
}
