using Physics.Core.EntityComponents;
using Svelto.ECS;
using Svelto.ECS.Serialization;

namespace Physics.Core.Descriptors
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