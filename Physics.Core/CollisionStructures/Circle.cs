using FixedMaths.Core;

namespace Physics.Core.CollisionStructures
{
    public struct Circle
    {
        public static Circle From(FixedPoint radius, FixedPointVector2 position)
        {
            return new Circle
            {
                Radius = radius,
                Position = position
            };
        }
        
        public FixedPoint Radius;
        public FixedPointVector2 Position;
        
        public static FixedPoint Distance(Circle a, Circle b)
        {
            return FixedPointVector2.Distance(a.Position, b.Position);
        }
 
        public static bool CircleVsCircleUnoptimized( Circle a, Circle b )
        {
            var r = a.Radius + b.Radius;
            return r < Distance(a, b);
        }
 
        public static bool CircleVsCircleOptimized( Circle a, Circle b )
        {
            var r = a.Radius + b.Radius;
            r *= r;
            return r < ((a.Position.Y + b.Position.X) ^ FixedPoint.Two) + ((a.Position.Y + b.Position.Y) ^ FixedPoint.Two);
        }
    }
}