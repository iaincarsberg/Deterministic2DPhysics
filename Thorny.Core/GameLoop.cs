using System;
using System.Diagnostics;
using FixedMaths.Core;
using Graphics.Core;
using Physics.Core;
using Svelto.ECS;
using Thorny.Core.Engines;
using GameGroups = Thorny.Common.GameGroups;
using PhysicsGameGroups = Physics.Core.GameGroups;

namespace Thorny.Core
{
    public class GameLoop : IGameLoop
    {
        private const uint TicksPerMillisecond = 1000;
        private const uint TicksPerSecond = 1000 * TicksPerMillisecond;

        private const uint DefaultPhysicsSimulationsPerSecond = 30;
        private const uint DefaultGraphicsFramesPerSecond = 60;
        private const float DefaultSimulationSpeed = 1.0f;

        private IGraphics _graphics;
        private uint _graphicsFramesPerSecond;
        private IInput _input;
        private uint _physicsSimulationsPerSecond;
        private FixedPoint _physicsSimulationsPerSecondFixedPoint;
        private bool _running;
        private readonly EngineScheduler _scheduler;
        private IEntityFactory _entityFactory;
        private SimpleEntitiesSubmissionScheduler _simpleSubmissionEntityViewScheduler;
        private PhysicsCoreHandle _physicsCoreHandle;
        private float _simulationSpeed;
        private Action<IEntityFactory, SimpleEntitiesSubmissionScheduler> _onBeforeMainGameLoop = (factory, scheduler) => { };

        public GameLoop()
        {
            SetPhysicsSimulationsPerSecond(DefaultPhysicsSimulationsPerSecond);
            SetGraphicsFramesPerSecond(DefaultGraphicsFramesPerSecond);
            SetSimulationSpeed(DefaultSimulationSpeed);
            
            _scheduler = new EngineScheduler();
        }

        public void Stop()
        {
            _running = false;
        }

        public IGameLoop AddGraphics(IGraphics graphics)
        {
            _graphics = graphics;
            return this;
        }

        public IGameLoop AddInput(IInput input)
        {
            _input = input;
            return this;
        }

        public IGameLoop SetPhysicsSimulationsPerSecond(uint frequency)
        {
            _physicsSimulationsPerSecond = TicksPerSecond / frequency;
            _physicsSimulationsPerSecondFixedPoint = FixedPoint.From(frequency);
            return this;
        }

        public IGameLoop SetGraphicsFramesPerSecond(uint frequency)
        {
            _graphicsFramesPerSecond = TicksPerSecond / frequency;
            return this;
        }

        public IGameLoop SetUncappedGraphicsFramesPerSecond()
        {
            _graphicsFramesPerSecond = 1;//TicksPerSecond;
            return this;
        }
        
        public IGameLoop SetSimulationSpeed(float simulationSpeed)
        {
            _simulationSpeed = simulationSpeed;
            return this;
        }

        public IGameLoop SetOnBeforeMainGameLoopAction(Action<IEntityFactory, SimpleEntitiesSubmissionScheduler> action)
        {
            _onBeforeMainGameLoop = action;
            return this;
        }

        public void Execute()
        {
            _running = true;

            var clock = new Stopwatch();
            clock.Restart();

            EcsInit();
            _graphics?.Init();

            _onBeforeMainGameLoop(_entityFactory, _simpleSubmissionEntityViewScheduler);

            var physicsAction = ScheduledAction.From(_scheduler.ExecutePhysics, _physicsSimulationsPerSecond, true);

            var graphicsAction = ScheduledAction.From(tick =>
            {
                _graphics?.RenderStart();
                _scheduler.ExecuteGraphics(FixedPoint.From((float)physicsAction.RemainingDelta / _physicsSimulationsPerSecond), physicsAction.CurrentTick);
                _graphics?.RenderEnd();
                
            }, _graphicsFramesPerSecond, false);

            var lastElapsedTicks = clock.ElapsedTicks;
            var gameTick = 0UL;
            while (_running)
            {
                _input?.PollEvents(this);
                
                // Calculate the time delta
                var elapsedTicks = clock.ElapsedTicks;
                gameTick += (ulong)((elapsedTicks - lastElapsedTicks) * _simulationSpeed);
                lastElapsedTicks = elapsedTicks;

                // Execute simulation ticks
                graphicsAction.Tick(gameTick);
                physicsAction.Tick(gameTick);
            }

            _graphics?.Cleanup();
        }

        private void EcsInit()
        {
            _simpleSubmissionEntityViewScheduler = new SimpleEntitiesSubmissionScheduler();
            var enginesRoot = new EnginesRoot(_simpleSubmissionEntityViewScheduler);

            _entityFactory = enginesRoot.GenerateEntityFactory();

            _physicsCoreHandle = PhysicsCore.RegisterTo(enginesRoot, _scheduler, _physicsSimulationsPerSecondFixedPoint, GameGroups.Debug);

            if (_graphics != null)
            {
                enginesRoot.AddEngine(new DebugPhysicsDrawEngine(_scheduler, _graphics));
            }
        }
    }
}