using System.Diagnostics;
using FixedMaths.Core;
using Graphics.Core;
using Physics.Core;
using Physics.Core.Builders;
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

        public void Execute()
        {
            _running = true;

            var clock = new Stopwatch();
            clock.Restart();

            EcsInit();
            _graphics?.Init();

            AddEntities();

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

        private void AddEntities()
        {
            // Make a simple bounding box
            RigidBodyWithBoxColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(-100)))
                .SetBoxColliderSize(FixedPointVector2.From(100, 5))
                .SetIsKinematic(true)
                .Build(_entityFactory, 0);
            
            RigidBodyWithBoxColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(100)))
                .SetBoxColliderSize(FixedPointVector2.From(100, 5))
                .SetIsKinematic(true)
                .Build(_entityFactory, 1);
            
            RigidBodyWithBoxColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(-100), FixedPoint.From(0)))
                .SetBoxColliderSize(FixedPointVector2.From(5, 100))
                .SetIsKinematic(true)
                .Build(_entityFactory, 2);
            
            RigidBodyWithBoxColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(100), FixedPoint.From(0)))
                .SetBoxColliderSize(FixedPointVector2.From(5, 100))
                .SetIsKinematic(true)
                .Build(_entityFactory, 3);
            
            // Add some bounding boxes
            AddEntity(4, FixedPointVector2.From(FixedPoint.From(-30), FixedPoint.From(0)), FixedPointVector2.Down, FixedPoint.From(3), FixedPointVector2.From(10, 10));
            AddEntity(5, FixedPointVector2.From(FixedPoint.From(-35), FixedPoint.From(-50)), FixedPointVector2.Up, FixedPoint.From(5), FixedPointVector2.From(10, 10));
            AddEntity(6, FixedPointVector2.From(FixedPoint.From(-30), FixedPoint.From(50)), FixedPointVector2.Up, FixedPoint.From(3), FixedPointVector2.From(10, 10));
            AddEntity(7, FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(50)), FixedPointVector2.Right, FixedPoint.From(3), FixedPointVector2.From(10, 10));
            AddEntity(8, FixedPointVector2.From(FixedPoint.From(40), FixedPoint.From(-90)), FixedPointVector2.From(1, 1).Normalize(), FixedPoint.From(10), FixedPointVector2.From(3, 3));
            AddEntity(9, FixedPointVector2.From(FixedPoint.From(40), FixedPoint.From(-60)), FixedPointVector2.From(1, 1).Normalize(), FixedPoint.From(10), FixedPointVector2.From(3, 3));
            AddEntity(10, FixedPointVector2.From(FixedPoint.From(40), FixedPoint.From(-30)), FixedPointVector2.From(1, 1).Normalize(), FixedPoint.From(3), FixedPointVector2.From(3, 3));
            
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
        
        private void AddEntity(uint egid, FixedPointVector2 position, FixedPointVector2 direction, FixedPoint speed, FixedPointVector2 boxColliderSize)
        {
            RigidBodyWithBoxColliderBuilder.Create()
                .SetPosition(position)
                .SetDirection(direction)
                .SetSpeed(speed)
                .SetBoxColliderSize(boxColliderSize)
                .Build(_entityFactory, egid);
        }
    }
}