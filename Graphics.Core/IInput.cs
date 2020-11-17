namespace Graphics.Core
{
    public interface IInput
    {
        void PollEvents(IGameLoop gameLoop);
    }

    public interface IGameLoop
    {
        void Stop();
        void Execute();
        IGameLoop AddGraphics(IGraphics graphics);
        IGameLoop AddInput(IInput input);
        IGameLoop SetPhysicsSimulationsPerSecond(uint frequency);
        IGameLoop SetGraphicsFramesPerSecond(uint frequency);
        IGameLoop SetUncappedGraphicsFramesPerSecond();
        IGameLoop SetSimulationSpeed(float simulationSpeed);
    }
}