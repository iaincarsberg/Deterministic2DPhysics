using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenCallingConvertToFloat
    {
        [Theory]
        [InlineData(-5.0f, -5.0f)]
        [InlineData(0.0f, 0.0f)]
        [InlineData(1.85f, 1.85f)]
        [InlineData(524287.0f, 524287.0f)]
        [InlineData(524287.9f, 524287.0f)]
        [InlineData(524288.0f, 524287.0f)]
        [InlineData(524288.99999f, 524287.0f)]
        [InlineData(-524288.0f, -524288.0f)]
        [InlineData(-524288.9f, -524288.0f)]
        [InlineData(-524289.0f, -524288.0f)]
        [InlineData(-524289.99999f, -524288.0f)]
        public void ShouldConvertBackIntoSourceNumber(float value, float expected)
        {
            FixedPoint.ConvertToFloat(FixedPoint.From(value)).Should().BeApproximately(expected, 0.05f);
        }
    }
}