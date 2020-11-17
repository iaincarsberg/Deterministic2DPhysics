using Physics.Core.CollisionStructures;
using Svelto.ECS;

namespace Physics.Core.EntityComponents
{
    public readonly struct CollisionManifoldEntityComponent : IEntityComponent
    {
        public static readonly CollisionManifoldEntityComponent Default = new CollisionManifoldEntityComponent();
        
        public static CollisionManifoldEntityComponent From(CollisionManifold collisionManifold, CollisionTarget collisionTarget)
        {
            return new CollisionManifoldEntityComponent(collisionManifold, collisionTarget);
        }
        
        public readonly CollisionManifold? CollisionManifold;
        public readonly CollisionTarget? CollisionTarget;

        private CollisionManifoldEntityComponent(CollisionManifold? collisionManifold, CollisionTarget? collisionTarget)
        {
            CollisionManifold = collisionManifold;
            CollisionTarget = collisionTarget;
        }
    }
}