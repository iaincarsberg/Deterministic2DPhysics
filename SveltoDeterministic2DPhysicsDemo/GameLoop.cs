using System;
using System.Diagnostics;
using Svelto.ECS;
using SveltoDeterministic2DPhysicsDemo.Graphics;
using SveltoDeterministic2DPhysicsDemo.Maths;
using SveltoDeterministic2DPhysicsDemo.Physics;
using SveltoDeterministic2DPhysicsDemo.Physics.Engines;


namespace SveltoDeterministic2DPhysicsDemo
{
    public class GameLoop : IGameLoop
    {
        private const uint TicksPerMillisecond = 1000;
        private const uint TicksPerSecond = 1000 * TicksPerMillisecond;

        private const uint DefaultPhysicsSimulationsPerSecond = 30;
        private const uint DefaultGraphicsFramesPerSecond = 60;
        private const float DefaultSimulationSpeed = 1.0f;

        
        private readonly EngineScheduler _scheduler;
        private readonly EngineSchedulerReporter _schedulerReporter;
        
        private IGraphics _graphics;
        private uint _graphicsFramesPerSecond;
        private IInput _input;
        private uint _physicsSimulationsPerSecond;
        private FixedPoint _physicsSimulationsPerSecondFixedPoint;
        private bool _running;
        private IEntityFactory _entityFactory;
        private SimpleEntitiesSubmissionScheduler _simpleSubmissionEntityViewScheduler;
        private float _simulationSpeed;
        private Action<IEntityFactory, SimpleEntitiesSubmissionScheduler> _onBeforeMainGameLoop = (factory, scheduler) => { };

        public GameLoop()
        {
            SetPhysicsSimulationsPerSecond(DefaultPhysicsSimulationsPerSecond);
            SetGraphicsFramesPerSecond(DefaultGraphicsFramesPerSecond);
            SetSimulationSpeed(DefaultSimulationSpeed);

            _schedulerReporter = new EngineSchedulerReporter();
            _scheduler = new EngineScheduler(_schedulerReporter);
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
            if (!_graphics?.Init() ?? true)
            {
                Console.WriteLine("Graphics exist, but failed to init.");
                return;
            }
            
            _onBeforeMainGameLoop(_entityFactory, _simpleSubmissionEntityViewScheduler);

            var physicsAction = ScheduledAction.From(_scheduler.ExecutePhysics, _physicsSimulationsPerSecond, true);
            
            var graphicsAction = ScheduledAction.From(tick =>
            {
                _graphics?.RenderStart();
                _scheduler.ExecuteGraphics(FixedPoint.From((float)physicsAction.RemainingDelta / _physicsSimulationsPerSecond), physicsAction.CurrentTick);

                if (_graphics != null)
                {
                    _schedulerReporter.Report(_graphics);
                }

                _graphics?.RenderEnd();
                
            }, _graphicsFramesPerSecond, false);
            
            var perSecond = ScheduledAction.From(tick =>
            {
                if (_graphics != null)
                {
                    _schedulerReporter.Reset();
                }
            }, TicksPerSecond, true);

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
                graphicsAction.Tick((ulong)elapsedTicks);
                physicsAction.Tick(gameTick);
                perSecond.Tick(gameTick);
            }

            _graphics?.Cleanup();
        }

        private void EcsInit()
        {
            _simpleSubmissionEntityViewScheduler = new SimpleEntitiesSubmissionScheduler();
            var enginesRoot = new EnginesRoot(_simpleSubmissionEntityViewScheduler);

            _entityFactory = enginesRoot.GenerateEntityFactory();

            PhysicsCore.RegisterTo(enginesRoot, _scheduler, _physicsSimulationsPerSecondFixedPoint);
            
            if (_graphics != null)
            {
                enginesRoot.AddEngine(new DebugPhysicsDrawEngine(_scheduler, _graphics));
            }
        }
    }
}