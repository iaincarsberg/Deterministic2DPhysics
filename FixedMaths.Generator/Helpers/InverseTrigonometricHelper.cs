using System;
using System.Collections.Generic;
using FixedMaths.Core;

namespace FixedMaths.Generator.Helpers
{
    public static class InverseTrigonometricHelper
    {
        private const float Range = 1.0f;
        private static readonly float Delta = Range / FixedPoint.One.Value;

        public static IDictionary<int, FixedPoint> Generate(Func<double, double> operation)
        {
            var result = new Dictionary<int, FixedPoint>();

            var sum = -Range;

            for (var i = -FixedPoint.One.Value; i <= FixedPoint.One.Value; i++)
            {
                result[i] = FixedPoint.From(operation(sum));
                sum += Delta;
            }

            return result;
        }
    }
}