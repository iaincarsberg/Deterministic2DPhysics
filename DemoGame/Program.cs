using FixedMaths.Core.Data;
using FixedMaths.FileSystem;
using Graphics.SDL2Driver;
using Thorny.Core;

namespace DemoGame
{
    internal static class Program
    {
        private static void Main()
        {
            var repository = ProcessedTableRepository.From("ProcessedTableData");
            ProcessedTableService.CreateInstance(repository);
            
            var sdl2 = new Sdl2Driver();
            new GameLoop()
                .AddGraphics(sdl2)
                .AddInput(sdl2)
                .SetPhysicsSimulationsPerSecond(30)
                .SetSimulationSpeed(1.0f)
                .SetUncappedGraphicsFramesPerSecond()
                .Execute();
        }
    }
}