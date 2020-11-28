using System;
using FixedMaths.Core;
using Physics.Core.EntityComponents;
using Physics.Core.Loggers;
using Physics.Core.Loggers.Data;
using Svelto.Common;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core.Engines
{
    [Sequenced(nameof(PhysicsEngineNames.ResolvePenetrationEngine))]
    public class ResolvePenetrationEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        private readonly IEngineScheduler _engineScheduler;
        private readonly IPhysicsCoreHandle _coreHandle;

        public string Name => nameof(ResolvePenetrationEngine);
        public EntitiesDB entitiesDB { get; set; }
        
        public ResolvePenetrationEngine(IEngineScheduler engineScheduler, IPhysicsCoreHandle coreHandle)
        {
            _coreHandle = coreHandle;
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

                    if (!manifold.CollisionManifold.HasValue)
                    {
                        continue;
                    }

                    var collisionManifold = manifold.CollisionManifold.Value;

                    //_coreHandle.SetSimulationSpeed(0.0f);

                    Console.WriteLine($"normal {collisionManifold.Normal} pen {collisionManifold.Penetration}");
                    
                    FixedPointVector2Logger.Instance.DrawCross(transform.Position - (collisionManifold.Normal * collisionManifold.Penetration), tick, Colour.Orange, FixedPoint.ConvertToInteger(MathFixedPoint.Round(collisionManifold.Penetration)));
                    
                    
                    
                    transform = TransformEntityComponent.From(
                        transform.Position - collisionManifold.Normal,
                        transform.PositionLastPhysicsTick,
                        transform.Position - collisionManifold.Normal / FixedPoint.Two);
                }
            }
        }
    }
}