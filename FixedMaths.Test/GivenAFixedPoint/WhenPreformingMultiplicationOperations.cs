using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenPreformingMultiplicationOperations
    {
        [Theory]
        [InlineData(2, 8, 16)]
        [InlineData(3, 7, 21)]
        [InlineData(-3, -7, 21)]
        [InlineData(-3, 7, -21)]
        [InlineData(524287, 2, 524287)]
        [InlineData(-524288, -2, 524287)]
        [InlineData(524288, -2, -524288)]
        [InlineData(-524288, 2, -524288)]
        [InlineData(524287, 524287, 524287)]
        [InlineData(524287, -524288, -524288)]
        [InlineData(-524288, 524287, -524288)]
        [InlineData(-524288, -524288, 524287)]
        public void WholeNumberAdditionFromIntegers(int a, int b, int expected)
        {
            var result = FixedPoint.From(a) * FixedPoint.From(b);

            FixedPoint.ConvertToInteger(result).Should().Be(expected);
        }

        [Theory]
        [InlineData(2.5f, 7.5f, 18.75f)]
        [InlineData(3.5f, 6.5f, 22.75f)]
        [InlineData(-3.5f, -6.5f, 22.75f)]
        [InlineData(-3.5f, 7.5f, -26.25f)]
        [InlineData(524287.0f, 2.0f, 524287.0f)]
        [InlineData(-524287.0f, -2.0f, 524287.0f)]
        [InlineData(524288.0f, -2.0f, -524288.0f)]
        [InlineData(-524288.0f, 2.0f, -524288.0f)]
        [InlineData(10.0f, 0.5f, 5.0f)]
        [InlineData(10.0f, -0.5f, -5.0f)]
        [InlineData(5.0f, float.NaN, float.NaN)]
        [InlineData(-0.8447f, -0.8447f, 0.71351f)]
        [InlineData(-6816.916f, -6816.916f, 524287.0f)]
        public void WholeNumberAdditionFromFloats(float a, float b, float expected)
        {
            var result = FixedPoint.From(a) * FixedPoint.From(b);
            
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
        [InlineData(0.0f, 0.0f)]
        [InlineData(-5.5f, 5.5f)]
        [InlineData(5.5f, -5.5f)]
        public void ShouldReverseSignWhenMultiplyingByNegativeOne(float value, float expected)
        {
            var result = FixedPoint.From(value) * FixedPoint.NegativeOne;
            
            if (float.IsNaN(expected))
            {
                FixedPoint.ConvertToFloat(result).Should().Be(float.NaN);
            }
            else
            {
                FixedPoint.ConvertToFloat(result).Should().BeApproximately(expected, 0.05f);
            }
        }
    }
}