using System;
using System.Collections.Generic;
using FixedMaths.Core;
using FixedMaths.Generator.Api;
using FixedMaths.Generator.Helpers;

namespace FixedMaths.Generator.Generators
{
    [RegisterGenerator]
    public class SinhGenerator : IGenerator
    {
        public string OperationName => nameof(Math.Sinh);
        public IDictionary<int, FixedPoint> Generate() => HyperbolicHelper.Generate(Math.Sinh, FixedPoint.Zero);
    }
}