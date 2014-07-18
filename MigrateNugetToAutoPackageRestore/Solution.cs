using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MigrateNugetToAutoPackageRestore
{
    // http://social.msdn.microsoft.com/Forums/en-US/6ac84a8a-6b5b-40d5-adfc-38e978d6cdbe/parsing-a-visual-studio-solution-file-in-the-c-code?forum=csharplanguage
    class Solution
    {
        //internal class SolutionParser
        //Name: Microsoft.Build.Construction.SolutionParser
        //Assembly: Microsoft.Build, Version=4.0.0.0

        static readonly Type _solutionParser;
        static readonly PropertyInfo _solutionReader;
        static readonly MethodInfo _parseSolution;
        static readonly PropertyInfo _projects;

        static Solution()
        {
            _solutionParser = Type.GetType("Microsoft.Build.Construction.SolutionParser, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, false);
            if (_solutionParser == null) 
                return;

            _solutionReader = _solutionParser.GetProperty("SolutionReader", BindingFlags.NonPublic | BindingFlags.Instance);
            _projects = _solutionParser.GetProperty("Projects", BindingFlags.NonPublic | BindingFlags.Instance);
            _parseSolution = _solutionParser.GetMethod("ParseSolution", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public List<SolutionProject> Projects { get; private set; }

        public Solution(string solutionFileName)
        {
            if (_solutionParser == null)
                throw new InvalidOperationException("Can not find type 'Microsoft.Build.Construction.SolutionParser' are you missing a assembly reference to 'Microsoft.Build.dll'?");
            
            var solutionParser = _solutionParser.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).First().Invoke(null);
            using (var streamReader = new StreamReader(solutionFileName))
            {
                _solutionReader.SetValue(solutionParser, streamReader, null);
                _parseSolution.Invoke(solutionParser, null);
            }

            var array = (Array)_projects.GetValue(solutionParser, null);            
            var projects = array.Cast<object>().Select((t, i) => new SolutionProject(array.GetValue(i))).ToList();
            this.Projects = projects;
        }


        public class SolutionProject
        {
            static readonly PropertyInfo _projectName;
            static readonly PropertyInfo _relativePath;
            static readonly PropertyInfo _projectGuid;

            static SolutionProject()
            {
                var solution = Type.GetType("Microsoft.Build.Construction.ProjectInSolution, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, false);
                if (solution == null)
                    return;

                _projectName = solution.GetProperty("ProjectName", BindingFlags.NonPublic | BindingFlags.Instance);
                _relativePath = solution.GetProperty("RelativePath", BindingFlags.NonPublic | BindingFlags.Instance);
                _projectGuid = solution.GetProperty("ProjectGuid", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            public string ProjectName { get; private set; }
            public string RelativePath { get; private set; }
            public string ProjectGuid { get; private set; }

            public SolutionProject(object solutionProject)
            {
                this.ProjectName = _projectName.GetValue(solutionProject, null) as string;
                this.RelativePath = _relativePath.GetValue(solutionProject, null) as string;
                this.ProjectGuid = _projectGuid.GetValue(solutionProject, null) as string;
            }
        }
    }
}
