﻿using Physics.Core.EntityComponents;
using Svelto.Common;
using Svelto.ECS;
using Thorny.Common;

namespace Physics.Core.Engines
{
    [Sequenced(nameof(PhysicsEngineNames.ClearPerFrameStateEngine))]
    public class ClearPerFrameStateEngine : IQueryingEntitiesEngine, IScheduledPhysicsEngine
    {
        private readonly IEngineScheduler _engineScheduler;
        public EntitiesDB entitiesDB { get; set; }
        
        public ClearPerFrameStateEngine(IEngineScheduler engineScheduler)
        {
            _engineScheduler = engineScheduler;
        }
        
        public void Ready()
        {
            _engineScheduler.RegisterScheduledPhysicsEngine(this);
        }

        public void Execute(ulong tick)
        {
            foreach (var ((manifolds, count), _) in entitiesDB.QueryEntities<CollisionManifoldEntityComponent>(GameGroups.RigidBodyGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    manifolds[i] = CollisionManifoldEntityComponent.Default;
                }
            }
        }
    }
}