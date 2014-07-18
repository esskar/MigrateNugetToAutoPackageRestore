using System;

namespace MigrateNugetToAutoPackageRestore
{
    class Program
    {
        /*
         * This Program will perform the migration process described here 
         * http://docs.nuget.org/docs/workflows/migrating-to-automatic-package-restore
        */

        static void Main(string[] args)
        {
            Console.WriteLine("WARNING. This program will modify your solution and project files. Please make a backup before.");
            Console.ReadLine();

            var migrate = new Migrate();
            migrate.Start(args[0]);
        }
    }
}
