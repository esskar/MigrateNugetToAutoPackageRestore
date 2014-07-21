using System;
using System.Collections.Generic;
using System.Reflection;
using CommandLine;
using CommandLine.Text;

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
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options)) 
                return;

            Console.WriteLine("WARNING. This program will modify your solution and project files. Please backup before continue.");
            Console.ReadLine();
           
            var migrate = new Migrate();
            migrate.Start(options.SolutionFolder);
        }

        class Options
        {
            [Option('f', "folder", HelpText = "Folder containing the solution to migrate.", Required = true)]
            public string SolutionFolder { get; set; }

            [HelpOption('h', "help")]
            public string GetUsage()
            {
                var assembly = Assembly.GetEntryAssembly();
                var version = assembly.GetName().Version;
                var help = new HelpText
                {
                    Heading = new HeadingInfo("MigrateNugetToAutoPackageRestore", version.ToString(3)),
                    Copyright = new CopyrightInfo("Sascha Kiefer (esskar)", GetCopyrightYears()),
                    AdditionalNewLineAfterOption = false,
                    AddDashesToOption = true
                };
                help.AddPreOptionsLine(string.Format("Version: {0}", version));
                help.AddPreOptionsLine("All rights reserved.");
                help.AddPreOptionsLine(string.Empty);
                help.AddPreOptionsLine(string.Format("Usage: {0} [options]", ExecutableName));
                help.AddOptions(this);

                return help;
            }

            private static string ExecutableName
            {
                get { return AppDomain.CurrentDomain.FriendlyName.ToLowerInvariant(); }
            }

            private static int[] GetCopyrightYears()
            {
                var year = 2014;
                var now = DateTime.Now;

                var years = new List<int>();
                while (year <= now.Year)
                {
                    years.Add(year++);
                }

                return years.ToArray();
            }
        }

    }    
}
