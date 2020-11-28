using FixedMaths.Core;
using FixedMaths.Core.Data;
using FixedMaths.FileSystem;
using Graphics.SDL2Driver;
using Physics.Core.Builders;
using Svelto.ECS;
using Thorny.Core;

namespace DemoGame
{
    internal static class Program
    {
        private static void Main()
        {
            var repository = ProcessedTableRepository.From("ProcessedTableData");
            ProcessedTableService.CreateInstance(repository);
            
            var sdl2 = new Sdl2Driver();
            new GameLoop()
                .AddGraphics(sdl2)
                .AddInput(sdl2)
                .SetPhysicsSimulationsPerSecond(30)
                .SetSimulationSpeed(1.0f)
                .SetUncappedGraphicsFramesPerSecond()
                .SetOnBeforeMainGameLoopAction(AddLoadsOfEntities)
                //.SetOnBeforeMainGameLoopAction(AddEntities)
                .Execute();
        }
        
        private static void AddEntities(IEntityFactory entityFactory, SimpleEntitiesSubmissionScheduler simpleSubmissionEntityViewScheduler)
        {
            // Make a simple bounding box
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(-100)))
                .SetBoxCollider(FixedPointVector2.From(100, 5))
                .SetIsKinematic(true)
                .Build(entityFactory, 0);
            
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(100)))
                .SetBoxCollider(FixedPointVector2.From(100, 5))
                .SetIsKinematic(true)
                .Build(entityFactory, 1);
            
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(-100), FixedPoint.From(0)))
                .SetBoxCollider(FixedPointVector2.From(5, 100))
                .SetIsKinematic(true)
                .Build(entityFactory, 2);
            
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(100), FixedPoint.From(0)))
                .SetBoxCollider(FixedPointVector2.From(5, 100))
                .SetIsKinematic(true)
                .Build(entityFactory, 3);
            
            // Add some bounding boxes
            AddBoxColliderEntity(entityFactory, 4, FixedPointVector2.From(FixedPoint.From(-30), FixedPoint.From(0)), FixedPointVector2.Down, FixedPoint.From(3), FixedPointVector2.From(10, 10));
            AddBoxColliderEntity(entityFactory, 5, FixedPointVector2.From(FixedPoint.From(-35), FixedPoint.From(-50)), FixedPointVector2.Up, FixedPoint.From(5), FixedPointVector2.From(10, 10));
            AddBoxColliderEntity(entityFactory, 6, FixedPointVector2.From(FixedPoint.From(-30), FixedPoint.From(50)), FixedPointVector2.Up, FixedPoint.From(3), FixedPointVector2.From(10, 10));
            AddBoxColliderEntity(entityFactory, 7, FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(50)), FixedPointVector2.Right, FixedPoint.From(3), FixedPointVector2.From(10, 10));
            AddBoxColliderEntity(entityFactory, 8, FixedPointVector2.From(FixedPoint.From(40), FixedPoint.From(-90)), FixedPointVector2.From(1, 1).Normalize(), FixedPoint.From(10), FixedPointVector2.From(3, 3));
            AddBoxColliderEntity(entityFactory, 9, FixedPointVector2.From(FixedPoint.From(40), FixedPoint.From(-60)), FixedPointVector2.From(1, 1).Normalize(), FixedPoint.From(10), FixedPointVector2.From(3, 3));
            AddBoxColliderEntity(entityFactory, 10, FixedPointVector2.From(FixedPoint.From(40), FixedPoint.From(-30)), FixedPointVector2.From(1, 1).Normalize(), FixedPoint.From(3), FixedPointVector2.From(3, 3));
            AddCircleColliderEntity(entityFactory, 11, FixedPointVector2.From(FixedPoint.From(60), FixedPoint.From(-30)), FixedPointVector2.From(1, 1).Normalize(), FixedPoint.From(3), FixedPoint.From(3));
            
            simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
        
        private static void AddBoxColliderEntity(IEntityFactory entityFactory, uint egid, FixedPointVector2 position, FixedPointVector2 direction, FixedPoint speed, FixedPointVector2 boxColliderSize)
        {
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(position)
                .SetDirection(direction)
                .SetSpeed(speed)
                .SetBoxCollider(boxColliderSize)
                .Build(entityFactory, egid);
        }
        
        private static void AddCircleColliderEntity(IEntityFactory entityFactory, uint egid, FixedPointVector2 position, FixedPointVector2 direction, FixedPoint speed, FixedPoint radius)
        {
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(position)
                .SetDirection(direction)
                .SetSpeed(speed)
                .SetCircleCollider(radius)
                .Build(entityFactory, egid);
        }
        
        
        private static void AddLoadsOfEntities(IEntityFactory entityFactory, SimpleEntitiesSubmissionScheduler simpleSubmissionEntityViewScheduler)
        {
            // Make a simple bounding box
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(-100)))
                .SetBoxCollider(FixedPointVector2.From(1000, 5))
                .SetIsKinematic(true)
                .Build(entityFactory, 0);
            
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(100)))
                .SetBoxCollider(FixedPointVector2.From(1000, 5))
                .SetIsKinematic(true)
                .Build(entityFactory, 1);
            
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(-1000), FixedPoint.From(0)))
                .SetBoxCollider(FixedPointVector2.From(5, 1000))
                .SetIsKinematic(true)
                .Build(entityFactory, 2);
            
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(1000), FixedPoint.From(0)))
                .SetBoxCollider(FixedPointVector2.From(5, 1000))
                .SetIsKinematic(true)
                .Build(entityFactory, 3);
            
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(-220)))
                .SetCircleCollider(FixedPoint.From(100))
                .SetIsKinematic(true)
                .Build(entityFactory, 4);
            
            RigidBodyWithColliderBuilder.Create()
                .SetPosition(FixedPointVector2.From(FixedPoint.From(0), FixedPoint.From(220)))
                .SetCircleCollider(FixedPoint.From(100))
                .SetIsKinematic(true)
                .Build(entityFactory, 5);
            
            // Add some bouncing colliders
            const int colliders = 1;
            for (var i = 0u; i < colliders; i++)
            {
                RigidBodyWithColliderBuilder.Create()
                    .SetPosition(FixedPointVector2.From(((int)i * 8) - (colliders * 4), (int)i))
                    .SetDirection(FixedPointVector2.Down)
                    .SetSpeed(FixedPoint.From(5))
                    .SetBoxCollider(FixedPointVector2.From(3, 3))
                    .Build(entityFactory, i + 6);
            }
            
            for (var i = 0u; i < colliders; i++)
            {
                RigidBodyWithColliderBuilder.Create()
                    .SetPosition(FixedPointVector2.From(((int)i * 8) - (colliders * 4), (int)i + 50))
                    .SetDirection(FixedPointVector2.Down)
                    .SetSpeed(FixedPoint.From(5))
                    .SetCircleCollider(FixedPoint.From(3))
                    .Build(entityFactory, i + 6);
                
                RigidBodyWithColliderBuilder.Create()
                    .SetPosition(FixedPointVector2.From(((int)i * 8) - (colliders * 4), (int)-i - 50))
                    .SetDirection(FixedPointVector2.Up)
                    .SetSpeed(FixedPoint.From(2))
                    .SetCircleCollider(FixedPoint.From(2))
                    .Build(entityFactory, i + 6 + colliders);
            }
            
            simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
    }
}