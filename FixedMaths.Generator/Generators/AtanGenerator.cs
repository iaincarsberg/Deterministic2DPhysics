using System;
using System.Collections.Generic;
using FixedMaths.Core;
using FixedMaths.Generator.Api;
using FixedMaths.Generator.Helpers;

namespace FixedMaths.Generator.Generators
{
    [RegisterGenerator]
    public class AtanGenerator : IGenerator
    {
        public string OperationName => nameof(Math.Atan);
        public IDictionary<int, FixedPoint> Generate() => InverseTrigonometricHelper.Generate(Math.Atan);
    }
}