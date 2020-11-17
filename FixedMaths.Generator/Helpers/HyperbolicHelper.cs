using System;
using System.Collections.Generic;
using FixedMaths.Core;

namespace FixedMaths.Generator.Helpers
{
    public static class HyperbolicHelper
    {
        public static Dictionary<int, FixedPoint> Generate(Func<double, double> operation, FixedPoint? minValue = null, FixedPoint? maxValue = null)
        {
            var result = new Dictionary<int, FixedPoint>();
            var max = (maxValue ?? FixedPoint.MaxValue).Value;
            var i = (minValue ?? FixedPoint.One).Value;
            var step = FixedPoint.FromExplicit(MathFixedPointConstants.HyperbolicHighResolutionStep);
            
            while (i <= max)
            {
                result[MathFixedPoint.RoundToNearestStep(FixedPoint.FromExplicit(i), step).Value] = FixedPoint.From(operation(FixedPoint.ConvertToDouble(FixedPoint.FromExplicit(i))));
                
                try
                {
                    checked
                    {
                        if (i >= MathFixedPointConstants.HyperbolicHighResolution && i < MathFixedPointConstants.HyperbolicMediumResolution)
                        {
                            i += MathFixedPointConstants.HyperbolicHighResolutionStep;
                            step = FixedPoint.FromExplicit(MathFixedPointConstants.HyperbolicHighResolutionStep);
                        }
                        else if (i >= MathFixedPointConstants.HyperbolicMediumResolution && i < MathFixedPointConstants.HyperbolicLowResolution)
                        {
                            i += MathFixedPointConstants.HyperbolicMediumResolutionStep;
                            step = FixedPoint.FromExplicit(MathFixedPointConstants.HyperbolicMediumResolutionStep);
                        }
                        else
                        {
                            i += MathFixedPointConstants.HyperbolicLowResolutionStep;
                            step = FixedPoint.FromExplicit(MathFixedPointConstants.HyperbolicLowResolutionStep);
                        }
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }

            return result;
        }
    }
}