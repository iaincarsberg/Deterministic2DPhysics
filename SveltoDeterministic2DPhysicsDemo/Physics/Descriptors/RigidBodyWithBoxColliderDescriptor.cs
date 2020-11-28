using Svelto.ECS;
using Svelto.ECS.Serialization;
using SveltoDeterministic2DPhysicsDemo.Physics.EntityComponents;

namespace SveltoDeterministic2DPhysicsDemo.Physics.Descriptors
{
    public class RigidBodyWithBoxColliderDescriptor : SerializableEntityDescriptor<RigidBodyWithBoxColliderDescriptor.RigidBodyWithBoxColliderImplementation>
    {
        [HashName("RigidBodyWithBoxColliderDescriptorV1")]
        public class RigidBodyWithBoxColliderImplementation : ExtendibleEntityDescriptor<RigidBodyDescriptor.RigidBodyImplementation>
        {
            public RigidBodyWithBoxColliderImplementation() : base(new IComponentBuilder[]
            {
                new ComponentBuilder<BoxColliderEntityComponent>()
            })
            {}
        }
    }
}