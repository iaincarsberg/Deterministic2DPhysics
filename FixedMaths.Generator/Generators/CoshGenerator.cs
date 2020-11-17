using System;
using System.Collections.Generic;
using FixedMaths.Core;
using FixedMaths.Generator.Api;
using FixedMaths.Generator.Helpers;

namespace FixedMaths.Generator.Generators
{
    [RegisterGenerator]
    public class CoshGenerator : IGenerator
    {
        public string OperationName => nameof(Math.Cosh);
        public IDictionary<int, FixedPoint> Generate() => HyperbolicHelper.Generate(Math.Cosh, FixedPoint.Zero);
    }
}