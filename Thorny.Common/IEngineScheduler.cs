using FixedMaths.Core;

namespace Thorny.Common
{
    public interface IEngineScheduler
    {
        void RegisterScheduledPhysicsEngine(IScheduledPhysicsEngine scheduled);
        void RegisterScheduledGraphicsEngine(IScheduledGraphicsEngine scheduledGraphicsEngine);
        void ExecutePhysics(ulong tick);
        void ExecuteGraphics(FixedPoint delta, ulong tick);
    }
}