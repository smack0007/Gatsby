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

        public SourceFiles Enumerate(string sourcePath, List<string> excludePatterns)
        {
            SourceFiles sourceFiles = new SourceFiles();

            List<SourceFilePath> pages = new List<SourceFilePath>();
            List<SourceFilePath> staticFiles = new List<SourceFilePath>();
            Scan(pages, staticFiles, sourcePath, sourcePath, excludePatterns);

            sourceFiles.Pages = pages;
            sourceFiles.StaticFiles = staticFiles;
            sourceFiles.Posts = FindFilesToProcess(Path.Combine(sourcePath, "_Posts"), "*.cshtml", excludePatterns);
            sourceFiles.Paginators = FindFilesToProcess(Path.Combine(sourcePath, "_Paginators"), "*.cshtml", excludePatterns);
            sourceFiles.Layouts = FindFilesToProcess(Path.Combine(sourcePath, "_Layouts"), "*.cshtml", excludePatterns);
            sourceFiles.Includes = FindFilesToProcess(Path.Combine(sourcePath, "_Includes"), "*.cshtml", excludePatterns);
            sourceFiles.Plugins = FindFilesToProcess(Path.Combine(sourcePath, "_Plugins"), "*.cs", excludePatterns);            

            return sourceFiles;
        }

        private static string CalculateRelativePath(string filePath, string basePath)
        {
            filePath = filePath.Substring(basePath.Length);

            if (filePath.StartsWith(DirectorySeperator))
                filePath = filePath.Substring(DirectorySeperator.Length);

            return filePath;
        }

        private static IEnumerable<SourceFilePath> FindFilesToProcess(string path, string searchPattern, List<string> excludePatterns)
        {
            if (!Directory.Exists(path))
                return Enumerable.Empty<SourceFilePath>();

            var excludeFiles = excludePatterns.SelectMany(x => Directory.EnumerateFiles(path, x, SearchOption.AllDirectories));

            return Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories)
                .Where(x => !excludeFiles.Contains(x))
                .Select(x => new SourceFilePath()
                    {
                        AbsolutePath = Path.GetFullPath(x),
                        RelativePath = CalculateRelativePath(x, path)
                    }).ToArray(); // ToArray is important to prevent delayed execution.
        }

        private static void Scan(List<SourceFilePath> pages, List<SourceFilePath> staticFiles, string path, string basePath, List<string> excludePatterns)
        {
            var excludeFileNames = excludePatterns.SelectMany(x => Directory.EnumerateFileSystemEntries(path, x, SearchOption.TopDirectoryOnly));
            var fileNames = Directory.EnumerateFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly).Where(x => !excludeFileNames.Contains(x));

            foreach (var fileName in fileNames)
            {
                string file = Path.GetFileName(fileName);

                // Ignore anything that starts with an "_".
                if (file.StartsWith("_"))
                    continue;

                if (Directory.Exists(fileName))
                {
                    Scan(pages, staticFiles, fileName, basePath, excludePatterns);
                }
                else
                {
                    SourceFilePath sourceFilePath = new SourceFilePath()
                        {
                            AbsolutePath = Path.GetFullPath(fileName),
                            RelativePath = CalculateRelativePath(fileName, basePath)
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
