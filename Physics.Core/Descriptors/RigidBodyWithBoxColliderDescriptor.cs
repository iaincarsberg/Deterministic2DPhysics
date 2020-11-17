using Physics.Core.EntityComponents;
using Svelto.ECS;
using Svelto.ECS.Serialization;

namespace Physics.Core.Descriptors
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