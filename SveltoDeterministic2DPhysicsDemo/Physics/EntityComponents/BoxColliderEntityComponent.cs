using Svelto.ECS;
using SveltoDeterministic2DPhysicsDemo.Maths;
using SveltoDeterministic2DPhysicsDemo.Physics.CollisionStructures;

namespace SveltoDeterministic2DPhysicsDemo.Physics.EntityComponents
{
    public readonly struct BoxColliderEntityComponent : IEntityComponent
    {
        public static BoxColliderEntityComponent From(FixedPointVector2 size, FixedPointVector2 center)
        {
            return new BoxColliderEntityComponent(size, center);
        }

        private readonly FixedPointVector2 _size;
        private readonly FixedPointVector2 _center;

        private BoxColliderEntityComponent(in FixedPointVector2 size, in FixedPointVector2 center)
        {
            _size = size;
            _center = center;
        }

        public AABB ToAABB(FixedPointVector2 point)
        {
            return AABB.From(point - _center - _size, point - _center + _size);
        }
    }
}