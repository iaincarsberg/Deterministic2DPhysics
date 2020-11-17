using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FixedMaths.Core.Data;
using FixedMaths.FileSystem;

namespace FixedMaths.Test.ClassFixtures
{
    public class ProcessedTableRepositoryFixture : IDisposable
    {
        public ProcessedTableRepositoryFixture()
        {
            var repository = ProcessedTableRepository.From(GetFilePath());
            ProcessedTableService.CreateInstance(repository);
        }
        
        private static string GetFilePath()
        {
            var codeBase = Assembly.GetAssembly(typeof(ProcessedTableRepositoryFixture))?.CodeBase;

            if (codeBase == null)
            {
                throw new Exception("Unable to find codebase");
            }

            var uri = new UriBuilder(codeBase);
            var path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path))?.Split(Path.DirectorySeparatorChar) ?? new string[0];
            var root = path.Take(Array.LastIndexOf(path, "FixedMaths.Test"));

            return string.Join(Path.DirectorySeparatorChar, root.Concat(new[] {"FixedMaths.Core", "ProcessedTableData"}));
        }

        public void Dispose()
        {
        }
    }
}