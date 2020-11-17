using NUnit.Framework;

namespace Svelto.ECS.Tests.ECS.Filters
{
    [TestFixture]
    public class FilterGroupsTests
    {
        [SetUp]
        public void Init()
        {
            _scheduler = new SimpleEntitiesSubmissionScheduler();
            _root = new EnginesRoot(_scheduler);
            _factory = _root.GenerateEntityFactory();
            _engine = new TestEngine();
            _root.AddEngine(_engine);
        }

        [TearDown]
        public void TearDown()
        {
            _root.Dispose();
        }

        [Test]
        public void Test_Removing_Last_Added_Entity_And_Adding_It_Back()
        {
            var egid0 = _factory.BuildEntity<TestEntityDescriptor>(0, GroupA).EGID;
            var egid1 = _factory.BuildEntity<TestEntityDescriptor>(1, GroupA).EGID;
            var egid2 = _factory.BuildEntity<TestEntityDescriptor>(2, GroupA).EGID;
            var egid3 = _factory.BuildEntity<TestEntityDescriptor>(3, GroupA).EGID;

            _scheduler.SubmitEntities();

            var filter = _engine.entitiesDB.GetFilters().CreateOrGetFilterForGroup<TestEntityStruct>(FilterIdA, GroupA);
            var mapper = _engine.entitiesDB.QueryMappedEntities<TestEntityStruct>(GroupA);

            filter.Add(egid0.entityID, mapper);
            filter.Add(egid1.entityID, mapper);
            filter.Add(egid2.entityID, mapper);
            filter.Add(egid3.entityID, mapper);
            Assert.AreEqual(4, filter.filteredIndices.Count());

            filter.Remove(egid3.entityID);
            Assert.AreEqual(3, filter.filteredIndices.Count());

            filter.Add(egid3.entityID, mapper);
            //Asert count.
            Assert.AreEqual(4, filter.filteredIndices.Count());
            // Assert EGID.entityID
            Assert.AreEqual(3, filter.filteredIndices.Get(3));
        }

        SimpleEntitiesSubmissionScheduler _scheduler;
        EnginesRoot _root;
        IEntityFactory _factory;
        IEntityFunctions _functions;
        TestEngine _engine;

        class TestEntityDescriptor : GenericEntityDescriptor<TestEntityStruct> {}

        static ExclusiveGroup GroupA = new ExclusiveGroup();
        const int FilterIdA = 0;
    }
}