﻿using System;
using System.Text;
using NUnit.Framework;
using Svelto.DataStructures;
using Svelto.ECS;
using Svelto.ECS.Experimental;
using Svelto.ECS.Hybrid;
using Assert = NUnit.Framework.Assert;

namespace Svelto.ECS.Tests.Messy
{
    [TestFixture]
    public class TestSveltoECS
    {
        static readonly ExclusiveGroup group1  = new ExclusiveGroup();
        static readonly ExclusiveGroup group2  = new ExclusiveGroup();
        static readonly ExclusiveGroup group3  = new ExclusiveGroup();
        static readonly ExclusiveGroup group6  = new ExclusiveGroup();
        static readonly ExclusiveGroup group7  = new ExclusiveGroup();
        static readonly ExclusiveGroup group8  = new ExclusiveGroup();
        static readonly ExclusiveGroup group0  = new ExclusiveGroup();
        static readonly ExclusiveGroup groupR4 = new ExclusiveGroup(4);

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
        public void TestBuildEntityViewStructWithoutImplementors()
        {
            _entityFactory.BuildEntity<TestDescriptorEntityViewWithGenerics>(new EGID(1, group1));
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.Pass();
        }

        [TestCase]
        public void TestBuildEntityViewStructWithoutImplementorsAndWrongGeneric()
        {
            void CheckFunction()
            {
                _entityFactory.BuildEntity<TestDescriptorEntityViewWithWrongGenerics>(new EGID(1, group1));
                _simpleSubmissionEntityViewScheduler.SubmitEntities();
            }

            Assert.Throws(typeof(TypeInitializationException), CheckFunction); //it's TypeInitializationException because the Type is not being constructed due to the ECSException
        }
        
        [TestCase((uint) 0)] [TestCase((uint) 1)] [TestCase((uint) 2)]
        public void TestExceptionTwoEntitiesCannotHaveTheSameIDInTheSameGroupInterleaved(uint id)
        {
            void CheckFunction()
            {
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group1), new[] {new TestIt(2)});
                _simpleSubmissionEntityViewScheduler.SubmitEntities();

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group1), new[] {new TestIt(2)});
                _simpleSubmissionEntityViewScheduler.SubmitEntities();
            }

            Assert.Throws(typeof(ECSException), CheckFunction);
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestCreationAndRemovalOfDynamicEntityDescriptors(uint id)
        {
            var ded = new DynamicEntityDescriptor<TestDescriptor>(new IComponentBuilder[]
            {
                new ComponentBuilder<TestEntityStruct>()
            });

            bool hasit;
            //Build Entity id, group0
            {
                _entityFactory.BuildEntity(new EGID(id, group0), ded, new[] {new TestIt(2)});

                _simpleSubmissionEntityViewScheduler.SubmitEntities();

                hasit = _neverDoThisIsJustForTheTest.HasEntity<TestEntityStruct>(new EGID(id, group0));

                Assert.IsTrue(hasit);
            }

            //Swap Entity id, group0 to group 3
            {
                _entityFunctions.SwapEntityGroup<TestDescriptor>(new EGID(id, group0), group3);

                _simpleSubmissionEntityViewScheduler.SubmitEntities();

                hasit = _neverDoThisIsJustForTheTest.HasEntity<TestEntityStruct>(new EGID(id, group3));

                Assert.IsTrue(hasit);
            }

            _entityFunctions.RemoveEntity<TestDescriptor>(new EGID(id, group3));

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            hasit = _neverDoThisIsJustForTheTest.HasEntity<TestEntityStruct>(new EGID(id, group3));

            Assert.IsFalse(hasit);
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestExceptionTwoEntitiesCannotHaveTheSameIDInTheSameGroup(uint id)
        {
            bool crashed = false;

            try
            {
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group0), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group0), new[] {new TestIt(2)});
                _simpleSubmissionEntityViewScheduler.SubmitEntities();
            }
            catch
            {
                crashed = true;
            }

            Assert.IsTrue(crashed);
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestExceptionTwoDifferentEntitiesCannotHaveTheSameIDInTheSameGroupInterleaved(uint id)
        {
            void CheckFunction()
            {
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group0), new[] {new TestIt(2)});

                _simpleSubmissionEntityViewScheduler.SubmitEntities();

                _entityFactory.BuildEntity<TestDescriptor2>(new EGID(id, group0), new[] {new TestIt(2)});

                _simpleSubmissionEntityViewScheduler.SubmitEntities();
            }

            Assert.That(CheckFunction, Throws.TypeOf<ECSException>());
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestExceptionTwoDifferentEntitiesCannotHaveTheSameIDInTheSameGroup(uint id)
        {
            bool crashed = false;

            try
            {
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group0), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor2>(new EGID(id, group0), new[] {new TestIt(2)});

                _simpleSubmissionEntityViewScheduler.SubmitEntities();
            }
            catch
            {
                crashed = true;
            }

            Assert.IsTrue(crashed);
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestTwoEntitiesWithSameIdThrowsIntervaled(uint id)
        {
            void CheckFunction()
            {
                _entityFactory.BuildEntity<TestDescriptor7>(new EGID(id, group0));
                _simpleSubmissionEntityViewScheduler.SubmitEntities();

                _entityFactory.BuildEntity<TestDescriptor3>(new EGID(id, group0), new[] {new TestIt(2)});
                _simpleSubmissionEntityViewScheduler.SubmitEntities();
            }

            Assert.That(CheckFunction, Throws.TypeOf<ECSException>());
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestTwoEntitiesWithSameIDThrows(uint id)
        {
            bool crashed = false;

            try
            {
                _entityFactory.BuildEntity<TestDescriptor7>(new EGID(id, group0), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor3>(new EGID(id, group0), new[] {new TestIt(2)});
                _simpleSubmissionEntityViewScheduler.SubmitEntities();
            }
            catch
            {
                crashed = true;
            }

            Assert.IsTrue(crashed);
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestTwoEntitiesWithSameIDWorksOnDifferentGroups(uint id)
        {
            _entityFactory.BuildEntity<TestDescriptor2>(new EGID(id, group0), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor3>(new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group0)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestRemoveEntity(uint id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFunctions.RemoveEntity<TestDescriptor>(new EGID(id, group1));
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestBuildEntity(uint id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestBuildEntityWithImplementor(uint id)
        {
            _entityFactory.BuildEntity<TestEntityWithComponentViewAndComponentStruct>(
                new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));

            var entityView =
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntity<TestEntityViewStruct>(new EGID(id, group1));
            Assert.AreEqual(entityView.TestIt.value, 2);

            uint index;
            Assert.AreEqual(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntitiesAndIndex<TestEntityViewStruct>(
                    new EGID(id, group1), out index)[index].TestIt.value, 2);
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestBuildEntityViewStruct(uint id)
        {
            _entityFactory.BuildEntity<TestDescriptor4>(new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestBuildEntitytruct(uint id)
        {
            _entityFactory.BuildEntity<TestDescriptor7>(new EGID(id, group1), null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityStruct>(group1));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestBuildEntityStructWithInitializer(uint id)
        {
            var init = _entityFactory.BuildEntity<TestDescriptor7>(new EGID(id, group1), null);
            init.Init(new TestEntityStruct(3));
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityStruct>(group1));
            uint index;
            Assert.IsTrue(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntitiesAndIndex<TestEntityStruct>(
                    new EGID(id, group1), out index)[index].value == 3);
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestBuildEntityMixed(uint id)
        {
            TestIt testIt = new TestIt(2);
            _entityFactory.BuildEntity<TestEntityWithComponentViewAndComponentStruct>(
                new EGID(id, group1), new[] {testIt});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityStruct>(group1));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));
            var (entityCollection, count) = _neverDoThisIsJustForTheTest.entitiesDB.QueryEntities<TestEntityViewStruct>(group1);
            Assert.AreSame(entityCollection[0].TestIt, testIt);
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestBuildEntityWithViewStructWithImplementorAndTestQueryEntitiesAndIndex(uint id)
        {
            var testIt = new TestIt(2);
            _entityFactory.BuildEntity<TestDescriptor4>(new EGID(id, group1), new[] {testIt});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));

            uint index;
            var testEntityView2 =
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntitiesAndIndex<TestEntityViewStruct>(
                    new EGID(id, group1), out index)[index];

            Assert.AreEqual(testEntityView2.TestIt, testIt);
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestBuildEntityToGroupWithDescriptorInfo(uint id)
        {
            _entityFactory.BuildEntity(new EGID(id, group1), new TestDescriptor(), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestBuildEntityInAddFunction(uint id)
        {
            _enginesRoot.AddEngine(new TestEngineAdd(_entityFactory));
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities(); //submit the entities
            _simpleSubmissionEntityViewScheduler.SubmitEntities(); //now submit the entities added by the engines
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityStruct>(new EGID(100, group0)));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestRemoveEntityFromGroup(uint id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));

            _entityFunctions.RemoveEntity<TestDescriptor>(id, group1);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestRemoveEntityGroup(uint id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));

            _entityFunctions.RemoveEntitiesFromGroup(group1);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestRemoveAndAddAgainEntity(uint id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFunctions.RemoveEntity<TestDescriptor>(id, group1);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group1)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group1));
        }

        [TestCase((uint) 0)]
        [TestCase((uint) 1)]
        [TestCase((uint) 2)]
        public void TestSwapGroup(uint id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(id, group0), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFunctions.SwapEntityGroup<TestDescriptor>(id, group0, group3);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group0)));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group0));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group3));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group3)));

            Assert.AreEqual(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntitiesAndIndex<TestEntityViewStruct>(
                    new EGID(id, group3), out var index)[index].ID.entityID, id);
            Assert.AreEqual(
                (ulong) _neverDoThisIsJustForTheTest.entitiesDB.QueryEntitiesAndIndex<TestEntityViewStruct>(
                    new EGID(id, group3), out index)[index].ID.groupID
              , (ulong) group3);

            _entityFunctions.SwapEntityGroup<TestDescriptor>(id, group3, group0);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group0)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group0));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group3));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(id, group3)));

            Assert.AreEqual(
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntitiesAndIndex<TestEntityViewStruct>(
                    new EGID(id, group0), out index)[index].ID.entityID, id);
            Assert.AreEqual(
                (ulong) _neverDoThisIsJustForTheTest.entitiesDB.QueryEntitiesAndIndex<TestEntityViewStruct>(
                    new EGID(id, group0), out index)[index].ID.groupID
              , (ulong) group0);
        }

        // [TestCase((uint)0, (uint)1, (uint)2, (uint)3)]
        // [TestCase((uint)4, (uint)5, (uint)6, (uint)7)]
        // [TestCase((uint)8, (uint)9, (uint)10, (uint)11)]
        // public void TestExecuteOnAllTheEntities(uint id, uint id2, uint id3, uint id4)
        // {
        //     _entityFactory.BuildEntity<TestEntityWithComponentViewAndComponentStruct>(new EGID(id, groupR4), new[] { new TestIt(2) });
        //     _entityFactory.BuildEntity<TestEntityWithComponentViewAndComponentStruct>(new EGID(id2, groupR4 + 1), new[] { new TestIt(2) });
        //     _entityFactory.BuildEntity<TestEntityWithComponentViewAndComponentStruct>(new EGID(id3, groupR4 + 2), new[] { new TestIt(2) });
        //     _entityFactory.BuildEntity<TestEntityWithComponentViewAndComponentStruct>(new EGID(id4, groupR4 + 3), new[] { new TestIt(2) });
        //     _simpleSubmissionEntityViewScheduler.SubmitEntities();
        //
        //     _neverDoThisIsJustForTheTest.entitiesDB.ExecuteOnAllEntities<TestEntityViewStruct>((entity, group, groupCount, db) =>
        //     {
        //         for (int i = 0; i < groupCount; i++)
        //             entity[i].TestIt.value = entity[i].ID.entityID;
        //     });
        //
        //     _neverDoThisIsJustForTheTest.entitiesDB.ExecuteOnAllEntities<TestEntityStruct>((entity, group, groupCount, db) =>
        //     {
        //         for (int i = 0; i < groupCount; i++)
        //             entity[i].value = entity[i].ID.entityID;
        //     });
        //
        //
        //     for (uint i = 0; i < 4; i++)
        //     {
        //         EntityCollection<TestEntityStruct> buffer1 = _neverDoThisIsJustForTheTest.entitiesDB.QueryEntities<TestEntityStruct>(groupR4 + i);
        //         EntityCollection<TestEntityViewStruct> buffer2 = _neverDoThisIsJustForTheTest.entitiesDB.QueryEntities<TestEntityViewStruct>(groupR4 + i);
        //
        //         Assert.AreEqual(buffer1.count, 1);
        //         Assert.AreEqual(buffer2.count, 1);
        //
        //         for (int j = 0; j < buffer1.count; j++)
        //         {
        //             Assert.AreEqual(buffer1[j].value, buffer1[j].ID.entityID);
        //             Assert.AreEqual(buffer2[j].TestIt.value, buffer2[j].ID.entityID);
        //         }
        //     }
        //
        //     _entityFunctions.RemoveEntity<TestEntityWithComponentViewAndComponentStruct>(new EGID(id, groupR4));
        //     _entityFunctions.RemoveEntity<TestEntityWithComponentViewAndComponentStruct>(new EGID(id2, groupR4 + 1));
        //     _entityFunctions.RemoveEntity<TestEntityWithComponentViewAndComponentStruct>(new EGID(id3, groupR4 + 2));
        //     _entityFunctions.RemoveEntity<TestEntityWithComponentViewAndComponentStruct>(new EGID(id4, groupR4 + 3));
        //     _simpleSubmissionEntityViewScheduler.SubmitEntities();
        //
        //     _neverDoThisIsJustForTheTest.entitiesDB.ExecuteOnAllEntities<TestEntityViewStruct>((entity, group,  groupCount, db) =>
        //     {
        //         Assert.Fail();
        //     });
        //
        //     _neverDoThisIsJustForTheTest.entitiesDB.ExecuteOnAllEntities<TestEntityStruct>((entity, group,  groupCount, db) =>
        //     {
        //         Assert.Fail();
        //     });
        // }

        [TestCase]
        public void QueryingNotExistingViewsInAnExistingGroupMustNotCrash()
        {
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasAnyEntityInGroup<TestEntityViewStruct>(group0));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasAnyEntityInGroupArray<TestEntityViewStruct>(group0));
        }

        [TestCase]
        public void TestExtendibleDescriptor()
        {
            _entityFactory.BuildEntity<B>(new EGID(1, group0), null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            _entityFunctions.SwapEntityGroup<A>(new EGID(1, group0), group1);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<EVS2>(new EGID(1, group0)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<EVS2>(new EGID(1, group1)));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<EVS1>(new EGID(1, group0)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<EVS1>(new EGID(1, group1)));
        }

        [TestCase]
        public void TestExtendibleDescriptor2()
        {
            _entityFactory.BuildEntity<B2>(new EGID(1, group0), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            _entityFunctions.SwapEntityGroup<A2>(new EGID(1, group0), group1);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(1, group0)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityViewStruct>(new EGID(1, group1)));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<TestEntityStruct>(new EGID(1, group0)));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityStruct>(new EGID(1, group1)));
        }

        [TestCase]
        public void TestQueryEntitiesWithMultipleParamsTwoStructs()
        {
            for (int i = 0; i < 100; i++)
            {
                var init = _entityFactory.BuildEntity<TestDescriptor6>(new EGID((uint) i, group0));
                init.Init(new TestEntityStruct((uint) (i)));
                init.Init(new TestEntityStruct2((uint) (i + 100)));
            }

            for (int i = 0; i < 100; i++)
            {
                var init = _entityFactory.BuildEntity<TestDescriptor6>(new EGID((uint) i, group1));
                init.Init(new TestEntityStruct((uint) (i + 200)));
                init.Init(new TestEntityStruct2((uint) (i + 300)));
            }

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            var iterators =
                _neverDoThisIsJustForTheTest.entitiesDB.QueryEntities<TestEntityStruct, TestEntityStruct2>(
                    new LocalFasterReadOnlyList<ExclusiveGroupStruct>(new FasterList<ExclusiveGroupStruct>(new ExclusiveGroupStruct[] {group0, group1})));

            uint index = 0;

            foreach (var ((iteratorentityComponentA, iteratorentityComponentB, count), exclusiveGroupStruct) in iterators)
            {
                for (int i = 0; i < count; i++)
                {
                    if (exclusiveGroupStruct == group0)
                    {
                        Assert.AreEqual(iteratorentityComponentA[i].value, index);
                        Assert.AreEqual(iteratorentityComponentB[i].value, index + 100);
                    }
                    else
                    {
                        Assert.AreEqual(iteratorentityComponentA[i].value, index + 200);
                        Assert.AreEqual(iteratorentityComponentB[i].value, index + 300);
                    }
                    
                    index = ++index % 100;
                }
            }
        }

        [TestCase]
        public void TestQueryEntitiesWithMultipleParamsOneStruct()
        {
            for (int i = 0; i < 100; i++)
            {
                var init = _entityFactory.BuildEntity<TestDescriptor6>(new EGID((uint) i, group0));
                init.Init(new TestEntityStruct((uint) (i)));
                init.Init(new TestEntityStruct2((uint) (i + 100)));
            }

            for (int i = 0; i < 100; i++)
            {
                var init = _entityFactory.BuildEntity<TestDescriptor6>(new EGID((uint) i, group1));
                init.Init(new TestEntityStruct((uint) (i + 200)));
                init.Init(new TestEntityStruct2((uint) (i + 300)));
            }

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            FasterList<ExclusiveGroupStruct> groupStructId =
                new FasterList<ExclusiveGroupStruct>(new ExclusiveGroupStruct[] {group0, group1});
            var iterators = _neverDoThisIsJustForTheTest.entitiesDB.QueryEntities<TestEntityStruct>(groupStructId);

            uint index = 0;

            foreach (var ((iterator, count), exclusiveGroupStruct) in iterators)
            {
                for (int i = 0; i < count; i++)
                {
                    if (iterator[i].ID.groupID == group0)
                        Assert.IsTrue(iterator[i].value == index);
                    else
                        Assert.IsTrue(iterator[i].value == (index + 200));

                    index = ++index % 100;
                }
            }
        }

        EnginesRoot                       _enginesRoot;
        IEntityFactory                    _entityFactory;
        IEntityFunctions                  _entityFunctions;
        SimpleEntitiesSubmissionScheduler _simpleSubmissionEntityViewScheduler;
        TestEngine                        _neverDoThisIsJustForTheTest;

        class TestEngineAdd : IReactOnAddAndRemove<TestEntityViewStruct>
        {
            public TestEngineAdd(IEntityFactory entityFactory) { _entityFactory = entityFactory; }

            public void Add(ref TestEntityViewStruct entityView, EGID egid)
            {
                _entityFactory.BuildEntity<TestDescriptor7>(new EGID(100, group0), null);
            }

            public void Remove(ref TestEntityViewStruct entityView, EGID egid)
            {
                // Svelto.ECS.Tests\Svelto.ECS\DataStructures\TypeSafeDictionary.cs:line 196
                // calls Remove - throwing NotImplementedException here causes test host to
                // crash in Visual Studio or when using "dotnet test" from the command line
                // throw new NotImplementedException();
            }

            readonly IEntityFactory _entityFactory;
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
    }

    struct EVS1 : IEntityComponent
    {
        public EGID ID { get; set; }
    }

    struct EVS2 : IEntityComponent
    {
        public EGID ID { get; set; }
    }

    class A : GenericEntityDescriptor<EVS1> { }

    class B : ExtendibleEntityDescriptor<A>
    {
        static readonly IComponentBuilder[] _nodesToBuild;

        static B() { _nodesToBuild = new IComponentBuilder[] {new ComponentBuilder<EVS2>(),}; }

        public B() : base(_nodesToBuild) { }
    }

    class A2 : GenericEntityDescriptor<TestEntityViewStruct> { }

    class B2 : ExtendibleEntityDescriptor<A2>
    {
        static readonly IComponentBuilder[] _nodesToBuild;

        static B2() { _nodesToBuild = new IComponentBuilder[] {new ComponentBuilder<TestEntityStruct>(),}; }

        public B2() : base(_nodesToBuild) { }
    }

    class TestDescriptor : GenericEntityDescriptor<TestEntityViewStruct> { }

    class TestDescriptor2 : GenericEntityDescriptor<TestEntityViewStruct> { }

    class TestDescriptor3 : GenericEntityDescriptor<TestEntityViewStruct> { }

    class TestDescriptor4 : GenericEntityDescriptor<TestEntityViewStruct> { }

    class TestDescriptor7 : GenericEntityDescriptor<TestEntityStruct> { }

    class TestEntityWithComponentViewAndComponentStruct : GenericEntityDescriptor<TestEntityViewStruct, TestEntityStruct
    > { }

    class TestDescriptor6 : GenericEntityDescriptor<TestEntityStruct, TestEntityStruct2> { }

    class TestDescriptorEntityViewWithGenerics : GenericEntityDescriptor<TestDescriptorEntityViewThatWorks> { }

    class TestDescriptorEntityViewWithWrongGenerics : GenericEntityDescriptor<TestDescriptorEntityViewThatCannotWork> { }

    struct TestDescriptorEntityViewThatWorks : IEntityViewComponent
    {
        internal struct Generic<Test> { }

#pragma warning disable 649
        public Generic<uint> test;
#pragma warning restore 649

        public EGID ID { get; set; }
    }

    struct TestDescriptorEntityViewThatCannotWork : IEntityViewComponent
    {
        internal struct Generic<Test> { }

#pragma warning disable 649
        public Generic<object> test;
#pragma warning restore 649

        public EGID ID { get; set; }
    }

    struct TestEntityStruct : IEntityComponent, INeedEGID
    {
        public readonly uint value;

        public TestEntityStruct(uint value) : this() { this.value = value; }

        public EGID ID { get; set; }
    }

    struct TestEntityStruct2 : IEntityComponent
    {
        public uint value;

        public TestEntityStruct2(uint value) : this() { this.value = value; }
    }

    struct TestEntityViewStruct : IEntityViewComponent
    {
#pragma warning disable 649
        public ITestIt TestIt;
#pragma warning restore 649

        public EGID ID { get; set; }
    }

    interface ITestIt
    {
        float? testNullable { get; set; }
        StringBuilder testStringBuilder { get; set; }
        string testStrings { get; set; }
        
        float value { get; set; }
    }

    class TestIt : ITestIt
    {
        public TestIt(int i) { value = i; }

        public float? testNullable { get; set; }
        public StringBuilder testStringBuilder { get; set; }
        public string testStrings { get; set; }
        public float value { get; set; }
    }
}