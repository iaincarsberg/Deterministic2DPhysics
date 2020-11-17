using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPointVector2
{
    public class WhenCallingInterpolate
    {
        [Theory]
        // positive
        [InlineData(0.0f, 1.0f, 0.0f, 0.0f)]
        [InlineData(0.0f, 1.0f, 0.25f, 0.25f)]
        [InlineData(0.0f, 1.0f, 0.5f, 0.5f)]
        [InlineData(0.0f, 1.0f, 0.75f, 0.75f)]
        [InlineData(0.0f, 1.0f, 1.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 0.25f, 0.75f)]
        [InlineData(1.0f, 0.0f, 0.5f, 0.5f)]
        [InlineData(1.0f, 0.0f, 0.75f, 0.25f)]
        [InlineData(1.0f, 0.0f, 1.0f, 0.0f)]
        [InlineData(5.0f, 10.0f, 0.0f, 5.0f)]
        [InlineData(5.0f, 10.0f, 0.2f, 6.0f)]
        [InlineData(5.0f, 10.0f, 0.4f, 7.0f)]
        [InlineData(5.0f, 10.0f, 0.6f, 8.0f)]
        [InlineData(5.0f, 10.0f, 0.8f, 9.0f)]
        [InlineData(5.0f, 10.0f, 1.0f, 10.0f)]
        // negative
        [InlineData(0.0f, -1.0f, 0.0f, -0.0f)]
        [InlineData(0.0f, -1.0f, 0.25f, -0.25f)]
        [InlineData(0.0f, -1.0f, 0.5f, -0.5f)]
        [InlineData(0.0f, -1.0f, 0.75f, -0.75f)]
        [InlineData(0.0f, -1.0f, 1.0f, -1.0f)]
        [InlineData(-1.0f, 0.0f, 0.0f, -1.0f)]
        [InlineData(-1.0f, 0.0f, 0.25f, -0.75f)]
        [InlineData(-1.0f, 0.0f, 0.5f, -0.5f)]
        [InlineData(-1.0f, 0.0f, 0.75f, -0.25f)]
        [InlineData(-1.0f, 0.0f, 1.0f, 0.0f)]
        public void ShouldInterpolateTheVectorAlongTheYAxis(float startingY, float targetY, float time, float result)
        {
            var (x, y) = FixedPointVector2.Interpolate(
                FixedPointVector2.From(FixedPoint.Zero, FixedPoint.From(startingY)), 
                FixedPointVector2.From(FixedPoint.Zero, FixedPoint.From(targetY)), 
                FixedPoint.From(time));

            x.Should().BeApproximately(0.0f, 0.05f);
            y.Should().BeApproximately(result, 0.05f);
        }
        
        [Theory]
        // positive
        [InlineData(0.0f, 1.0f, 0.0f, 0.0f)]
        [InlineData(0.0f, 1.0f, 0.25f, 0.25f)]
        [InlineData(0.0f, 1.0f, 0.5f, 0.5f)]
        [InlineData(0.0f, 1.0f, 0.75f, 0.75f)]
        [InlineData(0.0f, 1.0f, 1.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 0.25f, 0.75f)]
        [InlineData(1.0f, 0.0f, 0.5f, 0.5f)]
        [InlineData(1.0f, 0.0f, 0.75f, 0.25f)]
        [InlineData(1.0f, 0.0f, 1.0f, 0.0f)]
        [InlineData(5.0f, 10.0f, 0.0f, 5.0f)]
        [InlineData(5.0f, 10.0f, 0.2f, 6.0f)]
        [InlineData(5.0f, 10.0f, 0.4f, 7.0f)]
        [InlineData(5.0f, 10.0f, 0.6f, 8.0f)]
        [InlineData(5.0f, 10.0f, 0.8f, 9.0f)]
        [InlineData(5.0f, 10.0f, 1.0f, 10.0f)]
        // negative
        [InlineData(0.0f, -1.0f, 0.0f, -0.0f)]
        [InlineData(0.0f, -1.0f, 0.25f, -0.25f)]
        [InlineData(0.0f, -1.0f, 0.5f, -0.5f)]
        [InlineData(0.0f, -1.0f, 0.75f, -0.75f)]
        [InlineData(0.0f, -1.0f, 1.0f, -1.0f)]
        [InlineData(-1.0f, 0.0f, 0.0f, -1.0f)]
        [InlineData(-1.0f, 0.0f, 0.25f, -0.75f)]
        [InlineData(-1.0f, 0.0f, 0.5f, -0.5f)]
        [InlineData(-1.0f, 0.0f, 0.75f, -0.25f)]
        [InlineData(-1.0f, 0.0f, 1.0f, 0.0f)]
        public void ShouldInterpolateTheVectorAlongTheXAxis(float startingX, float targetX, float time, float result)
        {
            var (x, y) = FixedPointVector2.Interpolate(
                FixedPointVector2.From(FixedPoint.From(startingX), FixedPoint.Zero), 
                FixedPointVector2.From(FixedPoint.From(targetX), FixedPoint.Zero), 
                FixedPoint.From(time));

            x.Should().BeApproximately(result, 0.05f);
            y.Should().BeApproximately(0.0f, 0.05f);
        }
    }
}