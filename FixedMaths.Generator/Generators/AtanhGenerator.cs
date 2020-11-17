using System;
using System.Collections.Generic;
using FixedMaths.Core;
using FixedMaths.Generator.Api;
using FixedMaths.Generator.Helpers;

namespace FixedMaths.Generator.Generators
{
    [RegisterGenerator]
    public class AtanhGenerator : IGenerator
    {
        public string OperationName => nameof(Math.Atanh);

        public IDictionary<int, FixedPoint> Generate()
        {
            var data = InverseTrigonometricHelper.Generate(Math.Atanh);

            data.Remove(-FixedPoint.One.Value);
            data.Remove(FixedPoint.One.Value);

            return data;
        }
    }
}