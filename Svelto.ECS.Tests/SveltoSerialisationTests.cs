using NUnit.Framework;
using Svelto.DataStructures;
using Svelto.ECS;
using Svelto.ECS.Hybrid;
using Svelto.ECS.Serialization;

namespace Svelto.ECS.Tests.Serialization
{
    [TestFixture]
    public class TestSveltoECSSerialisation
    {
        class NamedGroup1 : NamedExclusiveGroup<NamedGroup1> { }

        [SetUp]
        public void Init()
        {
            _simpleSubmissionEntityViewScheduler = new SimpleEntitiesSubmissionScheduler();
            _enginesRoot                         = new EnginesRoot(_simpleSubmissionEntityViewScheduler);
            _neverDoThisIsJustForTheTest         = new TestEngine();

            _enginesRoot.AddEngine(_neverDoThisIsJustForTheTest);

            _entityFactory   = _enginesRoot.GenerateEntityFactory();
            _entityFunctions = _enginesRoot.GenerateEntityFunctions();
        }

        [TearDown]
        public void Dipose() { _enginesRoot.Dispose(); }

        [TestCase]
        public void TestSerializingToByteArrayRemoveGroup()
        {
            var init = _entityFactory.BuildEntity<SerializableEntityDescriptor>(0, NamedGroup1.Group);
            init.Init(new EntityStructSerialized()
            {
                value = 5
            });
            init.Init(new EntityStructSerialized2()
            {
                value = 4
            });
            init.Init(new EntityStructPartiallySerialized()
            {
                value1 = 3
            });
            init = _entityFactory.BuildEntity<SerializableEntityDescriptor>(1, NamedGroup1.Group);
            init.Init(new EntityStructSerialized()
            {
                value = 4
            });
            init.Init(new EntityStructSerialized2()
            {
                value = 3
            });
            init.Init(new EntityStructPartiallySerialized()
            {
                value1 = 2
            });

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            FasterList<byte> bytes                    = new FasterList<byte>();
            var              generateEntitySerializer = _enginesRoot.GenerateEntitySerializer();
            var              simpleSerializationData  = new SimpleSerializationData(bytes);
            generateEntitySerializer.SerializeEntity(new EGID(0, NamedGroup1.Group), simpleSerializationData
                                                   , (int) SerializationType.Storage);
            generateEntitySerializer.SerializeEntity(new EGID(1, NamedGroup1.Group), simpleSerializationData
                                                   , (int) SerializationType.Storage);

            _entityFunctions.RemoveEntitiesFromGroup(NamedGroup1.Group);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            simpleSerializationData.Reset();

            generateEntitySerializer.DeserializeNewEntity(new EGID(0, NamedGroup1.Group), simpleSerializationData
                                                        , (int) SerializationType.Storage);
            generateEntitySerializer.DeserializeNewEntity(new EGID(1, NamedGroup1.Group), simpleSerializationData
                                                        , (int) SerializationType.Storage);

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized>(0, NamedGroup1.Group).value
              , Is.EqualTo(5));
            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized2>(0, NamedGroup1.Group).value
              , Is.EqualTo(4));
            Assert.That(
                _neverDoThisIsJustForTheTest
                   .entitiesDB.QueryEntity<EntityStructPartiallySerialized>(0, NamedGroup1.Group).value1
              , Is.EqualTo(3));

            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized>(1, NamedGroup1.Group).value
              , Is.EqualTo(4));
            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized2>(1, NamedGroup1.Group).value
              , Is.EqualTo(3));
            Assert.That(
                _neverDoThisIsJustForTheTest
                   .entitiesDB.QueryEntity<EntityStructPartiallySerialized>(1, NamedGroup1.Group).value1
              , Is.EqualTo(2));
        }

        [TestCase]
        public void TestSerializingToByteArrayNewEnginesRoot()
        {
            var init = _entityFactory.BuildEntity<SerializableEntityDescriptor>(0, NamedGroup1.Group);
            init.Init(new EntityStructSerialized()
            {
                value = 5
            });
            init.Init(new EntityStructSerialized2()
            {
                value = 4
            });
            init.Init(new EntityStructPartiallySerialized()
            {
                value1 = 3
            });
            init = _entityFactory.BuildEntity<SerializableEntityDescriptor>(1, NamedGroup1.Group);
            init.Init(new EntityStructSerialized()
            {
                value = 4
            });
            init.Init(new EntityStructSerialized2()
            {
                value = 3
            });
            init.Init(new EntityStructPartiallySerialized()
            {
                value1 = 2
            });

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            var generateEntitySerializer = _enginesRoot.GenerateEntitySerializer();
            var simpleSerializationData  = new SimpleSerializationData(new FasterList<byte>());
            generateEntitySerializer.SerializeEntity(new EGID(0, NamedGroup1.Group), simpleSerializationData
                                                   , (int) SerializationType.Storage);
            generateEntitySerializer.SerializeEntity(new EGID(1, NamedGroup1.Group), simpleSerializationData
                                                   , (int) SerializationType.Storage);

            var newEnginesRoot = new EnginesRoot(_simpleSubmissionEntityViewScheduler);
            _neverDoThisIsJustForTheTest = new TestEngine();
            newEnginesRoot.AddEngine(_neverDoThisIsJustForTheTest);

            simpleSerializationData.Reset();
            generateEntitySerializer = newEnginesRoot.GenerateEntitySerializer();

            generateEntitySerializer.DeserializeNewEntity(new EGID(0, NamedGroup1.Group), simpleSerializationData
                                                        , (int) SerializationType.Storage);
            generateEntitySerializer.DeserializeNewEntity(new EGID(1, NamedGroup1.Group), simpleSerializationData
                                                        , (int) SerializationType.Storage);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized>(0, NamedGroup1.Group).value
              , Is.EqualTo(5));
            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized2>(0, NamedGroup1.Group).value
              , Is.EqualTo(4));
            Assert.That(
                _neverDoThisIsJustForTheTest
                   .entitiesDB.QueryEntity<EntityStructPartiallySerialized>(0, NamedGroup1.Group).value1
              , Is.EqualTo(3));

            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized>(1, NamedGroup1.Group).value
              , Is.EqualTo(4));
            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized2>(1, NamedGroup1.Group).value
              , Is.EqualTo(3));
            Assert.That(
                _neverDoThisIsJustForTheTest
                   .entitiesDB.QueryEntity<EntityStructPartiallySerialized>(1, NamedGroup1.Group).value1
              , Is.EqualTo(2));

            newEnginesRoot.Dispose();
        }

        [TestCase]
        public void TestSerializingWithEntityStructsWithVersioning()
        {
            var init = _entityFactory.BuildEntity<SerializableEntityDescriptorV0>(0, NamedGroup1.Group);
            init.Init(new EntityStructSerialized2()
            {
                value = 4
            });
            init.Init(new EntityStructPartiallySerialized()
            {
                value1 = 3
            });

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            FasterList<byte> bytes                    = new FasterList<byte>();
            var              generateEntitySerializer = _enginesRoot.GenerateEntitySerializer();
            var              simpleSerializationData  = new SimpleSerializationData(bytes);
            generateEntitySerializer.SerializeEntity(new EGID(0, NamedGroup1.Group), simpleSerializationData
                                                   , (int) SerializationType.Storage);

            var newEnginesRoot = new EnginesRoot(_simpleSubmissionEntityViewScheduler);
            _neverDoThisIsJustForTheTest = new TestEngine();

            newEnginesRoot.AddEngine(_neverDoThisIsJustForTheTest);

            simpleSerializationData.Reset();
            generateEntitySerializer = newEnginesRoot.GenerateEntitySerializer();

            //needed for the versioning to work
            generateEntitySerializer.RegisterSerializationFactory<SerializableEntityDescriptorV0>(
                new DefaultVersioningFactory<SerializableEntityDescriptorV1>());

            generateEntitySerializer.DeserializeNewEntity(new EGID(0, NamedGroup1.Group), simpleSerializationData
                                                        , (int) SerializationType.Storage);

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized2>(0, NamedGroup1.Group).value
              , Is.EqualTo(4));
            Assert.That(
                _neverDoThisIsJustForTheTest
                   .entitiesDB.QueryEntity<EntityStructPartiallySerialized>(0, NamedGroup1.Group).value1
              , Is.EqualTo(3));

            newEnginesRoot.Dispose();
        }

        [TestCase]
        public void TestSerializingWithEntityViewStructsAndFactories()
        {
            var init = _entityFactory.BuildEntity<SerializableEntityDescriptorWithViews>(
                0, NamedGroup1.Group, new[] {new Implementor(1)});
            init.Init(new EntityStructSerialized()
            {
                value = 5
            });
            init.Init(new EntityStructSerialized2()
            {
                value = 4
            });
            init.Init(new EntityStructPartiallySerialized()
            {
                value1 = 3
            });

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            FasterList<byte> bytes                    = new FasterList<byte>();
            var              generateEntitySerializer = _enginesRoot.GenerateEntitySerializer();
            var              simpleSerializationData  = new SimpleSerializationData(bytes);
            generateEntitySerializer.SerializeEntity(new EGID(0, NamedGroup1.Group), simpleSerializationData
                                                   , (int) SerializationType.Storage);

            var newEnginesRoot = new EnginesRoot(_simpleSubmissionEntityViewScheduler);
            _neverDoThisIsJustForTheTest = new TestEngine();

            newEnginesRoot.AddEngine(_neverDoThisIsJustForTheTest);

            simpleSerializationData.Reset();
            generateEntitySerializer = newEnginesRoot.GenerateEntitySerializer();
            DeserializationFactory factory = new DeserializationFactory();
            generateEntitySerializer.RegisterSerializationFactory<SerializableEntityDescriptorWithViews>(factory);

            generateEntitySerializer.DeserializeNewEntity(new EGID(0, NamedGroup1.Group), simpleSerializationData
                                                        , (int) SerializationType.Storage);

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized>(0, NamedGroup1.Group).value
              , Is.EqualTo(5));
            Assert.That(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<EntityStructSerialized2>(0, NamedGroup1.Group).value
              , Is.EqualTo(4));
            Assert.That(
                _neverDoThisIsJustForTheTest
                   .entitiesDB.QueryEntity<EntityStructPartiallySerialized>(0, NamedGroup1.Group).value1
              , Is.EqualTo(3));

            newEnginesRoot.Dispose();
        }

        EnginesRoot                       _enginesRoot;
        IEntityFactory                    _entityFactory;
        IEntityFunctions                  _entityFunctions;
        SimpleEntitiesSubmissionScheduler _simpleSubmissionEntityViewScheduler;
        TestEngine                        _neverDoThisIsJustForTheTest;

        //todo: this should not be at framework level
        public enum SerializationType
        {
            Network
          , Storage
        }

        class SerializableEntityDescriptor : SerializableEntityDescriptor<
            SerializableEntityDescriptor.DefaultPatternForEntityDescriptor>
        {
            [HashName("DefaultPatternForEntityDescriptor")]
            internal class DefaultPatternForEntityDescriptor : IEntityDescriptor
            {
                public IComponentBuilder[] componentsToBuild => ComponentsToBuild;

                static readonly IComponentBuilder[] ComponentsToBuild =
                {
                    new ComponentBuilder<EntityStructNotSerialized>()
                  , new SerializableComponentBuilder<SerializationType, EntityStructSerialized>()
                  , new SerializableComponentBuilder<SerializationType, EntityStructSerialized2>(
                        ((int) SerializationType.Storage, new DefaultSerializer<EntityStructSerialized2>())
                      , ((int) SerializationType.Network, new DefaultSerializer<EntityStructSerialized2>()))
                  , new SerializableComponentBuilder<SerializationType, EntityStructPartiallySerialized>(
                        ((int) SerializationType.Storage, new PartialSerializer<EntityStructPartiallySerialized>()))
                };
            }
        }

        class SerializableEntityDescriptorV0 : SerializableEntityDescriptor<
            SerializableEntityDescriptorV0.DefaultPatternForEntityDescriptor>
        {
            [HashName("DefaultPatternForEntityDescriptorV0")]
            internal class DefaultPatternForEntityDescriptor : IEntityDescriptor
            {
                public IComponentBuilder[] componentsToBuild => ComponentsToBuild;

                static readonly IComponentBuilder[] ComponentsToBuild =
                {
                    new ComponentBuilder<EntityStructNotSerialized>()
                  , new SerializableComponentBuilder<SerializationType, EntityStructSerialized2>(
                        ((int) SerializationType.Storage, new DefaultSerializer<EntityStructSerialized2>()))
                  , new SerializableComponentBuilder<SerializationType, EntityStructPartiallySerialized>(
                        ((int) SerializationType.Storage, new PartialSerializer<EntityStructPartiallySerialized>()))
                };
            }
        }

        class SerializableEntityDescriptorV1 : SerializableEntityDescriptor<
            SerializableEntityDescriptorV1.DefaultPatternForEntityDescriptor>
        {
            [HashName("DefaultPatternForEntityDescriptorV1")]
            internal class DefaultPatternForEntityDescriptor : IEntityDescriptor
            {
                public IComponentBuilder[] componentsToBuild => ComponentsToBuild;

                static readonly IComponentBuilder[] ComponentsToBuild =
                {
                    new ComponentBuilder<EntityStructNotSerialized>()
                  , new SerializableComponentBuilder<EntityStructSerialized>()
                  , new SerializableComponentBuilder<SerializationType, EntityStructSerialized2>(
                        ((int) SerializationType.Storage, new DefaultSerializer<EntityStructSerialized2>()))
                  , new SerializableComponentBuilder<SerializationType, EntityStructPartiallySerialized>(
                        ((int) SerializationType.Storage, new PartialSerializer<EntityStructPartiallySerialized>()))
                };
            }
        }

        class SerializableEntityDescriptorWithViews : SerializableEntityDescriptor<
            SerializableEntityDescriptorWithViews.DefaultPatternForEntityDescriptor>
        {
            [HashName("DefaultPatternForEntityDescriptorWithView")]
            internal class DefaultPatternForEntityDescriptor : IEntityDescriptor
            {
                public IComponentBuilder[] componentsToBuild => ComponentsToBuild;

                static readonly IComponentBuilder[] ComponentsToBuild =
                {
                    new ComponentBuilder<EntityViewStructNotSerialized>()
                  , new SerializableComponentBuilder<SerializationType, EntityStructSerialized>()
                  , new SerializableComponentBuilder<SerializationType, EntityStructSerialized2>(
                        ((int) SerializationType.Storage, new DefaultSerializer<EntityStructSerialized2>())
                      , ((int) SerializationType.Network, new DefaultSerializer<EntityStructSerialized2>()))
                  , new SerializableComponentBuilder<SerializationType, EntityStructPartiallySerialized>(
                        ((int) SerializationType.Storage, new PartialSerializer<EntityStructPartiallySerialized>()))
                };
            }
        }

        class TestEngine : IQueryingEntitiesEngine
        {
            public EntitiesDB entitiesDB { get; set; }
            public void       Ready()    { }

            public bool HasEntity<T>(EGID ID) where T : struct, IEntityComponent { return entitiesDB.Exists<T>(ID); }

            public bool HasAnyEntityInGroup<T>(ExclusiveGroup groupID) where T : struct, IEntityComponent
            {
                return entitiesDB.QueryEntities<T>(groupID).count > 0;
            }

            public bool HasAnyEntityInGroupArray<T>(ExclusiveGroup groupID) where T : struct, IEntityComponent
            {
                return entitiesDB.QueryEntities<T>(groupID).count > 0;
            }
        }

        struct EntityStructNotSerialized : IEntityComponent { }

        struct EntityStructSerialized : IEntityComponent
        {
            public int value;
        }

        struct EntityStructSerialized2 : IEntityComponent
        {
            public int value;
        }

        struct EntityStructPartiallySerialized : IEntityComponent
        {
#pragma warning disable 169
            int value;
#pragma warning restore 169
            [PartialSerializerField] public int value1;
        }

        struct EntityViewStructNotSerialized : IEntityViewComponent
        {
#pragma warning disable 649
            public ITestIt TestIt;
#pragma warning restore 649

            public EGID ID { get; set; }
        }

        interface ITestIt
        {
            float value { get; set; }
        }

        class Implementor : ITestIt
        {
            public Implementor(int i) { value = i; }

            public float value { get; set; }
        }

        class DeserializationFactory : IDeserializationFactory
        {
            public EntityComponentInitializer BuildDeserializedEntity
            (EGID egid, ISerializationData serializationData, ISerializableEntityDescriptor entityDescriptor
           , int serializationType, IEntitySerialization entitySerialization, IEntityFactory factory
           , bool enginesRootIsDeserializationOnly)
            {
                var initializer =
                    factory.BuildEntity<SerializableEntityDescriptorWithViews>(egid, new[] {new Implementor(1)});

                entitySerialization.DeserializeEntityComponents(serializationData, entityDescriptor, ref initializer
                                                              , (int) SerializationType.Storage);

                return initializer;
            }
        }
    }
}