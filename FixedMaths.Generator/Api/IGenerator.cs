using System.Collections.Generic;
using FixedMaths.Core;

namespace FixedMaths.Generator.Api
{
    public interface IGenerator
    {
        string OperationName { get; }
        IDictionary<int, FixedPoint> Generate();
    }
}