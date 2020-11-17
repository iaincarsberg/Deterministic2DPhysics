using System;
using System.Collections.Generic;
using FixedMaths.Core;

namespace FixedMaths.Generator.Helpers
{
    public static class TrigonometricHelper
    {
        public static IDictionary<int, FixedPoint> Generate(Func<double, double> operation)
        {
            var result = new Dictionary<int, FixedPoint>();
            
            for (var i = 0; i < MathFixedPointConstants.TrigonometricSteps; i++)
            {
                var theta = Math.PI * i / (MathFixedPointConstants.TrigonometricSteps - 1);
                result[i] = FixedPoint.From(operation(theta));
            }

            return result;
        }
    }
}