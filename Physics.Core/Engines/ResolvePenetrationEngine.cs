using FixedMaths.Core;
using Physics.Core.EntityComponents;
using Svelto.Common;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core.Engines
{
    [Sequenced(nameof(PhysicsEngineNames.ResolvePenetrationEngine))]
    public class ResolvePenetrationEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        private readonly IEngineScheduler _engineScheduler;
        
        public string Name => nameof(ResolvePenetrationEngine);
        public EntitiesDB entitiesDB { get; set; }
        
        public ResolvePenetrationEngine(IEngineScheduler engineScheduler)
        {
            _engineScheduler = engineScheduler;
        }
        public void Ready()
        {
            _engineScheduler.RegisterScheduledPhysicsEngine(this);
        }

        public void Execute(ulong tick)
        {
            foreach (var ((transforms, rigidbodies, manifolds, count), _) in entitiesDB.QueryEntities<TransformEntityComponent, RigidbodyEntityComponent, CollisionManifoldEntityComponent>(GameGroups.RigidBodyGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    ref var transform = ref transforms[i];
                    ref var manifold = ref manifolds[i];
                    ref var rigidbody = ref rigidbodies[i];

                    if (rigidbody.IsKinematic)
                    {
                        continue;
                    }

                    if (!manifold.CollisionManifold.HasValue || !manifold.CollisionTarget.HasValue)
                    {
                        continue;
                    }

                    var collisionManifold = manifold.CollisionManifold.Value;

                    /*
                    transform = TransformEntityComponent.From(
                        transform.Position - collisionManifold.Normal,
                        transform.PositionLastPhysicsTick,
                        transform.Position - collisionManifold.Normal / FixedPoint.Two);*/
                }
            }
        }
    }
}