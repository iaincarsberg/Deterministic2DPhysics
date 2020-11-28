using FixedMaths.Core;
using Svelto.ECS;

namespace Physics.Core.EntityComponents
{
    public readonly struct CircleColliderEntityComponent : IEntityComponent
    {
        public static CircleColliderEntityComponent From(FixedPoint radius, FixedPointVector2 center)
        {
            return new CircleColliderEntityComponent(radius, center);
        }

        public readonly FixedPoint Radius;
        public readonly FixedPointVector2 Center;

        private CircleColliderEntityComponent(FixedPoint radius, FixedPointVector2 center)
        {
            Radius = radius;
            Center = center;
        }
    }
}