using System;
using System.Collections.Generic;
using FixedMaths.Core;
using FixedMaths.Generator.Api;
using FixedMaths.Generator.Helpers;

namespace FixedMaths.Generator.Generators
{
    [RegisterGenerator]
    public class CosGenerator : IGenerator
    {
        public string OperationName => nameof(Math.Cos);
        public IDictionary<int, FixedPoint> Generate() => TrigonometricHelper.Generate(Math.Cos);
    }
}