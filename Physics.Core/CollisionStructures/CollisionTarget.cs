using FixedMaths.Core;
using Physics.Core.EntityComponents;

namespace Physics.Core.CollisionStructures
{
    public readonly struct CollisionTarget
    {
        public static CollisionTarget From(TransformEntityComponent transform, RigidbodyEntityComponent rigidbody)
        {
            return new CollisionTarget(transform.Position, rigidbody.Direction, rigidbody.Velocity, rigidbody.Speed, rigidbody.Potential, rigidbody.Restitution, rigidbody.Mass, rigidbody.InverseMass);
        }

        public readonly FixedPointVector2 Position;
        public readonly FixedPointVector2 Direction;
        public readonly FixedPointVector2 Velocity;
        public readonly FixedPoint Speed;
        public readonly FixedPoint Potential;
        public readonly FixedPoint Restitution;
        public readonly FixedPoint Mass;
        public readonly FixedPoint InverseMass;

        public CollisionTarget(FixedPointVector2 position, FixedPointVector2 direction, FixedPointVector2 velocity, FixedPoint speed, FixedPoint potential, FixedPoint restitution, FixedPoint mass, FixedPoint inverseMass)
        {
            Position = position;
            Direction = direction;
            Velocity = velocity;
            Speed = speed;
            Potential = potential;
            Restitution = restitution;
            Mass = mass;
            InverseMass = inverseMass;
        }
    }
}