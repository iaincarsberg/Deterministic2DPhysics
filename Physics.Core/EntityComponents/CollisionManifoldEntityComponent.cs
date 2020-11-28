using System;
using Physics.Core.CollisionStructures;
using Svelto.ECS;

namespace Physics.Core.EntityComponents
{
    public readonly struct CollisionManifoldEntityComponent : IEntityComponent
    {
        public static readonly CollisionManifoldEntityComponent Default = new CollisionManifoldEntityComponent();
        
        public static CollisionManifoldEntityComponent From(CollisionManifold collisionManifold)
        {
            return new CollisionManifoldEntityComponent(collisionManifold);
        }
        
        public readonly CollisionManifold? CollisionManifold;

        private CollisionManifoldEntityComponent(CollisionManifold? collisionManifold)
        {
            CollisionManifold = collisionManifold;
        }
        
        
        private bool Equals(CollisionManifoldEntityComponent other)
        {
            return Nullable.Equals(CollisionManifold, other.CollisionManifold) && CollisionManifold.Equals(other.CollisionManifold);
        }

        public override bool Equals(object obj)
        {
            return obj is CollisionManifoldEntityComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return CollisionManifold.GetHashCode();
        }
    }
}