using System;
using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenPreformingDivisionOperations
    {
        [Theory]
        [InlineData(8, 2, 4)]
        [InlineData(7, 2, 3)]
        [InlineData(-7, -3, 2)]
        [InlineData(-7, 4, -2)]
        [InlineData(524287, 2, 262143)]
        [InlineData(-524288, -2, 262144)]
        [InlineData(524287, -2, -262144)]
        [InlineData(-524288, 2, -262144)]
        public void WholeNumberAdditionFromIntegers(int a, int b, int expected)
        {
            var result = FixedPoint.From(a) / FixedPoint.From(b);
            
            FixedPoint.ConvertToInteger(result).Should().Be(expected);
            FixedPoint.ConvertToFloat(result).Should().BeApproximately(expected, 0.51f);
        }

        [Theory]
        [InlineData(7.5f, 2.5f, 3.0f)]
        [InlineData(6.5f, 3.5f, 1.85f)]
        [InlineData(-6.5f, -3.5f, 1.85f)]
        [InlineData(7.5f, -3.5f, -2.14f)]
        [InlineData(524287.0f, 2.0f, 262143.5f)]
        [InlineData(-524287.0f, -2.0f, 262143.5f)]
        [InlineData(524287.0f, -2.0f, -262143.5f)]
        [InlineData(-524288.0f, 2.0f, -262144.0f)]
        [InlineData(3.0f, 2.0f, 1.5f)]
        [InlineData(5.0f, float.NaN, float.NaN)]
        public void WholeNumberAdditionFromFloats(float a, float b, float expected)
        {
            var result = FixedPoint.From(a) / FixedPoint.From(b);
            
            if (float.IsNaN(expected))
            {
                FixedPoint.ConvertToFloat(result).Should().Be(float.NaN);
            }
            else
            {
                FixedPoint.ConvertToFloat(result).Should().BeApproximately(expected, 0.05f);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        public void WhenDividingByZero(int value)
        {
            Func<FixedPoint> operation = () => FixedPoint.From(value) / FixedPoint.From(0);

            operation.Should().Throw<DivideByZeroException>();
        }
    }
}