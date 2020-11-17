using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingRoundToNearestStep
    {
        [Theory]
        [InlineData(0.0f, 0.25f, 0.0f)]
        [InlineData(0.1f, 0.25f, 0.0f)]
        [InlineData(0.2f, 0.25f, 0.0f)]
        [InlineData(0.3f, 0.25f, 0.25f)]
        [InlineData(0.4f, 0.25f, 0.25f)]
        [InlineData(0.5f, 0.25f, 0.5f)]
        [InlineData(0.6f, 0.25f, 0.5f)]
        [InlineData(0.7f, 0.25f, 0.5f)]
        [InlineData(0.8f, 0.25f, 0.75f)]
        [InlineData(0.9f, 0.25f, 0.75f)]
        [InlineData(1.0f, 0.25f, 1.0f)]
        [InlineData(1.1f, 0.25f, 1.0f)]
        [InlineData(1.2f, 0.25f, 1.0f)]
        [InlineData(1.3f, 0.25f, 1.25f)]
        [InlineData(0.0f, 10.0f, 0.0f)]
        [InlineData(1.0f, 10.0f, 0.0f)]
        [InlineData(2.0f, 10.0f, 0.0f)]
        [InlineData(3.0f, 10.0f, 0.0f)]
        [InlineData(4.0f, 10.0f, 0.0f)]
        [InlineData(5.0f, 10.0f, 0.0f)]
        [InlineData(6.0f, 10.0f, 0.0f)]
        [InlineData(7.0f, 10.0f, 0.0f)]
        [InlineData(8.0f, 10.0f, 0.0f)]
        [InlineData(9.0f, 10.0f, 0.0f)]
        [InlineData(10.0f, 10.0f, 10.0f)]
        [InlineData(11.0f, 10.0f, 10.0f)]
        [InlineData(12.0f, 10.0f, 10.0f)]
        [InlineData(13.0f, 10.0f, 10.0f)]
        [InlineData(14.0f, 10.0f, 10.0f)]
        [InlineData(15.0f, 10.0f, 10.0f)]
        [InlineData(16.0f, 10.0f, 10.0f)]
        [InlineData(17.0f, 10.0f, 10.0f)]
        [InlineData(18.0f, 10.0f, 10.0f)]
        [InlineData(19.0f, 10.0f, 10.0f)]
        [InlineData(20.0f, 10.0f, 20.0f)]
        public void ShouldRoundTheValueToTheNearestStep(float value, float step, float expected)
        {
            FixedPoint.ConvertToFloat(MathFixedPoint.RoundToNearestStep(FixedPoint.From(value), FixedPoint.From(step))).Should().BeApproximately(expected, 0.05f);
        }
    }
}