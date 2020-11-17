using System;
using System.Collections.Generic;
using FixedMaths.Core;
using FixedMaths.Generator.Api;

namespace FixedMaths.Generator.Generators
{
    [RegisterGenerator]
    public class SqrtGenerator : IGenerator
    {
        public string OperationName => nameof(Math.Sqrt);
        public IDictionary<int, FixedPoint> Generate()
        {
            var result = new Dictionary<int, FixedPoint>();
            var max = FixedPoint.MaxValue.Value;
            var i = 0;
            var step = FixedPoint.FromExplicit(MathFixedPointConstants.SqrtHighResolutionStep);
            
            while (i <= max)
            {
                result[MathFixedPoint.RoundToNearestStep(FixedPoint.FromExplicit(i), step).Value] = FixedPoint.From(Math.Sqrt(FixedPoint.ConvertToDouble(FixedPoint.FromExplicit(i))));
                
                try
                {
                    checked
                    {
                        if (i >= MathFixedPointConstants.SqrtHighResolution && i < MathFixedPointConstants.SqrtMediumResolution)
                        {
                            i += MathFixedPointConstants.SqrtHighResolutionStep;
                            step = FixedPoint.FromExplicit(MathFixedPointConstants.SqrtHighResolutionStep);
                        }
                        else if (i >= MathFixedPointConstants.SqrtMediumResolution && i < MathFixedPointConstants.SqrtLowResolution)
                        {
                            i += MathFixedPointConstants.SqrtMediumResolutionStep;
                            step = FixedPoint.FromExplicit(MathFixedPointConstants.SqrtMediumResolutionStep);
                        }
                        else
                        {
                            i += MathFixedPointConstants.SqrtLowResolutionStep;
                            step = FixedPoint.FromExplicit(MathFixedPointConstants.SqrtLowResolutionStep);
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