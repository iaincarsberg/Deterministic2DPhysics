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
            enginesRoot.AddEngine(new ApplyVelocityEngine(scheduler, physicsSimulationsPerSecond));
            enginesRoot.AddEngine(new DetectCollisionsEngine(scheduler));
            enginesRoot.AddEngine(new ResolvePenetrationEngine(scheduler));
            enginesRoot.AddEngine(new ResolveCollisionEngine(scheduler));
            enginesRoot.AddEngine(new ClearPerFrameStateEngine(scheduler));
            enginesRoot.AddEngine(new PositionSyncEngine(scheduler, syncGroup));

            var entityFactory = enginesRoot.GenerateEntityFactory();
            
            var instance = new PhysicsCoreHandle(entityFactory);
            
            return instance;
        }
    }

    public class PhysicsCoreHandle
    {
        private readonly IEntityFactory _entityFactory;

        public PhysicsCoreHandle(IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
        }
    }
}