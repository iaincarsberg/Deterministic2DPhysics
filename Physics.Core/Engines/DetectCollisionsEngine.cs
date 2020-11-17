using FixedMaths.Core;
using Physics.Core.CollisionStructures;
using Physics.Core.EntityComponents;
using Physics.Core.Loggers;
using Physics.Core.Loggers.Data;
using Physics.Core.Types;
using Svelto.Common;
using Svelto.DataStructures;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core.Engines
{
    [Sequenced(nameof(PhysicsEngineNames.DetectCollisionsEngine))]
    public class DetectCollisionsEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        private readonly IEngineScheduler _engineScheduler;
        
        public string Name => nameof(DetectCollisionsEngine);
        public EntitiesDB entitiesDB { get; set; }

        public DetectCollisionsEngine(IEngineScheduler engineScheduler)
        {
            _engineScheduler = engineScheduler;
        }
        
        public void Ready()
        {
            _engineScheduler.RegisterScheduledPhysicsEngine(this);
        }

        public void Execute(ulong tick)
        {
            foreach (var ((transforms, colliders, manifolds, rigidBodies, count), _) in entitiesDB.QueryEntities<TransformEntityComponent, BoxColliderEntityComponent, CollisionManifoldEntityComponent, RigidbodyEntityComponent>(GameGroups.RigidBodyWithBoxColliderGroups))
            {
                AABBToAABB(tick, transforms, colliders, manifolds, rigidBodies, count);
            }
        }

        private static void AABBToAABB(ulong tick, NB<TransformEntityComponent> transforms, NB<BoxColliderEntityComponent> colliders, NB<CollisionManifoldEntityComponent> manifolds, NB<RigidbodyEntityComponent> rigidBodies, int count)
        {
            for (var a = 0; a < count; a++)
            {
                ref var colliderA = ref colliders[a];
                ref var transformA = ref transforms[a];
                ref var manifoldA = ref manifolds[a];
                ref var rigidBodyA = ref rigidBodies[a];

                var aabbA = colliderA.ToAABB(transformA.Position);

                for (var b = a + 1; b < count; b++)
                {
                    ref var rigidBodyB = ref rigidBodies[b];

                    if (rigidBodyA.IsKinematic && rigidBodyB.IsKinematic)
                    {
                        continue;
                    }
                    ref var colliderB = ref colliders[b];
                    ref var transformB = ref transforms[b];
                    ref var manifoldB = ref manifolds[b];

                    var aabbB = colliderB.ToAABB(transformB.Position);

                    
                    var manifold = CalculateManifold(aabbA, aabbB);

                    if (!manifold.HasValue)
                    {
                        continue;
                    }
                    /*
                    FixedPointVector2Logger.Instance.DrawBox(aabbA.Min, aabbA.Max, tick, Colour.LightYellow);
                    FixedPointVector2Logger.Instance.DrawCross(transformA.Position, tick, Colour.LightYellow, 3);
                    FixedPointVector2Logger.Instance.DrawLine(transformA.Position, transformA.PositionLastPhysicsTick, tick, Colour.LightYellow, 3);

                    FixedPointVector2Logger.Instance.DrawBox(aabbB.Min, aabbB.Max, tick, Colour.GreenYellow);
                    FixedPointVector2Logger.Instance.DrawCross(transformB.Position, tick, Colour.GreenYellow, 3);
                    FixedPointVector2Logger.Instance.DrawLine(transformB.Position, transformB.PositionLastPhysicsTick, tick, Colour.GreenYellow, 3);
                    
                    FixedPointVector2Logger.Instance.DrawCross(transformA.Position + manifold.Value.Normal, tick, Colour.Gold, FixedPoint.ConvertToInteger(manifold.Value.Penetration));
*/
                    manifoldA = CollisionManifoldEntityComponent.From(manifold.Value, CollisionTarget.From(transformB, rigidBodyB));
                    manifoldB = CollisionManifoldEntityComponent.From(manifold.Value.Reverse(), CollisionTarget.From(transformA, rigidBodyA));
                }
            }
        }

        private static CollisionManifold? CalculateManifold(AABB a, AABB b)  {
            // First, calculate the Minkowski difference. a maps to red, and b maps to blue from our example (though it doesn't matter!)
            var top = a.Max.Y - b.Min.Y;
            var bottom = a.Min.Y - b.Max.Y;
            var left = a.Min.X - b.Max.X;
            var right = a.Max.X - b.Min.X;
            //var extents = ((a.Max - a.Min) / FixedPointVector2.From(2, 2)) + ((b.Max - b.Min) / FixedPointVector2.From(2, 2));
            
            
            // If the Minkowski difference intersects the origin, there's a collision
            if (right >= FixedPoint.Zero && left <= FixedPoint.Zero && top >= FixedPoint.Zero && bottom <= FixedPoint.Zero) {
                // The pen vector is the shortest vector from the origin of the MD to an edge.
                // You know this has to be a vertical or horizontal line from the origin (these are by def. the shortest)
                var min = FixedPoint.MaxValue;
                FixedPointVector2? penetration = null;
                
                if (MathFixedPoint.Abs(left) < min) {
                    min = MathFixedPoint.Abs(left);
                    penetration = FixedPointVector2.From(left, FixedPoint.Zero);
                }
                if (MathFixedPoint.Abs(right) < min) {
                    min = MathFixedPoint.Abs(right);
                    penetration = FixedPointVector2.From(right, FixedPoint.Zero);
                }
                if (MathFixedPoint.Abs(top) < min) {
                    min = MathFixedPoint.Abs(top);
                    penetration = FixedPointVector2.From(FixedPoint.Zero, top);
                }
                if (MathFixedPoint.Abs(bottom) < min) {
                    min = MathFixedPoint.Abs(bottom);
                    penetration = FixedPointVector2.From(FixedPoint.Zero, bottom);
                }

                if (penetration.HasValue)
                {
                    return CollisionManifold.From(
                        min,
                        penetration.Value.Normalize(), 
                        CollisionType.AABBToAABB);
                }
            }

            return null;
        }
    }
}