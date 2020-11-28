using Svelto.ECS;
using Svelto.ECS.Serialization;
using SveltoDeterministic2DPhysicsDemo.Physics.EntityComponents;

namespace SveltoDeterministic2DPhysicsDemo.Physics.Descriptors
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