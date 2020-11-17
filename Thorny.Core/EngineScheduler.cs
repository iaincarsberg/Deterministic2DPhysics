using System.Collections.Generic;
using FixedMaths.Core;
using Thorny.Common;

namespace Thorny.Core
{
    public class EngineScheduler : IEngineScheduler
    {
        private readonly List<IScheduledPhysicsEngine> _scheduledPhysicsEngines;
        private readonly List<IScheduledGraphicsEngine> _scheduledGraphicsEngine;
        
        public EngineScheduler()
        {
            _scheduledPhysicsEngines = new List<IScheduledPhysicsEngine>();
            _scheduledGraphicsEngine = new List<IScheduledGraphicsEngine>();
        }

        public void RegisterScheduledPhysicsEngine(IScheduledPhysicsEngine scheduled)
        {
            _scheduledPhysicsEngines.Add(scheduled);
        }
        
        
        public void RegisterScheduledGraphicsEngine(IScheduledGraphicsEngine scheduledGraphicsEngine)
        {
            _scheduledGraphicsEngine.Add(scheduledGraphicsEngine);
        }

        public void ExecutePhysics(ulong tick)
        {
            foreach (var engine in _scheduledPhysicsEngines)
            {
                engine.Execute(tick);
            }
        }

        public void ExecuteGraphics(FixedPoint delta, ulong physicsTick)
        {
            foreach (var engine in _scheduledGraphicsEngine)
            {
                engine.Draw(delta, physicsTick);
            }
        }
    }
}