using Svelto.ECS;
using Svelto.ECS.Serialization;
using SveltoDeterministic2DPhysicsDemo.Physics.EntityComponents;

namespace SveltoDeterministic2DPhysicsDemo.Physics.Descriptors
{
    public class RigidBodyWithCircleColliderDescriptor : SerializableEntityDescriptor<RigidBodyWithCircleColliderDescriptor.RigidBodyWithCircleColliderImplementation>
    {
        [HashName("RigidBodyWithCircleColliderDescriptorV1")]
        public class RigidBodyWithCircleColliderImplementation : ExtendibleEntityDescriptor<RigidBodyDescriptor.RigidBodyImplementation>
        {
            public RigidBodyWithCircleColliderImplementation() : base(new IComponentBuilder[]
            {
                new ComponentBuilder<CircleColliderEntityComponent>()
            })
            {}
        }
    }
}