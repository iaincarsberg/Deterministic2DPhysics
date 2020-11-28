using SveltoDeterministic2DPhysicsDemo.Maths;

namespace SveltoDeterministic2DPhysicsDemo
{
    public interface IEngineScheduler
    {
        void RegisterScheduledPhysicsEngine(IScheduledPhysicsEngine scheduled);
        void RegisterScheduledGraphicsEngine(IScheduledGraphicsEngine scheduledGraphicsEngine);
        void ExecutePhysics(ulong tick);
        void ExecuteGraphics(FixedPoint delta, ulong tick);
    }
}