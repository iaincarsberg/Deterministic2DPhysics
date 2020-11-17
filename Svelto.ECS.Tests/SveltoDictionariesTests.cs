using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Svelto.DataStructures;

namespace Svelto.Common.Tests.Datastructures
{
    [TestFixture]
    public class TestDictionaries
    {
        struct Test
        {
            public int i;

            public Test(int i) : this() { this.i = i; }
        }

        [TestCase]
        public void TestFasterDictionary()
        {
            FasterDictionary<int, Test> test           = new FasterDictionary<int, Test>();
            uint                        dictionarysize = 10000;
            int[]                       numbers        = new int[dictionarysize];
            for (int i = 1; i < dictionarysize; i++)
                numbers[i] = numbers[i - 1] + i * HashHelpers.ExpandPrime((int) dictionarysize);

            for (int i = 0; i < dictionarysize; i++)
                test[i] = new Test(numbers[i]);

            for (int i = 0; i < dictionarysize; i++)
                if (test[i].i != numbers[i])
                    throw new Exception();

            for (int i = 0; i < dictionarysize; i += 2)
                if (test.Remove(i) == false)
                    throw new Exception();

            test.Trim();

            for (int i = 0; i < dictionarysize; i++)
                test[i] = new Test(numbers[i]);

            for (int i = 1; i < dictionarysize - 1; i += 2)
                if (test[i].i != numbers[i])
                    throw new Exception();

            for (int i = 0; i < dictionarysize; i++)
                if (test[i].i != numbers[i])
                    throw new Exception();

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                if (test.Remove(i) == false)
                    throw new Exception();

            test.Trim();

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                test[i] = new Test(numbers[i]);

            for (int i = 0; i < dictionarysize; i++)
                if (test[i].i != numbers[i])
                    throw new Exception();

            for (int i = 0; i < dictionarysize; i++)
                if (test.Remove(i) == false)
                    throw new Exception();

            for (int i = 0; i < dictionarysize; i++)
                if (test.Remove(i) == true)
                    throw new Exception();

            test.Trim();

            test.Clear();
            for (int i = 0; i < dictionarysize; i++)
                test[numbers[i]] = new Test(i);

            for (int i = 0; i < dictionarysize; i++)
            {
                Test JapaneseCalendar = test[numbers[i]];
                if (JapaneseCalendar.i != i)
                    throw new Exception("read back test failed");
            }
        }

        [Test]
        public void TestReadBack()
        {
            FasterDictionary<int, Test> test           = new FasterDictionary<int, Test>();
            uint                        dictionarysize = 10000;
            int[]                       numbers        = new int[dictionarysize];
            for (int i = 1; i < dictionarysize; i++)
                numbers[i] = numbers[i - 1] + i * HashHelpers.ExpandPrime((int) dictionarysize);

            for (int i = 0; i < dictionarysize; i++)
                test[numbers[i]] = new Test(i);

            for (int i = 0; i < dictionarysize; i++)
            {
                Test JapaneseCalendar = test[numbers[i]];
                if (JapaneseCalendar.i != i)
                    throw new Exception("read back test failed");
            }
        }

        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void TestSveltoDictionary(int dictionarysize)
        {
            SveltoDictionary<int, Test, NativeStrategy<FasterDictionaryNode<int>>, NativeStrategy<Test>, NativeStrategy<int>> test =
                new SveltoDictionary<int, Test, NativeStrategy<FasterDictionaryNode<int>>, NativeStrategy<Test>, NativeStrategy<int>>(1);

            int[] numbers = new int[dictionarysize];

            for (int i = 1; i < dictionarysize; i++)
                numbers[i] = numbers[i - 1] + i * HashHelpers.ExpandPrime((int) dictionarysize);

            for (int i = 0; i < dictionarysize; i++)
                test[i] = new Test(numbers[i]);

            for (int i = 0; i < dictionarysize; i++)
                if (test[i].i != numbers[i])
                    throw new Exception();

            for (int i = 0; i < dictionarysize; i += 2)
                if (test.Remove(i) == false)
                    throw new Exception();

            test.Clear();

            for (int i = 0; i < dictionarysize; i++)
                test[i] = new Test(numbers[i]);

            for (int i = 1; i < dictionarysize - 1; i += 2)
                if (test[i].i != numbers[i])
                    throw new Exception();

            for (int i = 0; i < dictionarysize; i++)
                if (test[i].i != numbers[i])
                    throw new Exception();

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                if (test.Remove(i) == false)
                    throw new Exception();

            test.Clear();

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                test[i] = new Test(numbers[i]);

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                if (test[i].i != numbers[i])
                    throw new Exception();

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                if (test.Remove(i) == false)
                    throw new Exception();

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                if (test.Remove(i) == true)
                    throw new Exception();

            test.Clear();

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                test[i] = new Test(numbers[i]);

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                if (test.Remove(i) == false)
                    throw new Exception();

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                test[i] = new Test(numbers[i]);

            for (int i = (int) (dictionarysize - 1); i >= 0; i -= 3)
                if (test[i].i != numbers[i])
                    throw new Exception();

            test.Clear();
            for (int i = 0; i < dictionarysize; i++)
                test[numbers[i]] = new Test(i);

            for (int i = 0; i < dictionarysize; i++)
            {
                Test JapaneseCalendar = test[numbers[i]];
                if (JapaneseCalendar.i != i)
                    throw new Exception("read back test failed");
            }

            test.Clear();

            for (int i = 0; i < dictionarysize; i++)
                test[numbers[i]] = new Test(i);

            for (int i = 0; i < dictionarysize; i++)
            {
                Test JapaneseCalendar = test[numbers[i]];
                if (JapaneseCalendar.i != i)
                    throw new Exception("read back test failed");
            }

            test.Dispose();
        }
    }
}