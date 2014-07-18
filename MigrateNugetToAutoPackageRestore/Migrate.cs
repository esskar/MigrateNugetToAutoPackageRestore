namespace MigrateNugetToAutoPackageRestore
{
    class Migrate
    {
        public void Start(string path)
        {
            this.Start<MigrateSolutions>(path);
            this.Start<MigrateNuget>(path);            
        }

        private void Start<T>(string path) where T : MigrateBase, new()
        {
            var m = new T();
            m.Migrate(path);
        }
    }
}
