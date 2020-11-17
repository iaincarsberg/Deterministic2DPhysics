using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Svelto.ECS.Tests.ECS
{
    [TestFixture(0u, 100)]
    [TestFixture(20u, 100)]
    [TestFixture(123u, 100)]
    class EntityCollectionTests
    {
        public EntityCollectionTests(uint idStart, int entityCountPerGroup)
        {
            this._idStart             = idStart;
            this._entityCountPerGroup = entityCountPerGroup;
        }

        readonly uint   _idStart;
        readonly int    _entityCountPerGroup;

        EnginesRoot                       _enginesRoot;
        IEntityFactory                    _entityFactory;
        IEntityFunctions                  _entityFunctions;
        SimpleEntitiesSubmissionScheduler _simpleSubmissionEntityViewScheduler;
        TestEngine                        _testEngine;

        static   ushort         numberOfGroups = 10;
        static   ExclusiveGroup _group         = new ExclusiveGroup(numberOfGroups);
        readonly ushort         _groupCount    = numberOfGroups;


        [SetUp]
        public void Init()
        {
            _simpleSubmissionEntityViewScheduler = new SimpleEntitiesSubmissionScheduler();
            _enginesRoot                         = new EnginesRoot(_simpleSubmissionEntityViewScheduler);
            _testEngine                          = new TestEngine();

            _enginesRoot.AddEngine(_testEngine);

            _entityFactory   = _enginesRoot.GenerateEntityFactory();
            _entityFunctions = _enginesRoot.GenerateEntityFunctions();

            var id = _idStart;
            for (uint i = 0; i < _groupCount; i++)
            {
                for (int j = 0; j < _entityCountPerGroup; j++)
                {
                    _entityFactory.BuildEntity<TestEntityWithComponentViewAndComponentStruct>(
                        new EGID(id++, _group + i), new object[] {new TestFloatValue(1f), new TestIntValue(1)});
                    
                    _entityFactory.BuildEntity<TestEntityViewComponentWithString>(
                        new EGID(id++, _group + i), new object[] {new TestStringValue("test")});
                }
            }

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            foreach (var ((buffer, count), exclusiveGroupStruct) in _testEngine
                                                                   .entitiesDB.QueryEntities<TestEntityViewStruct>())
            {
                for (int i = 0; i < count; i++)
                {
                    buffer[i].TestFloatValue.Value += buffer[i].ID.entityID;
                    buffer[i].TestIntValue.Value   += (int) buffer[i].ID.entityID;
                }
            }

            foreach (var ((buffer, count), exclusiveGroupStruct) in _testEngine
                                                                   .entitiesDB.QueryEntities<TestEntityStruct>())
            {
                for (int i = 0; i < count; i++)
                {
                    buffer[i].floatValue = 1 + buffer[i].ID.entityID;
                    buffer[i].intValue   = 1 + (int) buffer[i].ID.entityID;
                }
            }
            
            foreach (var ((buffer, count), exclusiveGroupStruct) in _testEngine
                .entitiesDB.QueryEntities<TestEntityViewComponentString>())
            {
                for (int i = 0; i < count; i++)
                {
                    buffer[i].TestStringValue.Value = (1 + buffer[i].ID.entityID).ToString();
                }
            }
        }

        [TestCase(Description = "Test EntityCollection<T> QueryEntities")]
        public void TestEntityCollection1()
        {
            void TestNotAcceptedEntityComponent()
            {
                _entityFactory.BuildEntity<TestEntityViewComponentWithCustomStruct>(
                    new EGID(0, _group), new object[] {new TestCustomStructWithString("test")});
            }

            Assert.Throws<TypeInitializationException>(TestNotAcceptedEntityComponent);
        }

        [TestCase(Description = "Test EntityCollection<T> ToBuffer ToManagedArray")]
        public void TestEntityCollection1ToBufferToManagedArray()
        {
            for (uint i = 0; i < _groupCount; i++)
            {
                var (entityViewsManagedArray, count) = _testEngine.entitiesDB.QueryEntities<TestEntityViewStruct>(_group + i);

                // can't get a native array from a managed buffer
                Assert.Throws<NotImplementedException>(() => entityViewsManagedArray.ToNativeArray(out _));

                for (int j = 0; j < count; j++)
                {
                    Assert.AreEqual(entityViewsManagedArray[j].ID.entityID + 1, entityViewsManagedArray[j].TestFloatValue.Value);
                    Assert.AreEqual(entityViewsManagedArray[j].ID.entityID + 1, entityViewsManagedArray[j].TestIntValue.Value);
                }
            }
        }

        [TestCase(Description = "Test EntityCollection<T> ToBuffer ToNativeArray")]
        public void TestEntityCollection1ToBufferToNativeArray()
        {
            for (uint i = 0; i < _groupCount; i++)
            {
                var (entityComponents, count)       = _testEngine.entitiesDB.QueryEntities<TestEntityStruct>(_group + i);
                
                // can't get a managed array from a native buffer
                Assert.Throws<NotImplementedException>(() => entityComponents.ToManagedArray());                

                for (int j = 0; j < count; j++)
                {
                    ref var entity = ref entityComponents[j];
                    Assert.AreEqual(entityComponents[j].ID.entityID + 1, entity.floatValue);
                    Assert.AreEqual(entityComponents[j].ID.entityID + 1, entity.intValue);
                }
            }
        }

        [TestCase(Description = "Test EntityCollection<T> String")]
        public void TestEntityCollection1WithString()
        {
            for (uint i = 0; i < _groupCount; i++)
            {
                var (entityViews, count) = _testEngine.entitiesDB.QueryEntities<TestEntityViewComponentString>(_group + i);

                for (int j = 0; j < count; j++)
                {
                    Assert.AreEqual((entityViews[j].ID.entityID + 1).ToString(), entityViews[j].TestStringValue.Value);
                }
            }
        }
    }
}