using System;
using System.IO;
using System.Xml;

namespace MigrateNugetToAutoPackageRestore
{
    class MigrateOneProject
    {
        public void Migrate(string projectFile)
        {
            Console.WriteLine("Migrating project file {0} ...\n", projectFile);

            var xmldoc = new XmlDocument();
            xmldoc.Load(projectFile);

            var root = xmldoc.DocumentElement;
            if (root == null)
            {
                Console.WriteLine("WARNING: Cannot find XML root in {0}.", projectFile);
                return;
            }

            if (root.Name != "Project")
            {
                Console.WriteLine("WARNING: XML root is not 'Project' in {0}.", projectFile);
                return;
            }

            var xnm = new XmlNamespaceManager(new NameTable());
            xnm.AddNamespace("x", root.NamespaceURI);

            // <RestorePackages>true</RestorePackages>
            this.RemoveNodes(root.SelectNodes(@"//x:RestorePackages", xnm));

            // <Import Project="$(SolutionDir)\.nuget\nuget.targets" />
            this.RemoveNodes(root.SelectNodes(@"//x:Import[@Project='$(SolutionDir)\.nuget\nuget.targets']", xnm));

            // <Target Name="EnsureNuGetPackageBuildImports" BeforeTargets="PrepareForBuild">...</Target>
            this.RemoveNodes(root.SelectNodes(@"//x:Target[@Name='EnsureNuGetPackageBuildImports' and @BeforeTargets='PrepareForBuild']", xnm));

            var tempFile = Path.GetTempFileName();
            using (var wrriter = XmlWriter.Create(tempFile))
                xmldoc.WriteContentTo(wrriter);

            File.Delete(projectFile);
            File.Move(tempFile, projectFile);

            Console.WriteLine("Migrating project file finished.\n");
        }

        private void RemoveNodes(XmlNodeList nodes)
        {
            if (nodes == null)
                return;

            foreach (XmlNode node in nodes)
            {
                var parent = node.ParentNode;
                if (parent == null)
                    continue;

                Console.WriteLine("Removing child node {0}", node.OuterXml);
                parent.RemoveChild(node);
            }
        }
    }
}
