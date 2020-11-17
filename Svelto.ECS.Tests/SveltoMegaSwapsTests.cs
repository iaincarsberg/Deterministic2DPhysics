using NUnit.Framework;
using Svelto.ECS;

namespace Svelto.ECS.Tests.Messy
{
    [TestFixture]
    public class TestSveltoMegaSwaps
    {
        static readonly ExclusiveGroup group1  = new ExclusiveGroup();
        static readonly ExclusiveGroup group2  = new ExclusiveGroup();
        static readonly ExclusiveGroup group3  = new ExclusiveGroup();
        static readonly ExclusiveGroup group6  = new ExclusiveGroup();
        static readonly ExclusiveGroup group7  = new ExclusiveGroup();
        static readonly ExclusiveGroup group8  = new ExclusiveGroup();
        static readonly ExclusiveGroup group0  = new ExclusiveGroup();
        static readonly ExclusiveGroup groupR4 = new ExclusiveGroup(4);

        EnginesRoot                       _enginesRoot;
        IEntityFactory                    _entityFactory;
        IEntityFunctions                  _entityFunctions;
        SimpleEntitiesSubmissionScheduler _simpleSubmissionEntityViewScheduler;

        [SetUp]
        public void Init()
        {
            _simpleSubmissionEntityViewScheduler = new SimpleEntitiesSubmissionScheduler();
            _enginesRoot                         = new EnginesRoot(_simpleSubmissionEntityViewScheduler);

            _entityFactory   = _enginesRoot.GenerateEntityFactory();
            _entityFunctions = _enginesRoot.GenerateEntityFunctions();
        }

        [TearDown]
        public void Dipose() { _enginesRoot.Dispose(); }

        [TestCase]
        public void TestMegaEntitySwap()
        {
            for (int i = 0; i < 29; i++)
            {
                EGID egid = new EGID((uint) i, group1);
                _entityFactory.BuildEntity<TestDescriptor>(egid, new[] {new TestIt(2)});
            }

            Assert.IsTrue(true);

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            SwapMinNeededForException(_entityFunctions);

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            bool allFound       = true;
            bool mustNotBeFound = false;

            for (uint i = 0; i < 29; i++)
                allFound &=
                    ((IUnitTestingInterface) _enginesRoot).entitiesForTesting.Exists<TestEntityViewStruct>(
                        new EGID(i, group2));

            for (int i = 0; i < 29; i++)
                mustNotBeFound |=
                    ((IUnitTestingInterface) _enginesRoot).entitiesForTesting.Exists<TestEntityViewStruct>(
                        new EGID((uint) i, group1));

            Assert.IsTrue(allFound);
            Assert.IsTrue(mustNotBeFound == false);
        }

        void SwapMinNeededForException(IEntityFunctions entityFunctions)
        {
            entityFunctions.SwapEntityGroup<TestDescriptor>(18, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(19, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(20, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(21, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(22, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(17, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(16, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(15, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(14, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(13, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(11, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(9, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(6, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(5, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(4, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(3, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(2, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(0, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(24, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(25, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(26, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(27, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(28, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(23, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(8, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(7, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(1, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(12, group1, group2);
            entityFunctions.SwapEntityGroup<TestDescriptor>(10, group1, group2);
        }

        [TestCase]
        public void TestMegaEntitySwap2()
        {
            unchecked
            {
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(5000, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4999, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4998, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4997, group1), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4996, group2), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4995, group2), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4994, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4993, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4992, group1), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4991, group2), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4990, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4988, group1), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4987, group2), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4986, group2), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4985, group1), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4984, group2), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4980, group2), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4977, group2), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4976, group1), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4974, group2), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4971, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4970, group1), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4967, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4966, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4965, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4964, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4963, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4962, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4961, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4960, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4959, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4958, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4957, group1), new[] {new TestIt(2)});

                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4955, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4954, group1), new[] {new TestIt(2)});
                _entityFactory.BuildEntity<TestDescriptor>(new EGID(4953, group1), new[] {new TestIt(2)});

                _simpleSubmissionEntityViewScheduler.SubmitEntities();

                _entityFunctions.SwapEntityGroup<TestDescriptor>(4996, group2, group1);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4995, group2, group1);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4991, group2, group1);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4986, group2, group1);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4984, group2, group1);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4980, group2, group1);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4977, group2, group1);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4974, group2, group1);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(5000, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4999, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4998, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4997, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4994, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4993, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4992, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4990, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4988, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4985, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4976, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4971, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4970, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4967, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4966, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4965, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4964, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4963, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4962, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4961, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4960, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4959, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4958, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4957, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4955, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4954, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4953, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4974, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4977, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4980, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4984, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4986, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4991, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4995, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4996, group1, group6);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(5000, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4999, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4998, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4997, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4994, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4993, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4992, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4990, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4988, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4985, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4976, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4971, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4970, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4967, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4966, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4965, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4964, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4963, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4962, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4961, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4960, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4959, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4958, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4957, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4955, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4954, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4953, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4996, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4995, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4991, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4986, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4984, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4980, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4977, group6, group7);
                _entityFunctions.SwapEntityGroup<TestDescriptor>(4974, group6, group7);

                _simpleSubmissionEntityViewScheduler.SubmitEntities();

                EGID egid = new EGID(4958, group7);
                bool exists =
                    ((IUnitTestingInterface) _enginesRoot).entitiesForTesting.Exists<TestEntityViewStruct>(egid);
                Assert.IsTrue(exists);

                _entityFunctions.SwapEntityGroup<TestDescriptor>(4961, group7, group8);
                _simpleSubmissionEntityViewScheduler.SubmitEntities();

                exists = ((IUnitTestingInterface) _enginesRoot).entitiesForTesting.Exists<TestEntityViewStruct>(egid);

                Assert.IsTrue(exists);
            }
        }

        [TestCase]
        public void TestMegaEntitySwap3()
        {
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(5000, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4999, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4998, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4997, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4996, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4995, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4994, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4993, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4992, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4991, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4990, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4989, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4988, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4987, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4986, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4985, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4984, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4983, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4982, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4981, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4980, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4979, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4978, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4977, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4976, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4975, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4974, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4973, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4972, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4971, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4970, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4969, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4968, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4967, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4966, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4965, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4964, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4963, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4962, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4961, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4960, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4959, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4958, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4957, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4956, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4955, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4954, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4953, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4952, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4951, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4950, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4949, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4948, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4947, group2), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4946, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4945, group1), new[] {new TestIt(2)});
            _entityFactory.BuildEntity<TestDescriptor>(new EGID(4944, group1), new[] {new TestIt(2)});
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4987, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4986, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4982, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4977, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4975, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4974, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4971, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4968, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4965, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4964, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4960, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4959, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4947, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(5000, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4999, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4998, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4997, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4996, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4995, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4994, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4993, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4992, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4991, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4990, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4989, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4988, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4985, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4984, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4983, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4981, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4980, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4979, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4976, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4973, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4970, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4967, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4966, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4962, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4961, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4958, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4957, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4956, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4955, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4954, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4953, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4952, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4951, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4950, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4949, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4948, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4946, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4945, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4944, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4963, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4972, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(5000, group2, group0);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4998, group2, group0);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4980, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4973, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4970, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4966, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4947, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4959, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4960, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4964, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4965, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4968, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4971, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4974, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4975, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4977, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4982, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4986, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4987, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4999, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4997, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4996, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4995, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4994, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4993, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4992, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4991, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4990, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4989, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4988, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4985, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4984, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4981, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4979, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4976, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4967, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4962, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4961, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4958, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4957, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4956, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4955, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4954, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4953, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4952, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4951, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4950, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4949, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4948, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4946, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4945, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4944, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4969, group2, group1);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4971, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4966, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4970, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4973, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4980, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4972, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4963, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4987, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4986, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4982, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4977, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4975, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4968, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4965, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4964, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4960, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4959, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4947, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4983, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4974, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4970, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4980, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4972, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4971, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4969, group1, group2);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4963, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4973, group6, group7);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4966, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4978, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4969, group2, group1);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4972, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4980, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4983, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4974, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4970, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4971, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4969, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4978, group1, group6);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4970, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4972, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4980, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4983, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4974, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4978, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4969, group6, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4988, group7, group8);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4988, group8, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4971, group7, group8);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4971, group8, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4992, group7, group8);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4992, group8, group7);
            _entityFunctions.SwapEntityGroup<TestDescriptor>(4967, group7, group8);

            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            EGID egid   = new EGID(4985, group7);
            bool exists = ((IUnitTestingInterface) _enginesRoot).entitiesForTesting.Exists<TestEntityViewStruct>(egid);

            Assert.IsTrue(exists);
        }
    }
}