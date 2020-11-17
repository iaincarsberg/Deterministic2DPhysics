using Physics.Core.EntityComponents;
using Svelto.DataStructures;
using Svelto.ECS;

namespace Physics.Core.Descriptors
{
    public class TransformDescriptor : GenericEntityDescriptor<TransformEntityComponent, EGIDComponent>
    {
        public static (NB<TransformEntityComponent>, NB<EGIDComponent>, int) Query(EntitiesDB entitiesDb)
        {
            var (transforms, egids, count) = entitiesDb.QueryEntities<TransformEntityComponent, EGIDComponent>(GameGroups.RigidBodyWithBoxCollider);
        
            return (transforms, egids, count);
        }
    }
}