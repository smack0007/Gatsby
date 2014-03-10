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

        public SourceFiles Enumerate(Config config)
        {
            SourceFiles sourceFiles = new SourceFiles();

            List<SourceFilePath> pages = new List<SourceFilePath>();
            List<SourceFilePath> staticFiles = new List<SourceFilePath>();
            Scan(pages, staticFiles, config.Source, config.Source, config);

            sourceFiles.Pages = pages;
            sourceFiles.StaticFiles = staticFiles;
            sourceFiles.Posts = FindFilesToProcess(Path.Combine(config.Source, "_Posts"), "*.cshtml", config.ExcludePatterns);
            sourceFiles.Generators = FindFilesToProcess(Path.Combine(config.Source, "_Generators"), "*.cshtml", config.ExcludePatterns);
            sourceFiles.Layouts = FindFilesToProcess(Path.Combine(config.Source, "_Layouts"), "*.cshtml", config.ExcludePatterns);
            sourceFiles.Includes = FindFilesToProcess(Path.Combine(config.Source, "_Includes"), "*.cshtml", config.ExcludePatterns);
            sourceFiles.Plugins = FindFilesToProcess(Path.Combine(config.Source, "_Plugins"), "*.cs", config.ExcludePatterns);            

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

        private static void Scan(List<SourceFilePath> pages, List<SourceFilePath> staticFiles, string path, string basePath, Config config)
        {
            var excludeFileNames = config.ExcludePatterns.SelectMany(x => Directory.EnumerateFileSystemEntries(path, x, SearchOption.TopDirectoryOnly));
            var fileNames = Directory.EnumerateFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly).Where(x => !excludeFileNames.Contains(x));

            foreach (var fileName in fileNames)
            {
                string file = Path.GetFileName(fileName);

                // Ignore anything that starts with an "_".
                if (file.StartsWith("_"))
                    continue;

                if (Directory.Exists(fileName))
                {
                    Scan(pages, staticFiles, fileName, basePath, config);
                }
                else
                {
                    SourceFilePath sourceFilePath = new SourceFilePath()
                        {
                            AbsolutePath = Path.GetFullPath(fileName),
                            RelativePath = CalculateRelativePath(fileName, basePath)
                        };

                    if (config.PageFileNameExtensions.Contains(Path.GetExtension(file).Substring(1)))
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
