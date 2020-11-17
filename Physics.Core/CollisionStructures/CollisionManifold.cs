using FixedMaths.Core;
using Physics.Core.Types;
using Svelto.ECS;

namespace Physics.Core.CollisionStructures
{
    public readonly struct CollisionManifold
    {
        public static CollisionManifold From(EGID a, EGID b, FixedPoint penetration, FixedPointVector2 normal, CollisionType collisionType)
        {
            return new CollisionManifold(a, b, penetration, normal, collisionType);
        }

        public readonly EGID A;
        public readonly EGID B;
        public readonly FixedPoint Penetration;
        public readonly FixedPointVector2 Normal;
        public readonly CollisionType CollisionType;

        private CollisionManifold(EGID a, EGID b, FixedPoint penetration, FixedPointVector2 normal, CollisionType collisionType)
        {
            A = a;
            B = b;
            Penetration = penetration;
            Normal = normal;
            CollisionType = collisionType;
        }

        public bool Involves(EGID other)
        {
            return A.entityID.Equals(other.entityID) || B.entityID.Equals(other.entityID);
        }

        public CollisionManifold Reverse()
        {
            return new CollisionManifold(B, A, Penetration * FixedPoint.NegativeOne, Normal * FixedPoint.NegativeOne, CollisionType);
        }
    }
}