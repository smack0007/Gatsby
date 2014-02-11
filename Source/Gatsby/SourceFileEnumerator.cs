using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class SourceFileEnumerator
    {
        private static string DirectorySeperator = Path.DirectorySeparatorChar.ToString();

        public SourceFiles Enumerate(string sourcePath)
        {
            SourceFiles sourceFiles = new SourceFiles();

            List<SourceFilePath> pages = new List<SourceFilePath>();
            List<SourceFilePath> staticFiles = new List<SourceFilePath>();
            Scan(pages, staticFiles, sourcePath, sourcePath);

            sourceFiles.Pages = pages;
            sourceFiles.StaticFiles = staticFiles;
            sourceFiles.Posts = FindFilesToProcess(Path.Combine(sourcePath, "_Posts"));
            sourceFiles.Layouts = FindFilesToProcess(Path.Combine(sourcePath, "_Layouts"));
            sourceFiles.Includes = FindFilesToProcess(Path.Combine(sourcePath, "_Includes"));
                        
            return sourceFiles;
        }

        private static string CalculateRelativePath(string filePath, string basePath)
        {
            filePath = filePath.Substring(basePath.Length);

            if (filePath.StartsWith(DirectorySeperator))
                filePath = filePath.Substring(DirectorySeperator.Length);

            return filePath;
        }

        private static IEnumerable<SourceFilePath> FindFilesToProcess(string path)
        {
            if (!Directory.Exists(path))
                return Enumerable.Empty<SourceFilePath>();

            return Directory.EnumerateFiles(path, "*.cshtml", SearchOption.AllDirectories)
                .Select(x => new SourceFilePath()
                    {
                        Path = x,
                        RelativePath = CalculateRelativePath(x, path)
                    });
        }

        private static void Scan(List<SourceFilePath> pages, List<SourceFilePath> staticFiles, string path, string basePath)
        {
            foreach (var filePath in Directory.EnumerateFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly))
            {
                string file = Path.GetFileName(filePath);

                // Ignore anything that starts with an "_".
                if (file.StartsWith("_"))
                    continue;

                if (Directory.Exists(file))
                {
                    Scan(pages, staticFiles, file, basePath);
                }
                else
                {
                    SourceFilePath sourceFilePath = new SourceFilePath()
                            {
                                Path = filePath,
                                RelativePath = CalculateRelativePath(filePath, basePath)
                            };

                    if (Path.GetExtension(file) == ".cshtml")
                    {
                        pages.Add(sourceFilePath);
                    }
                    else
                    {
                        staticFiles.Add(sourceFilePath);
                    }
                }
            }
        }
    }
}
