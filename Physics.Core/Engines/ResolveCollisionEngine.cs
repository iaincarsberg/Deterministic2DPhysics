using System;
using System.Runtime.CompilerServices;
using FixedMaths.Core;
using Physics.Core.CollisionStructures;
using Physics.Core.EntityComponents;
using Physics.Core.Types;
using Svelto.Common;
using Svelto.DataStructures;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core.Engines
{
    [Sequenced(nameof(PhysicsEngineNames.ResolveCollisionEngine))]
    public class ResolveCollisionEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        private readonly IEngineScheduler _engineScheduler;
        
        public string Name => nameof(ResolveCollisionEngine);
        public EntitiesDB entitiesDB { get; set; }

        public ResolveCollisionEngine(IEngineScheduler engineScheduler)
        {
            _engineScheduler = engineScheduler;
        }
        
        public void Ready()
        {
            _engineScheduler.RegisterScheduledPhysicsEngine(this);
        }

        public void Execute(ulong tick)
        {
            var boxRigidbodies = new NB<RigidbodyEntityComponent>();
            var boxManifolds = new NB<CollisionManifoldEntityComponent>();
            var boxCount = 0;
            var circleRigidbodies = new NB<RigidbodyEntityComponent>();
            var circleManifolds = new NB<CollisionManifoldEntityComponent>();
            var circleCount = 0;
            
            foreach (var ((rigidbodies, manifolds, count), groupStruct) in entitiesDB.QueryEntities<RigidbodyEntityComponent, CollisionManifoldEntityComponent>(GameGroups.RigidBodyGroups))
            {
                if (groupStruct.Equals(GameGroups.RigidBodyWithBoxCollider))
                {
                    boxRigidbodies = rigidbodies;
                    boxManifolds = manifolds;
                    boxCount = count;
                }
                if (groupStruct.Equals(GameGroups.RigidBodyWithCircleCollider))
                {
                    circleRigidbodies = rigidbodies;
                    circleManifolds = manifolds;
                    circleCount = count;
                }
            }

            for (var i = 0; i < boxCount; i++)
            {
                ref var manifold = ref boxManifolds[i];

                if (!manifold.CollisionManifold.HasValue)
                {
                    continue;
                }

                if (manifold.CollisionManifold.Value.CollisionType.Equals(CollisionType.AABBToCircle))
                {
                    ResolveCollision(i, manifold.CollisionManifold.Value, ref boxRigidbodies[manifold.CollisionManifold.Value.EntityIndex1], ref circleRigidbodies[manifold.CollisionManifold.Value.EntityIndex2]);
                }
                else
                {
                    ResolveCollision(i, manifold.CollisionManifold.Value, ref boxRigidbodies[manifold.CollisionManifold.Value.EntityIndex1], ref boxRigidbodies[manifold.CollisionManifold.Value.EntityIndex2]);
                }
            }
            
            for (var i = 0; i < circleCount; i++)
            {
                ref var manifold = ref circleManifolds[i];

                if (!manifold.CollisionManifold.HasValue)
                {
                    continue;
                }

                if (manifold.CollisionManifold.Value.CollisionType.Equals(CollisionType.AABBToCircle))
                {
                    ResolveCollision(i, manifold.CollisionManifold.Value, ref boxRigidbodies[manifold.CollisionManifold.Value.EntityIndex1], ref circleRigidbodies[manifold.CollisionManifold.Value.EntityIndex2]);
                }
                else
                {
                    ResolveCollision(i, manifold.CollisionManifold.Value, ref circleRigidbodies[manifold.CollisionManifold.Value.EntityIndex1], ref circleRigidbodies[manifold.CollisionManifold.Value.EntityIndex2]);
                }
            }

/*
            foreach (var ((rigidbodies, manifolds, count), groupStruct) in entitiesDB.QueryEntities<RigidbodyEntityComponent, CollisionManifoldEntityComponent>(GameGroups.RigidBodyGroups))
            {
                Console.WriteLine($"box: {groupStruct.Equals(GameGroups.RigidBodyWithBoxCollider)}, circle: {groupStruct.Equals(GameGroups.RigidBodyWithCircleCollider)}");
                
                Console.WriteLine($"group {count}");
                for (var i = 0; i < count; i++)
                {
                    ref var manifold = ref manifolds[i];

                    if (!manifold.CollisionManifold.HasValue)
                    {
                        continue;
                    }

                    Console.WriteLine($"count: {count}, idx1: {manifold.CollisionManifold.Value.EntityIndex1}, idx2: {manifold.CollisionManifold.Value.EntityIndex2}");

                    ResolveCollision(i, manifold.CollisionManifold.Value, ref rigidbodies[manifold.CollisionManifold.Value.EntityIndex1], ref rigidbodies[manifold.CollisionManifold.Value.EntityIndex2]);
                }
            }
*/
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ResolveCollision(int index, CollisionManifold manifold, ref RigidbodyEntityComponent rigidbodyA, ref RigidbodyEntityComponent rigidbodyB)
        {
            // Calculate relative velocity
            var rv = rigidbodyB.Velocity - rigidbodyA.Velocity;

            // Calculate relative velocity in terms of the normal direction
            var velAlongNormal = FixedPointVector2.Dot(rv, manifold.Normal);

            // Do not resolve if velocities are separating
            if (velAlongNormal > FixedPoint.Zero)
            {
                return;
            }

            // Calculate restitution
            var e = MathFixedPoint.Min(rigidbodyA.Restitution, rigidbodyB.Restitution);

            // Calculate impulse scalar
            var j = -(FixedPoint.One + e) * velAlongNormal;
            //j /= rigidbody.InverseMass + collisionTarget.InverseMass;

            // Apply impulse
            var impulse = manifold.Normal * j;
            
            //Console.WriteLine($"ResolveCollision {tick} {egid.ID.entityID} {rigidbody.Direction} | {rigidbody.Velocity} - {impulse} * {rigidbody.InverseMass} == {(rigidbody.Velocity - impulse * rigidbody.InverseMass).Normalize()}");
            
            //return rigidbody.CloneAndReplaceDirection((rigidbody.Velocity - impulse * rigidbody.InverseMass).Normalize());
            
            
            //Console.WriteLine($"ResolveCollision {manifold.EntityIndex1} vs {manifold.EntityIndex2} dir:{rigidbodyA.Direction} norm: {manifold.Normal} j:{j} | vel:{rigidbodyA.Velocity} - imp:{impulse} == {(rigidbodyA.Velocity - impulse)} norm: {(rigidbodyA.Velocity - impulse).Normalize()}");

            if (manifold.CollisionType == CollisionType.AABBToCircle)
            {
                if (manifold.EntityIndex1 == index)
                {
                    rigidbodyA = rigidbodyA.CloneAndReplaceDirection((rigidbodyA.Velocity - impulse).Normalize());
                }
                else
                {
                    rigidbodyB = rigidbodyB.CloneAndReplaceDirection((rigidbodyB.Velocity + impulse).Normalize());
                }
            }
            else
            {
                if (!rigidbodyA.IsKinematic)
                {
                    rigidbodyA = rigidbodyA.CloneAndReplaceDirection((rigidbodyA.Velocity - impulse).Normalize());
                }

                if (!rigidbodyB.IsKinematic)
                {
                    rigidbodyB = rigidbodyB.CloneAndReplaceDirection((rigidbodyB.Velocity + impulse).Normalize());
                }
            }
        }
    }
}