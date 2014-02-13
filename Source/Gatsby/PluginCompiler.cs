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
        public void Compile(SourceFilePath path)
        {
            SyntaxTree syntaxTree = SyntaxTree.ParseFile(path.Path);

            var compilation = Compilation.Create("HelloWorld", new CompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateAssemblyReference("mscorlib"))
                .AddSyntaxTrees(syntaxTree);

            using (MemoryStream ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                
                if (result.Success)
                {
                    //ms.Seek(0, SeekOrigin.Begin);
                    Assembly.Load(ms.ToArray());
                }
                else
                {
                    string message = string.Join(Environment.NewLine, result.Diagnostics);
                    throw new GatsbyException(string.Format("Failed to compile plugin {0}:\n{1}", path.Path, message));
                }
            }
        }
    }
}
