using System;
using System.Collections.Generic;
using FixedMaths.Core;
using FixedMaths.Generator.Api;
using FixedMaths.Generator.Helpers;

namespace FixedMaths.Generator.Generators
{
    [RegisterGenerator]
    public class AsinGenerator : IGenerator
    {
        public string OperationName => nameof(Math.Asin);
        public IDictionary<int, FixedPoint> Generate() => InverseTrigonometricHelper.Generate(Math.Asin);
    }
}