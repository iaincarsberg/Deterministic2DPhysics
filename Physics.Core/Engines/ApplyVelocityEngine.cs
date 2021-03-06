﻿using FixedMaths.Core;
using Physics.Core.EntityComponents;
using Svelto.Common;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core.Engines
{
    [Sequenced(nameof(PhysicsEngineNames.ApplyVelocityEngine))]
    public class ApplyVelocityEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        private readonly IEngineScheduler _engineScheduler;
        private readonly FixedPoint _physicsSimulationsPerSecond;

        public ApplyVelocityEngine(IEngineScheduler engineScheduler, FixedPoint physicsSimulationsPerSecond)
        {
            _physicsSimulationsPerSecond = physicsSimulationsPerSecond;
            _engineScheduler = engineScheduler;
        }

        public void Ready()
        {
            _engineScheduler.RegisterScheduledPhysicsEngine(this);
        }

        public EntitiesDB entitiesDB { get; set; }

        public void Execute(ulong tick)
        {
            foreach (var ((rigidbodies, transforms, count), _) in entitiesDB.QueryEntities<RigidbodyEntityComponent, TransformEntityComponent>(GameGroups.RigidBodyGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    ref var transform = ref transforms[i];
                    ref var rigidbody = ref rigidbodies[i];
                    
                    if (rigidbody.IsKinematic)
                    {
                        continue;
                    }
                    
                    var position = transform.Position;
                    var velocity = rigidbody.Direction * rigidbody.Speed;

                    var targetPosition = position + velocity / _physicsSimulationsPerSecond;
                    
                    transform = TransformEntityComponent.From(targetPosition, position);
                    rigidbody = rigidbody.CloneAndReplaceVelocity(velocity);
                }
            }
        }
    }
}