using System;
using FixedMaths.Core;
using Physics.Core.Descriptors;
using Physics.Core.EntityComponents;
using Svelto.ECS;

namespace Physics.Core.Builders
{
    public class RigidBodyWithColliderBuilder
    {
        public static RigidBodyWithColliderBuilder Create()
        {
            return new RigidBodyWithColliderBuilder();
        }

        private FixedPointVector2 _position = FixedPointVector2.Zero;
        private FixedPointVector2 _direction = FixedPointVector2.Zero;
        private FixedPointVector2 _boxColliderSize = FixedPointVector2.Zero;
        private FixedPointVector2 _boxColliderCentre = FixedPointVector2.Zero;
        private FixedPointVector2 _circleColliderCentre = FixedPointVector2.Zero;
        private FixedPoint _circleColliderRadius = FixedPoint.Zero;
        private FixedPoint _speed = FixedPoint.Zero;
        private FixedPoint _restitution = FixedPoint.One;
        private FixedPoint _mass = FixedPoint.One;
        private bool _isKinematic;
        private ColliderType _colliderType = ColliderType.Box;

        public RigidBodyWithColliderBuilder SetPosition(FixedPointVector2 value)
        {
            _position = value;
            return this;
        }
        
        public RigidBodyWithColliderBuilder SetDirection(FixedPointVector2 value)
        {
            _direction = value;
            return this;
        }
        
        public RigidBodyWithColliderBuilder SetBoxCollider(FixedPointVector2 size, FixedPointVector2? centre = null)
        {
            _boxColliderSize = size;
            _boxColliderCentre = centre ?? FixedPointVector2.Zero;
            _colliderType = ColliderType.Box;
            
            return this;
        }
        
        public RigidBodyWithColliderBuilder SetCircleCollider(FixedPoint radius, FixedPointVector2? centre = null)
        {
            _circleColliderRadius = radius;
            _circleColliderCentre = centre ?? FixedPointVector2.Zero;
            _colliderType = ColliderType.Circle;
            
            return this;
        }

        public RigidBodyWithColliderBuilder SetSpeed(FixedPoint value)
        {
            _speed = value;
            return this;
        }
        
        public RigidBodyWithColliderBuilder SetRestitution(FixedPoint value)
        {
            _restitution = value;
            return this;
        }
        
        public RigidBodyWithColliderBuilder SetMass(FixedPoint value)
        {
            _mass = value;
            return this;
        }

        public RigidBodyWithColliderBuilder SetIsKinematic(bool isKinematic)
        {
            _isKinematic = isKinematic;
            return this;
        }

        public void Build(IEntityFactory entityFactory, uint egid)
        {
            var initializer = _colliderType switch
            {
                ColliderType.Box => entityFactory.BuildEntity<RigidBodyWithBoxColliderDescriptor>(egid, GameGroups.RigidBodyWithBoxCollider),
                ColliderType.Circle => entityFactory.BuildEntity<RigidBodyWithCircleColliderDescriptor>(egid, GameGroups.RigidBodyWithCircleCollider),
                _ => throw new ArgumentOutOfRangeException($"Unknown {_colliderType}")
            };
            
            initializer.Init(TransformEntityComponent.From(_position, _position));
            initializer.Init(RigidbodyEntityComponent.From(_direction, _speed, _restitution, _mass, _isKinematic));
            initializer.Init(CollisionManifoldEntityComponent.Default);

            switch (_colliderType)
            {
                case ColliderType.Box:
                    initializer.Init(BoxColliderEntityComponent.From(_boxColliderSize, _boxColliderCentre));
                    break;
                
                case ColliderType.Circle:
                    initializer.Init(CircleColliderEntityComponent.From(_circleColliderRadius, _circleColliderCentre));
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException($"Unknown {_colliderType}");
            }
        }
    }

    public enum ColliderType
    {
        Box,
        Circle
    }
}