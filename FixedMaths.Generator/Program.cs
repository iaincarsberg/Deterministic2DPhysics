using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FixedMaths.Core;
using FixedMaths.Generator.Api;

namespace FixedMaths.Generator
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("FixedMaths.Generator:");

            var registeredGenerators = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(RegisterGeneratorAttribute), true).Length > 0)
                .Select(type => (IGenerator) Activator.CreateInstance(type))
                .Where(x => x != null)
                .OrderBy(x => x.OperationName)
                .ToList();
            
            using var lutFs = File.Create(GetFilePath("math-lut", ".dat"));
            using var lutSw = new StreamWriter(lutFs);

            foreach (var generator in registeredGenerators)
            {
                Console.Write($"  {generator.OperationName}..");
                
                var data = generator.Generate();

                foreach (var (key, value) in data)
                {
                    if (!Enum.TryParse(generator.OperationName, out Operation operation))
                    {
                        throw new Exception($"Unable to convert {generator.OperationName} to {nameof(Operation)} enum.");
                    }

                    lutSw.WriteLine((long)operation);
                    lutSw.WriteLine(key);
                    lutSw.WriteLine(value.Value);
                }
                
                Console.WriteLine($" saved {data.Keys.Count} keys");
            }

            Console.WriteLine("Done");
        }
        
        private static string GetFilePath(string filename, string suffix = "Table.json")
        {
            var codeBase = Assembly.GetAssembly(typeof(FixedPoint))?.CodeBase;

            if (codeBase == null)
            {
                throw new Exception("Unable to find codebase");
            }

            var uri = new UriBuilder(codeBase);
            var path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path))?.Split(Path.DirectorySeparatorChar) ??
                       new string[0];
            var root = path.Take(Array.LastIndexOf(path, "FixedMaths.Generator"));

            return string.Join(Path.DirectorySeparatorChar, root.Concat(new[] {"FixedMaths.Core", "ProcessedTableData", $"{filename}{suffix}"}));
        }
    }
}