using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenPreformingSubtractionOperations
    {
        [Theory]
        [InlineData(2, 8, -6)]
        [InlineData(3, 7, -4)]
        [InlineData(-3, -7, 4)]
        [InlineData(-3, 7, -10)]
        [InlineData(-10, 524288, -524288)]
        [InlineData(10, -524288, 524287)]
        public void WholeNumberSubtractionFromIntegers(int a, int b, int expected)
        {
            var result = FixedPoint.From(a) - FixedPoint.From(b);

            FixedPoint.ConvertToInteger(result).Should().Be(expected);
        }

        [Theory]
        [InlineData(2.5f, 7.5f, -5.0f)]
        [InlineData(3.5f, 6.5f, -3.0f)]
        [InlineData(-3.5f, -6.5f, 3.0f)]
        [InlineData(-3.5f, 7.5f, -11.0f)]
        [InlineData(-10.0f, 524287.0f, -524288.0f)]
        [InlineData(10.0f, -524288.0f, 524287.0f)]
        [InlineData(5.0f, float.NaN, float.NaN)]
        [InlineData(-524288.0f, 0.9f, -524288.0f)]
        public void WholeNumberSubtractionFromFloats(float a, float b, float expected)
        {
            var result = FixedPoint.From(a) - FixedPoint.From(b);

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
        [InlineData(0.0f)]
        [InlineData(524287.0f)]
        [InlineData(-524287.0f)]
        public void ShouldInverseGivenValue(float value)
        {
            var result = -FixedPoint.From(value);

            FixedPoint.ConvertToFloat(result).Should().BeApproximately(-value, 0.01f);
        }
    }
}