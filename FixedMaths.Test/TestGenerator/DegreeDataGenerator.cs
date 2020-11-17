using System;
using System.Collections;
using System.Collections.Generic;

namespace FixedMaths.Test.TestGenerator
{
    public class DegreeDataGenerator : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            for (var i = -720; i < 720; i++)
            {
                yield return new object[] { i, (Math.Abs(i / 360) + 1) * 0.05d };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}