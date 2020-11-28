using System;
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
    [Sequenced(nameof(PhysicsEngineNames.DetectCircleVsCircleCollisionsEngine))]
    public class DetectCircleVsCircleCollisionsEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        private readonly IEngineScheduler _engineScheduler;
        
        public string Name => nameof(DetectCircleVsCircleCollisionsEngine);
        public EntitiesDB entitiesDB { get; set; }

        public DetectCircleVsCircleCollisionsEngine(IEngineScheduler engineScheduler)
        {
            _engineScheduler = engineScheduler;
        }
        
        public void Ready()
        {
            _engineScheduler.RegisterScheduledPhysicsEngine(this);
        }

        public void Execute(ulong tick)
        {
            foreach (var ((transforms, colliders, manifolds, rigidbodies, count), _) in entitiesDB.QueryEntities<TransformEntityComponent, CircleColliderEntityComponent, CollisionManifoldEntityComponent, RigidbodyEntityComponent>(GameGroups.RigidBodyWithCircleColliderGroups))
            {
                for (var a = 0; a < count; a++)
                {
                    ref var colliderA = ref colliders[a];
                    ref var transformA = ref transforms[a];
                    ref var rigidBodyA = ref rigidbodies[a];

                    for (var b = a + 1; b < count; b++)
                    {
                        ref var colliderB = ref colliders[b];
                        ref var transformB = ref transforms[b];
                        ref var rigidBodyB = ref rigidbodies[b];

                        if (rigidBodyA.IsKinematic && rigidBodyB.IsKinematic)
                        {
                            continue;
                        }

                        var manifold = CalculateManifold(a, transformA, colliderA, b, transformB, colliderB);

                        if (!manifold.HasValue)
                        {
                            continue;
                        }
                        
                        Console.WriteLine($"circle collision {a} vs {b}");
                    
                        manifolds[a] = CollisionManifoldEntityComponent.From(manifold.Value);
                    }
                }
            }
        }

        private static CollisionManifold? CalculateManifold(int indexA, TransformEntityComponent transformA, CircleColliderEntityComponent colliderA, int indexB, TransformEntityComponent transformB, CircleColliderEntityComponent colliderB)
        {
            // Vector from A to B
            var n = transformB.Position - transformA.Position;

            var r = (colliderA.Radius + colliderB.Radius).Squared();

            // Length between vectors
            var d = n.Length();

            if (d.Squared() > r)
            {
                return null;
            }

            // Circles have collided, now compute manifold
            
            // Check if circles are on same position
            if (d.Equals(FixedPoint.Zero))
            {
                return CollisionManifold.From(colliderA.Radius, FixedPointVector2.Up, CollisionType.CircleToCircle, indexA, indexB);
            }

            // Distance is difference between radius and distance
            var penetration = r - d;
 
            // Utilize our d since we performed sqrt on it already within Length( )
            // Points from A to B, and is a unit vector
            var normal = n / d;
            return CollisionManifold.From(penetration, normal.Normalize(), CollisionType.CircleToCircle, indexA, indexB);
        }
    }
}