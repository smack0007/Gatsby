using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

        public void Compile(SourceFilePath path, string outputPath, PluginManager pluginManager)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseFile(path.AbsolutePath);

            string assemblyName = "GatsbyPlugin_" + Path.GetFileNameWithoutExtension(path.AbsolutePath);

			var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
						
			var compilation = CSharpCompilation.Create(assemblyName)
				.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
				.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "mscorlib.dll")))
				.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "System.dll")))
				.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "System.Core.dll")))
				.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "Microsoft.CSharp.dll")))
				.AddReferences(new MetadataFileReference(Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "RazorBurn.dll")))
				.AddReferences(new MetadataFileReference(Assembly.GetEntryAssembly().Location))
				.AddSyntaxTrees(syntaxTree);

            using (FileStream stream = new FileStream(outputPath, FileMode.Create))
            {
                var result = compilation.Emit(stream);
                
                if (!result.Success)
                {
                    string message = string.Join(Environment.NewLine, result.Diagnostics);
                    throw new GatsbyException(string.Format("Failed to compile plugin {0}:\n{1}", path.AbsolutePath, message));
                }
            }

            Assembly assembly = Assembly.LoadFile(Path.GetFullPath(outputPath));
            this.assemblies.Add(new AssemblyData()
                {
                    Assembly = assembly,
                    Path = outputPath
                });

            foreach (Type type in assembly.GetTypes())
                pluginManager.Register(type);
        }

        public void DeleteAll()
        {
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();

            //foreach (AssemblyData assemblyData in this.assemblies)
            //    File.Delete(assemblyData.Path);
        }
    }
}
