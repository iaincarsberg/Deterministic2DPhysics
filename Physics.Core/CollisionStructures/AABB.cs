using FixedMaths.Core;

namespace Physics.Core.CollisionStructures
{
    public readonly struct AABB
    {
        public static bool AABBvsAABB(AABB a, AABB b)
        {
            // Exit with no intersection if found separated along an axis
            if (a.Max.X < b.Min.X || a.Min.X > b.Max.X)
            {
                return false;
            }

            if (a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y)
            {
                return false;
            }

            // No separating axis found, therefor there is at least one overlapping axis
            return true;
        }
        
        public static AABB From(FixedPointVector2 min, FixedPointVector2 max)
        {
            return new AABB(min, max);
        }

        public readonly FixedPointVector2 Min;
        public readonly FixedPointVector2 Max;

        private AABB(FixedPointVector2 min, FixedPointVector2 max)
        {
            Min = min;
            Max = max;
        }
    }
}