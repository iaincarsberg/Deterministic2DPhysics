using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingClamp
    {
        [Theory]
        [InlineData(8.0f, 5.0f, 10.0f, 8.0f)]
        [InlineData(3.0f, 5.0f, 10.0f, 5.0f)]
        [InlineData(15.0f, 5.0f, 10.0f, 10.0f)]
        public void ShouldClampValue(float value, float min, float max, float expected)
        {
            var result = MathFixedPoint.Clamp(FixedPoint.From(value), FixedPoint.From(min), FixedPoint.From(max));

            FixedPoint.ConvertToFloat(result).Should().BeApproximately(expected, 0.01f);
        }
    }
}