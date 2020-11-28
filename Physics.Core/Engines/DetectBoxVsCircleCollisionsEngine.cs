using System;
using FixedMaths.Core;
using Physics.Core.CollisionStructures;
using Physics.Core.EntityComponents;
using Physics.Core.Types;
using Svelto.Common;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core.Engines
{
    [Sequenced(nameof(PhysicsEngineNames.DetectBoxVsCircleCollisionsEngine))]
    public class DetectBoxVsCircleCollisionsEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        public string Name => nameof(DetectBoxVsCircleCollisionsEngine);
        public EntitiesDB entitiesDB { get; set; }
        
        private readonly IEngineScheduler _engineScheduler;
        
        public DetectBoxVsCircleCollisionsEngine(IEngineScheduler engineScheduler)
        {
            _engineScheduler = engineScheduler;
        }
        
        public void Ready()
        {
            _engineScheduler.RegisterScheduledPhysicsEngine(this);
        }
        
        public void Execute(ulong tick)
        {
            foreach (var (boxes, _) in entitiesDB.QueryEntities<TransformEntityComponent, BoxColliderEntityComponent, CollisionManifoldEntityComponent, RigidbodyEntityComponent>(GameGroups.RigidBodyWithBoxColliderGroups))
            {
                foreach (var (circles, _) in entitiesDB.QueryEntities<TransformEntityComponent, CircleColliderEntityComponent, CollisionManifoldEntityComponent, RigidbodyEntityComponent>(GameGroups.RigidBodyWithCircleColliderGroups))
                {
                    var (boxTransforms, boxColliders, boxManifolds, boxRigidbodies, boxCount) = boxes.ToBuffers();
                    var (circleTransforms, circleColliders, circleManifolds, circleRigidbodies, circleCount) = circles.ToBuffers();

                    for (var boxIndex = 0; boxIndex < boxCount; boxIndex++)
                    {
                        ref var boxCollider = ref boxColliders[boxIndex];
                        ref var boxTransform = ref boxTransforms[boxIndex];
                        ref var boxRigidbody = ref boxRigidbodies[boxIndex];

                        for (var circleIndex = 0; circleIndex < circleCount; circleIndex++)
                        {
                            ref var circleCollider = ref circleColliders[circleIndex];
                            ref var circleTransform = ref circleTransforms[circleIndex];
                            ref var circleRigidbody = ref circleRigidbodies[circleIndex];

                            if (boxRigidbody.IsKinematic && circleRigidbody.IsKinematic)
                            {
                                continue;
                            }

                            var manifold = CalculateManifold(tick, boxIndex, boxTransform, boxCollider, circleIndex, circleTransform, circleCollider);

                            if (!manifold.HasValue)
                            {
                                continue;
                            }
                    
                            boxManifolds[boxIndex] = CollisionManifoldEntityComponent.From(manifold.Value);
                            circleManifolds[circleIndex] = CollisionManifoldEntityComponent.From(manifold.Value);
                        }
                    }
                }
            }
        }

        private static CollisionManifold? CalculateManifold(ulong tick, int boxIndex, TransformEntityComponent boxTransform, BoxColliderEntityComponent boxCollider, int circleIndex, TransformEntityComponent circleTransform, CircleColliderEntityComponent circleCollider)
        {
            // Vector from A to B
            var n = circleTransform.Position - boxTransform.Position;

            var aabb = boxCollider.ToAABB(boxTransform.Position);
 
            // Calculate half extents along each axis
            var xExtent = (aabb.Max.X - aabb.Min.X) / FixedPoint.Two;
            var yExtent = (aabb.Max.Y - aabb.Min.Y) / FixedPoint.Two;
 
            // Clamp point to edges of the AABB
            var closest = FixedPointVector2.From(
                MathFixedPoint.Clamp(n.X, -xExtent, xExtent),
                MathFixedPoint.Clamp(n.Y, -yExtent, yExtent));

            var inside = false;
 
            // Circle is inside the AABB, so we need to clamp the circle's center
            // to the closest edge
            if(n.Equals(closest))
            {
                inside = true;
 
                // Find closest axis
                if(MathFixedPoint.Abs( n.X ) > MathFixedPoint.Abs( n.Y ))
                {
                    // Clamp to closest extent
                    closest = closest.X > FixedPoint.Zero 
                        ? closest.CloneWithReplacedX(xExtent) 
                        : closest.CloneWithReplacedX(-xExtent);
                }
 
                // y axis is shorter
                else
                {
                    // Clamp to closest extent
                    closest = closest.Y > FixedPoint.Zero 
                        ? closest.CloneWithReplacedY(yExtent) 
                        : closest.CloneWithReplacedY(-yExtent);
                }
            }

            var normal = n - closest;
            var d = normal.LengthSquared();
            var r = circleCollider.Radius;
 
            // Early out of the radius is shorter than distance to closest point and
            // Circle not inside the AABB
            if (d > r * r && !inside)
            {
                return null;
            }

            // Avoided sqrt until we needed
            d = MathFixedPoint.Sqrt(d);
            
            
 
            // Collision normal needs to be flipped to point outside if circle was
            // inside the AABB
            if(inside)
            {
                var penetration = r - d;
                
                Console.WriteLine($"{tick} - box/circle collision {boxIndex} vs {circleIndex} | pen: {penetration}, normal: {-normal} into {-normal.Normalize()}, inside: {inside}");
                
                return CollisionManifold.From(penetration, -normal.Normalize(), CollisionType.AABBToCircle, boxIndex, circleIndex);
            }
            else
            {
                var penetration = r - d;
                
                Console.WriteLine($"{tick} - box/circle collision {boxIndex} vs {circleIndex} | pen: {penetration}, normal: {normal} into {normal.Normalize()}, inside: {inside}");
                    
                return CollisionManifold.From(penetration, normal.Normalize(), CollisionType.AABBToCircle, boxIndex, circleIndex);
            }
        }
    }
}