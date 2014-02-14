using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using System.Reflection;
using System.IO;

namespace Gatsby
{
    public class PluginCompiler 
    {
        class AssemblyData
        {
            public Assembly Assembly;
            public string Path;
        }

        List<AssemblyData> assemblies;

        public PluginCompiler()
        {
            this.assemblies = new List<AssemblyData>();
        }

        public void RegisterAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += this.AppDomain_AssemblyResolve;
        }

        public void UnregisterAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= this.AppDomain_AssemblyResolve;
        }

        private Assembly AppDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            foreach (AssemblyData assemblyData in this.assemblies)
            {
                if (e.Name == assemblyData.Assembly.FullName)
                    return assemblyData.Assembly;
            }
             
            return null;
        }

        public void Compile(SourceFilePath path, string outputPath)
        {
            SyntaxTree syntaxTree = SyntaxTree.ParseFile(path.Path);

            string assemblyName = "GatsbyPlugin_" + Path.GetFileNameWithoutExtension(path.Path);

            var compilation = Compilation.Create(assemblyName, new CompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateAssemblyReference("mscorlib"))
                .AddSyntaxTrees(syntaxTree);

            using (FileStream stream = new FileStream(outputPath, FileMode.Create))
            {
                var result = compilation.Emit(stream);
                
                if (!result.Success)
                {
                    string message = string.Join(Environment.NewLine, result.Diagnostics);
                    throw new GatsbyException(string.Format("Failed to compile plugin {0}:\n{1}", path.Path, message));
                }
            }

            Assembly assembly = Assembly.LoadFile(Path.GetFullPath(outputPath));
            this.assemblies.Add(new AssemblyData()
                {
                    Assembly = assembly,
                    Path = outputPath
                });            
        }

        public void DeleteAll()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            foreach (AssemblyData assemblyData in this.assemblies)
                File.Delete(assemblyData.Path);
        }
    }
}
