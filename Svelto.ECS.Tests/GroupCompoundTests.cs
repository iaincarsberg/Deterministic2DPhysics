using NUnit.Framework;
using Svelto.ECS;

namespace Svelto.ECS.Tests.GroupCompounds
{
    [TestFixture]
    public class GroupCompound1
    {
        public class DOOFUSES : GroupTag<DOOFUSES> { }

        public class EATING : GroupTag<EATING> { }

        public class NOTEATING : GroupTag<NOTEATING> { }

        public class RED : GroupTag<RED> { }

        [TestCase]
        public void TestGroupCompound1()
        {
            var EatingReadDoofuses1 = GroupCompound<DOOFUSES, RED, EATING>.Groups;
            var EatingReadDoofuses2 = GroupCompound<RED, DOOFUSES, EATING>.Groups;
            var EatingReadDoofuses3 = GroupCompound<DOOFUSES, EATING, RED>.Groups;
            var EatingReadDoofuses4 = GroupCompound<EATING, DOOFUSES, RED>.Groups;
            var EatingReadDoofuses5 = GroupCompound<EATING, RED, DOOFUSES>.Groups;
            var EatingReadDoofuses6 = GroupCompound<RED, EATING, DOOFUSES>.Groups;

            var NoEatingReadDoofuses1 = GroupCompound<DOOFUSES, RED, NOTEATING>.Groups;
            var NoEatingReadDoofuses2 = GroupCompound<RED, DOOFUSES, NOTEATING>.Groups;
            var NoEatingReadDoofuses3 = GroupCompound<DOOFUSES, NOTEATING, RED>.Groups;
            var NoEatingReadDoofuses4 = GroupCompound<NOTEATING, DOOFUSES, RED>.Groups;
            var NoEatingReadDoofuses5 = GroupCompound<NOTEATING, RED, DOOFUSES>.Groups;
            var NoEatingReadDoofuses6 = GroupCompound<RED, NOTEATING, DOOFUSES>.Groups;

            var RedDoofuses1 = GroupCompound<DOOFUSES, RED>.Groups;
            var RedDoofuses2 = GroupCompound<RED, DOOFUSES>.Groups;

            var EatingDoofuses1 = GroupCompound<DOOFUSES, EATING>.Groups;
            var EatingDoofuses2 = GroupCompound<EATING, DOOFUSES>.Groups;

            var NotEatingDoofuses1 = GroupCompound<NOTEATING, DOOFUSES>.Groups;
            var NotEatingDoofuses2 = GroupCompound<DOOFUSES, NOTEATING>.Groups;

            var RedEating1 = GroupCompound<NOTEATING, RED>.Groups;
            var RedEating2 = GroupCompound<RED, NOTEATING>.Groups;

            var NotReadEating1 = GroupCompound<EATING, RED>.Groups;
            var NotReadEating2 = GroupCompound<RED, EATING>.Groups;

            Assert.AreEqual(RedDoofuses1, RedDoofuses2);

            Assert.AreEqual(EatingDoofuses1, EatingDoofuses2);

            Assert.AreEqual(NotEatingDoofuses1, NotEatingDoofuses2);

            Assert.AreEqual(RedEating1, RedEating2);

            Assert.AreEqual(NotReadEating1, NotReadEating2);

            Assert.AreEqual(EatingReadDoofuses1, EatingReadDoofuses2);
            Assert.AreEqual(EatingReadDoofuses2, EatingReadDoofuses3);
            Assert.AreEqual(EatingReadDoofuses3, EatingReadDoofuses4);
            Assert.AreEqual(EatingReadDoofuses4, EatingReadDoofuses5);
            Assert.AreEqual(EatingReadDoofuses5, EatingReadDoofuses6);

            Assert.AreEqual(NoEatingReadDoofuses1, NoEatingReadDoofuses2);
            Assert.AreEqual(NoEatingReadDoofuses2, NoEatingReadDoofuses3);
            Assert.AreEqual(NoEatingReadDoofuses3, NoEatingReadDoofuses4);
            Assert.AreEqual(NoEatingReadDoofuses4, NoEatingReadDoofuses5);
            Assert.AreEqual(NoEatingReadDoofuses5, NoEatingReadDoofuses6);

            Assert.AreNotEqual(EatingReadDoofuses5, NoEatingReadDoofuses6);

            Assert.That(GroupTag<DOOFUSES>.Groups.count, Is.EqualTo(6));
            Assert.That(GroupTag<EATING>.Groups.count, Is.EqualTo(4));
            Assert.That(GroupTag<RED>.Groups.count, Is.EqualTo(6));
            Assert.That(GroupTag<NOTEATING>.Groups.count, Is.EqualTo(4));
        }
    }

    [TestFixture]
    public class GroupCompound4
    {
        public class DOOFUSES : GroupTag<DOOFUSES> { }

        public class EATING : GroupTag<EATING> { }

        public class NOTEATING : GroupTag<NOTEATING> { }

        public class RED : GroupTag<RED> { }

        [TestCase]
        public void TestGroupCompound4()
        {
            var EatingDoofuses1 = GroupCompound<DOOFUSES, EATING>.Groups;
            var EatingDoofuses2 = GroupCompound<EATING, DOOFUSES>.Groups;

            var NotEatingDoofuses1 = GroupCompound<NOTEATING, DOOFUSES>.Groups;
            var NotEatingDoofuses2 = GroupCompound<DOOFUSES, NOTEATING>.Groups;

            var RedEating1 = GroupCompound<NOTEATING, RED>.Groups;
            var RedEating2 = GroupCompound<RED, NOTEATING>.Groups;

            var NotReadEating1 = GroupCompound<EATING, RED>.Groups;
            var NotReadEating2 = GroupCompound<RED, EATING>.Groups;

            var RedDoofuses1 = GroupCompound<DOOFUSES, RED>.Groups;
            var RedDoofuses2 = GroupCompound<RED, DOOFUSES>.Groups;

            var EatingReadDoofuses1 = GroupCompound<DOOFUSES, RED, EATING>.Groups;
            var EatingReadDoofuses2 = GroupCompound<RED, DOOFUSES, EATING>.Groups;
            var EatingReadDoofuses3 = GroupCompound<DOOFUSES, EATING, RED>.Groups;
            var EatingReadDoofuses4 = GroupCompound<EATING, DOOFUSES, RED>.Groups;
            var EatingReadDoofuses5 = GroupCompound<EATING, RED, DOOFUSES>.Groups;
            var EatingReadDoofuses6 = GroupCompound<RED, EATING, DOOFUSES>.Groups;

            var NoEatingReadDoofuses1 = GroupCompound<DOOFUSES, RED, NOTEATING>.Groups;
            var NoEatingReadDoofuses2 = GroupCompound<RED, DOOFUSES, NOTEATING>.Groups;
            var NoEatingReadDoofuses3 = GroupCompound<DOOFUSES, NOTEATING, RED>.Groups;
            var NoEatingReadDoofuses4 = GroupCompound<NOTEATING, DOOFUSES, RED>.Groups;
            var NoEatingReadDoofuses5 = GroupCompound<NOTEATING, RED, DOOFUSES>.Groups;
            var NoEatingReadDoofuses6 = GroupCompound<RED, NOTEATING, DOOFUSES>.Groups;

            Assert.AreEqual(RedDoofuses1, RedDoofuses2);

            Assert.AreEqual(EatingDoofuses1, EatingDoofuses2);

            Assert.AreEqual(NotEatingDoofuses1, NotEatingDoofuses2);

            Assert.AreEqual(RedEating1, RedEating2);

            Assert.AreEqual(NotReadEating1, NotReadEating2);

            Assert.AreEqual(EatingReadDoofuses1, EatingReadDoofuses2);
            Assert.AreEqual(EatingReadDoofuses2, EatingReadDoofuses3);
            Assert.AreEqual(EatingReadDoofuses3, EatingReadDoofuses4);
            Assert.AreEqual(EatingReadDoofuses4, EatingReadDoofuses5);
            Assert.AreEqual(EatingReadDoofuses5, EatingReadDoofuses6);

            Assert.AreEqual(NoEatingReadDoofuses1, NoEatingReadDoofuses2);
            Assert.AreEqual(NoEatingReadDoofuses2, NoEatingReadDoofuses3);
            Assert.AreEqual(NoEatingReadDoofuses3, NoEatingReadDoofuses4);
            Assert.AreEqual(NoEatingReadDoofuses4, NoEatingReadDoofuses5);
            Assert.AreEqual(NoEatingReadDoofuses5, NoEatingReadDoofuses6);

            Assert.AreNotEqual(EatingReadDoofuses5, NoEatingReadDoofuses6);

            Assert.That(GroupTag<DOOFUSES>.Groups.count, Is.EqualTo(6));
            Assert.That(GroupTag<EATING>.Groups.count, Is.EqualTo(4));
            Assert.That(GroupTag<RED>.Groups.count, Is.EqualTo(6));
            Assert.That(GroupTag<NOTEATING>.Groups.count, Is.EqualTo(4));
        }
    }

    [TestFixture]
    public class GroupCompound3
    {
        public class DOOFUSES : GroupTag<DOOFUSES> { }

        public class EATING : GroupTag<EATING> { }

        public class NOTEATING : GroupTag<NOTEATING> { }

        public class RED : GroupTag<RED> { }

        [TestCase]
        public void TestGroupCompound3()
        {
            var EatingReadDoofuses1 = GroupCompound<DOOFUSES, RED, EATING>.Groups;
            var EatingReadDoofuses2 = GroupCompound<RED, DOOFUSES, EATING>.Groups;
            var EatingReadDoofuses3 = GroupCompound<DOOFUSES, EATING, RED>.Groups;
            var EatingReadDoofuses4 = GroupCompound<EATING, DOOFUSES, RED>.Groups;
            var EatingReadDoofuses5 = GroupCompound<EATING, RED, DOOFUSES>.Groups;
            var EatingReadDoofuses6 = GroupCompound<RED, EATING, DOOFUSES>.Groups;

            var RedDoofuses1 = GroupCompound<DOOFUSES, RED>.Groups;
            var RedDoofuses2 = GroupCompound<RED, DOOFUSES>.Groups;

            var EatingDoofuses1 = GroupCompound<DOOFUSES, EATING>.Groups;
            var EatingDoofuses2 = GroupCompound<EATING, DOOFUSES>.Groups;

            var NotEatingDoofuses1 = GroupCompound<NOTEATING, DOOFUSES>.Groups;
            var NotEatingDoofuses2 = GroupCompound<DOOFUSES, NOTEATING>.Groups;

            var RedEating1 = GroupCompound<NOTEATING, RED>.Groups;
            var RedEating2 = GroupCompound<RED, NOTEATING>.Groups;

            var NotReadEating1 = GroupCompound<EATING, RED>.Groups;
            var NotReadEating2 = GroupCompound<RED, EATING>.Groups;

            Assert.AreEqual(RedDoofuses1, RedDoofuses2);

            Assert.AreEqual(EatingDoofuses1, EatingDoofuses2);

            Assert.AreEqual(NotEatingDoofuses1, NotEatingDoofuses2);

            Assert.AreEqual(RedEating1, RedEating2);

            Assert.AreEqual(NotReadEating1, NotReadEating2);

            Assert.AreEqual(EatingReadDoofuses1, EatingReadDoofuses2);
            Assert.AreEqual(EatingReadDoofuses2, EatingReadDoofuses3);
            Assert.AreEqual(EatingReadDoofuses3, EatingReadDoofuses4);
            Assert.AreEqual(EatingReadDoofuses4, EatingReadDoofuses5);
            Assert.AreEqual(EatingReadDoofuses5, EatingReadDoofuses6);

            Assert.That(GroupTag<DOOFUSES>.Groups.count, Is.EqualTo(5));
            Assert.That(GroupTag<EATING>.Groups.count, Is.EqualTo(4));
            Assert.That(GroupTag<RED>.Groups.count, Is.EqualTo(5));
            Assert.That(GroupTag<NOTEATING>.Groups.count, Is.EqualTo(3));
        }
    }

    [TestFixture]
    public class GroupCompound2
    {
        public class DOOFUSES : GroupTag<DOOFUSES> { }

        public class EATING : GroupTag<EATING> { }

        public class NOTEATING : GroupTag<NOTEATING> { }

        public class RED : GroupTag<RED> { }

        [TestCase]
        public void TestGroupCompound2()
        {
            var RedDoofuses1 = GroupCompound<DOOFUSES, RED>.Groups;
            var RedDoofuses2 = GroupCompound<RED, DOOFUSES>.Groups;

            var EatingDoofuses1 = GroupCompound<DOOFUSES, EATING>.Groups;
            var EatingDoofuses2 = GroupCompound<EATING, DOOFUSES>.Groups;

            var NotEatingDoofuses1 = GroupCompound<NOTEATING, DOOFUSES>.Groups;
            var NotEatingDoofuses2 = GroupCompound<DOOFUSES, NOTEATING>.Groups;

            var RedEating1 = GroupCompound<NOTEATING, RED>.Groups;
            var RedEating2 = GroupCompound<RED, NOTEATING>.Groups;

            var NotReadEating1 = GroupCompound<EATING, RED>.Groups;
            var NotReadEating2 = GroupCompound<RED, EATING>.Groups;

            Assert.AreEqual(RedDoofuses1, RedDoofuses2);

            Assert.AreEqual(EatingDoofuses1, EatingDoofuses2);

            Assert.AreEqual(NotEatingDoofuses1, NotEatingDoofuses2);

            Assert.AreEqual(RedEating1, RedEating2);

            Assert.AreEqual(NotReadEating1, NotReadEating2);

            Assert.That(GroupTag<DOOFUSES>.Groups.count, Is.EqualTo(4));
            Assert.That(GroupTag<EATING>.Groups.count, Is.EqualTo(3));
            Assert.That(GroupTag<RED>.Groups.count, Is.EqualTo(4));
            Assert.That(GroupTag<NOTEATING>.Groups.count, Is.EqualTo(3));
        }
    }
}