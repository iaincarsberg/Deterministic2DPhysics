using FixedMaths.Core;
using Physics.Core.Types;
using Svelto.ECS;

namespace Physics.Core.CollisionStructures
{
    public readonly struct CollisionManifold
    {
        public static CollisionManifold From(FixedPoint penetration, FixedPointVector2 normal, CollisionType collisionType)
        {
            return new CollisionManifold(penetration, normal, collisionType);
        }

        public readonly FixedPoint Penetration;
        public readonly FixedPointVector2 Normal;
        public readonly CollisionType CollisionType;

        private CollisionManifold(FixedPoint penetration, FixedPointVector2 normal, CollisionType collisionType)
        {
            Penetration = penetration;
            Normal = normal;
            CollisionType = collisionType;
        }

        public CollisionManifold Reverse()
        {
            return new CollisionManifold(Penetration * FixedPoint.NegativeOne, Normal * FixedPoint.NegativeOne, CollisionType);
        }
    }
}