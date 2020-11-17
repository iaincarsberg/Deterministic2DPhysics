using NUnit.Framework;
using Svelto.DataStructures;
using Svelto.ECS.Experimental;

namespace Svelto.ECS.Tests.ECS
{
    [TestFixture]
    public class EnginesRoot_GenericEntityFunctionsTests
    {
        [SetUp]
        public void Init()
        {
            _scheduler   = new SimpleEntitiesSubmissionScheduler();
            _enginesRoot = new EnginesRoot(_scheduler);
            _factory = _enginesRoot.GenerateEntityFactory();
            _functions = _enginesRoot.GenerateEntityFunctions();
            _neverdoThisEngine = new TestEngine();

            _enginesRoot.AddEngine(_neverdoThisEngine);
        }

        [TearDown]
        public void Cleanup()
        {
            _functions.RemoveEntitiesFromGroup(GroupA);
            _functions.RemoveEntitiesFromGroup(GroupB);
            _scheduler.SubmitEntities();
        }

        [Test]
        public void TestRemoveEntityWithEntityIdAndGroup()
        {
            CreateTestEntity(0, GroupA);
            CreateTestEntity(1, GroupA);
            _scheduler.SubmitEntities();

            _functions.RemoveEntity<TestEntityWithComponentViewAndComponentStruct>(0, GroupA);
            _scheduler.SubmitEntities();

            var exists = _neverdoThisEngine.entitiesDB.Exists<TestEntityStruct>(0, GroupA);
            Assert.IsFalse(exists, "Entity should be removed from target group");

            var count = _neverdoThisEngine.entitiesDB.Count<TestEntityStruct>(GroupA);
            Assert.AreEqual(1, count, "Other entities should not be removed");

            void RemoveEntityNotFound()
            {
                _functions.RemoveEntity<TestEntityWithComponentViewAndComponentStruct>(0, GroupA);
                _scheduler.SubmitEntities();
            }

            Assert.Throws<ECSException>(RemoveEntityNotFound, "When removing non created entities an exception should be thrown");
        }

        [Test]
        public void TestRemoveEntityWithEgid()
        {
            CreateTestEntity(0, GroupA);
            CreateTestEntity(1, GroupA);
            _scheduler.SubmitEntities();

            var egid = new EGID(0, GroupA);
            _functions.RemoveEntity<TestEntityWithComponentViewAndComponentStruct>(egid);
            _scheduler.SubmitEntities();

            var exists = _neverdoThisEngine.entitiesDB.Exists<TestEntityStruct>(0, GroupA);
            Assert.IsFalse(exists, "Entity should be removed from target group");

            var count = _neverdoThisEngine.entitiesDB.Count<TestEntityStruct>(GroupA);
            Assert.AreEqual(1, count, "Other entities should not be removed");

            void RemoveEntityNotFound()
            {
                _functions.RemoveEntity<TestEntityWithComponentViewAndComponentStruct>(new EGID(0, GroupA));
                _scheduler.SubmitEntities();
            }

            Assert.Throws<ECSException>(RemoveEntityNotFound, "When removing non created entities an exception should be thrown");
        }

        [Test]
        public void TestRemoveGroupAndEntities()
        {
            CreateTestEntity(0, GroupA);
            CreateTestEntity(1, GroupA);
            CreateTestEntity(1, GroupB);
            _scheduler.SubmitEntities();

            _functions.RemoveEntitiesFromGroup(GroupA);
            _scheduler.SubmitEntities();

            var query = new QueryGroups(GroupAB);
            
            var entityCount = query.Count<TestEntityStruct>(_neverdoThisEngine.entitiesDB);
            Assert.AreEqual(1, entityCount, "Entities in the target group should be removed");
        }

        [Test]
        public void TestSwapEntitiesInGroup()
        {
            // todo: Test what happens when source group is empty?

            CreateTestEntity(0, GroupA, 0);
            CreateTestEntity(1, GroupA, 1);
            CreateTestEntity(2, GroupA, 2);
            _scheduler.SubmitEntities();

            _functions.SwapEntitiesInGroup<TestEntityWithComponentViewAndComponentStruct>(GroupA, GroupB);
            _scheduler.SubmitEntities();

            var countA = _neverdoThisEngine.entitiesDB.Count<TestEntityStruct>(GroupA);
            Assert.AreEqual(0, countA, "Source group should be empty after swap");

            var (componentsB, countB) = _neverdoThisEngine.entitiesDB.QueryEntities<TestEntityStruct>(GroupB);
            Assert.AreEqual(3, countB, "All entities should exist in target group after swap");
            Assert.AreEqual(2, componentsB[2].intValue, "Values in components should be copied after swap");

            // todo: What is the expected behaviour when source entity id is already used in target group?
        }

        [Test]
        public void TestSwapEntityFromEgidToEgid()
        {
            CreateTestEntity(0, GroupA);

            _scheduler.SubmitEntities();

            var fromEgid = new EGID(0, GroupA);
            var toEgid = new EGID(1, GroupB);
            _functions.SwapEntityGroup<TestEntityWithComponentViewAndComponentStruct>(fromEgid, toEgid);

            _scheduler.SubmitEntities();

            var (componentB, componentViewB, countB) = _neverdoThisEngine.entitiesDB.QueryEntities<TestEntityStruct, TestEntityViewStruct>(GroupB);

            Assert.AreEqual(1, countB, "An entity should exist in target Group");
            Assert.AreEqual(toEgid.entityID, componentB[0].ID.entityID, "Swapped entity should have the target entityID");
            Assert.AreEqual(1f, componentB[0].floatValue, "Component values should be copied");
            Assert.AreEqual(1, componentB[0].intValue, "Component values should be copied");
            Assert.AreEqual(1f, componentViewB[0].TestFloatValue.Value, "ViewComponent values should be copied");
            Assert.AreEqual(1, componentViewB[0].TestIntValue.Value, "ViewComponent values should be copied");

            var existsA = _neverdoThisEngine.entitiesDB.Exists<TestEntityStruct>(0, GroupA);
            Assert.IsFalse(existsA, "Entity should not be present in source Group anymore");

            void SwapEntityAlreadyExists()
            {
                CreateTestEntity(2, GroupA);
                CreateTestEntity(2, GroupB);
                fromEgid = new EGID(0, GroupA);
                toEgid = new EGID(0, GroupB);
                _functions.SwapEntityGroup<TestEntityWithComponentViewAndComponentStruct>(fromEgid, toEgid);
                _scheduler.SubmitEntities();
            }

            Assert.Throws<ECSException>(SwapEntityAlreadyExists, "When target EGID already exists it should throw an exception");

            void SwapEntityNotFound()
            {
                fromEgid = new EGID(3, GroupA);
                toEgid = new EGID(3, GroupB);
                _functions.SwapEntityGroup<TestEntityWithComponentViewAndComponentStruct>(fromEgid, toEgid);
                _scheduler.SubmitEntities();
            }

            Assert.Throws<ECSException>(SwapEntityNotFound, "When source EGID doesn't exists it should throw an exception");
        }

        EntityComponentInitializer CreateTestEntity(uint entityId, ExclusiveGroupStruct group, int value = 1)
        {
            var initializer = _factory.BuildEntity<TestEntityWithComponentViewAndComponentStruct>
                (entityId, group, new object[] {new TestFloatValue(value), new TestIntValue(value)});
            initializer.Init(new TestEntityStruct(value, value));
            return initializer;
        }

        SimpleEntitiesSubmissionScheduler _scheduler;
        EnginesRoot _enginesRoot;
        IEntityFactory _factory;
        IEntityFunctions _functions;
        TestEngine _neverdoThisEngine;

        static ExclusiveGroup GroupA = new ExclusiveGroup();
        static ExclusiveGroup GroupB = new ExclusiveGroup();
        static FasterList<ExclusiveGroupStruct> GroupAB = new FasterList<ExclusiveGroupStruct>().Add(GroupA).Add(GroupB);
    }
}
