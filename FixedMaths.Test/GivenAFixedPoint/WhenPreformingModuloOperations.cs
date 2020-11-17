using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenPreformingModuloOperations
    {
        [Theory]
        [InlineData(-2.0f, 2.0f)]
        [InlineData(-1.5f, 2.0f)]
        [InlineData(-1.0f, 2.0f)]
        [InlineData(-0.5f, 2.0f)]
        [InlineData(0.0f, 2.0f)]
        [InlineData(0.5f, 2.0f)]
        [InlineData(1.0f, 2.0f)]
        [InlineData(1.5f, 2.0f)]
        [InlineData(2.0f, 2.0f)]
        public void ShouldPreformModuloOperationsSimilarToCSharp(float value, float moduloAmount)
        {
            FixedPoint.ConvertToFloat(FixedPoint.From(value) % FixedPoint.From(moduloAmount)).Should().BeApproximately(value % moduloAmount, 0.05f);
        }
    }
}