using FixedMaths.Core;
using Physics.Core.Engines;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core
{
    public static class PhysicsCore
    {
        public static PhysicsCoreHandle RegisterTo(EnginesRoot enginesRoot, IEngineScheduler scheduler, FixedPoint physicsSimulationsPerSecond, ExclusiveGroupStruct syncGroup)
        {
            var entityFactory = enginesRoot.GenerateEntityFactory();
            var instance = new PhysicsCoreHandle(entityFactory);
            
            enginesRoot.AddEngine(new ApplyVelocityEngine(scheduler, physicsSimulationsPerSecond));
            enginesRoot.AddEngine(new DetectBoxVsBoxCollisionsEngine(scheduler));
            enginesRoot.AddEngine(new DetectCircleVsCircleCollisionsEngine(scheduler));
            enginesRoot.AddEngine(new DetectBoxVsCircleCollisionsEngine(scheduler));
            enginesRoot.AddEngine(new ResolveCollisionEngine(scheduler));
            enginesRoot.AddEngine(new ResolvePenetrationEngine(scheduler, instance));
            enginesRoot.AddEngine(new ClearPerFrameStateEngine(scheduler));
            enginesRoot.AddEngine(new PositionSyncEngine(scheduler, syncGroup));

            return instance;
        }
    }
}