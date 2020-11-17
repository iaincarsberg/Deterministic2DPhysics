using Physics.Core.EntityComponents;
using Svelto.ECS;
using Svelto.ECS.Serialization;

namespace Physics.Core.Descriptors
{
    public class RigidBodyDescriptor : SerializableEntityDescriptor<RigidBodyDescriptor.RigidBodyImplementation>
    {
        [HashName("RigidBodyDescriptorV1")]
        public class RigidBodyImplementation : ExtendibleEntityDescriptor<TransformDescriptor>
        {
            public RigidBodyImplementation() : base(new IComponentBuilder[]
            {
                new ComponentBuilder<RigidbodyEntityComponent>(),
                new ComponentBuilder<CollisionManifoldEntityComponent>(), 
            })
            {}
        }
    }
}