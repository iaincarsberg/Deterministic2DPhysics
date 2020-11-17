using Physics.Core.EntityComponents;
using Svelto.Common;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core.Engines
{
    
    [Sequenced(nameof(PhysicsEngineNames.PositionSyncEngine))]
    public class PositionSyncEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        private readonly IEngineScheduler _engineScheduler;
        private readonly ExclusiveGroupStruct _group;
        
        public string Name => nameof(PositionSyncEngine);
        public EntitiesDB entitiesDB { get; set; }

        public PositionSyncEngine(IEngineScheduler engineScheduler, ExclusiveGroupStruct group)
        {
            _group = group;
            _engineScheduler = engineScheduler;
        }

        public void Ready()
        {
            _engineScheduler.RegisterScheduledPhysicsEngine(this);
        }

        public void Execute(ulong tick)
        {
            var (transforms, egids, count) = entitiesDB.QueryEntities<TransformEntityComponent, EGIDComponent>(_group);

            foreach (var ((groupedTransforms, groupedEgids, groupedCount), _) in entitiesDB.QueryEntities<TransformEntityComponent, EGIDComponent>(GameGroups.TransformGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    ref var transform = ref transforms[i];
                    ref var egid = ref egids[i];
                    
                    for (var groupedIndex = 0; groupedIndex < groupedCount; groupedIndex++)
                    {
                        ref var groupedTransform = ref groupedTransforms[groupedIndex];
                        ref var groupedEgid = ref groupedEgids[groupedIndex];
                    
                        if (egid.ID.entityID.Equals(groupedEgid.ID.entityID))
                        {
                            transform = groupedTransform;
                        }
                    }
                }
            }
        }
    }
}