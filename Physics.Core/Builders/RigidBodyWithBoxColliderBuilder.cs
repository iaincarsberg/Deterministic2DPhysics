using FixedMaths.Core;
using Physics.Core.Descriptors;
using Physics.Core.EntityComponents;
using Svelto.ECS;

namespace Physics.Core.Builders
{
    public class RigidBodyWithBoxColliderBuilder
    {
        public static RigidBodyWithBoxColliderBuilder Create()
        {
            return new RigidBodyWithBoxColliderBuilder();
        }

        private FixedPointVector2 _position = FixedPointVector2.Zero;
        private FixedPointVector2 _direction = FixedPointVector2.Zero;
        private FixedPointVector2 _boxColliderSize = FixedPointVector2.Zero;
        private FixedPointVector2 _boxColliderCentre = FixedPointVector2.Zero;
        private FixedPoint _speed = FixedPoint.Zero;
        private FixedPoint _restitution = FixedPoint.One;
        private FixedPoint _mass = FixedPoint.One;
        private bool _isKinematic;

        public RigidBodyWithBoxColliderBuilder SetPosition(FixedPointVector2 value)
        {
            _position = value;
            return this;
        }
        
        public RigidBodyWithBoxColliderBuilder SetDirection(FixedPointVector2 value)
        {
            _direction = value;
            return this;
        }
        
        public RigidBodyWithBoxColliderBuilder SetBoxColliderSize(FixedPointVector2 value)
        {
            _boxColliderSize = value;
            return this;
        }
        
        public RigidBodyWithBoxColliderBuilder SetBoxColliderCentre(FixedPointVector2 value)
        {
            _boxColliderCentre = value;
            return this;
        }
        
        public RigidBodyWithBoxColliderBuilder SetSpeed(FixedPoint value)
        {
            _speed = value;
            return this;
        }
        
        public RigidBodyWithBoxColliderBuilder SetRestitution(FixedPoint value)
        {
            _restitution = value;
            return this;
        }
        
        public RigidBodyWithBoxColliderBuilder SetMass(FixedPoint value)
        {
            _mass = value;
            return this;
        }

        public RigidBodyWithBoxColliderBuilder SetIsKinematic(bool isKinematic)
        {
            _isKinematic = isKinematic;
            return this;
        }

        public void Build(IEntityFactory entityFactory, uint egid)
        {
            var initializer = entityFactory.BuildEntity<RigidBodyWithBoxColliderDescriptor>(egid, GameGroups.RigidBodyWithBoxCollider);
            
            initializer.Init(TransformEntityComponent.From(_position, _position));
            initializer.Init(RigidbodyEntityComponent.From(_direction, _speed, _restitution, _mass, _isKinematic));
            initializer.Init(CollisionManifoldEntityComponent.Default);
            initializer.Init(BoxColliderEntityComponent.From(_boxColliderSize, _boxColliderCentre));
        }
    }
}