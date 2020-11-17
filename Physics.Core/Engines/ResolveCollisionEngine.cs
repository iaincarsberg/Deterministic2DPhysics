using System.Runtime.CompilerServices;
using FixedMaths.Core;
using Physics.Core.CollisionStructures;
using Physics.Core.EntityComponents;
using Svelto.Common;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core.Engines
{
    [Sequenced(nameof(PhysicsEngineNames.ResolveCollisionEngine))]
    public class ResolveCollisionEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        private readonly IEngineScheduler _engineScheduler;

        public ResolveCollisionEngine(IEngineScheduler engineScheduler)
        {
            _engineScheduler = engineScheduler;
        }
            
        public EntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            _engineScheduler.RegisterScheduledPhysicsEngine(this);
        }

        public void Execute(ulong tick)
        {
            foreach (var ((rigidbodies, manifolds, egids, count), _) in entitiesDB.QueryEntities<RigidbodyEntityComponent, CollisionManifoldEntityComponent, EGIDComponent>(GameGroups.RigidBodyGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    ref var manifold = ref manifolds[i];
                    ref var rigidbody = ref rigidbodies[i];
                    ref var egid = ref egids[i];

                    if (rigidbody.IsKinematic)
                    {
                        continue;
                    }

                    if (manifold.CollisionManifold.HasValue && manifold.CollisionTarget.HasValue)
                    {
                        rigidbody = ResolveCollision(egid, tick, manifold.CollisionManifold.Value, rigidbody, manifold.CollisionTarget.Value);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RigidbodyEntityComponent ResolveCollision(EGIDComponent egid, ulong tick, CollisionManifold manifold, RigidbodyEntityComponent rigidbody, CollisionTarget collisionTarget)
        {
            // Calculate relative velocity
            var rv = collisionTarget.Velocity - rigidbody.Velocity;

            // Calculate relative velocity in terms of the normal direction
            var velAlongNormal = FixedPointVector2.Dot(rv, manifold.Normal);

            // Do not resolve if velocities are separating
            if (velAlongNormal > FixedPoint.Zero)
            {
                return rigidbody;
            }

            // Calculate restitution
            var e = MathFixedPoint.Min(rigidbody.Restitution, collisionTarget.Restitution);

            // Calculate impulse scalar
            var j = -(FixedPoint.One + e) * velAlongNormal;
            j /= rigidbody.InverseMass + collisionTarget.InverseMass;

            // Apply impulse
            var impulse = manifold.Normal * j;
            
            //Console.WriteLine($"ResolveCollision {tick} {egid.ID.entityID} {rigidbody.Direction} | {rigidbody.Velocity} - {impulse} * {rigidbody.InverseMass} == {(rigidbody.Velocity - impulse * rigidbody.InverseMass).Normalize()}");
            
            //return rigidbody.CloneAndReplaceDirection((rigidbody.Velocity - impulse * rigidbody.InverseMass).Normalize());
            return rigidbody.CloneAndReplaceDirection((rigidbody.Velocity - impulse).Normalize());
        }
    }
}