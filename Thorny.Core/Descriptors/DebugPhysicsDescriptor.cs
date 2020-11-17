using Physics.Core.Descriptors;
using Svelto.ECS;

namespace Thorny.Core.Descriptors
{
    public class DebugPhysicsDescriptor : ExtendibleEntityDescriptor<TransformDescriptor>
    {
        public DebugPhysicsDescriptor() : base(new IComponentBuilder[0])
        {
        }
    }
}