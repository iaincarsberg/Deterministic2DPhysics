using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingMagnitude : IClassFixture<ProcessedTableRepositoryFixture>
    {
        [Theory]
        [InlineData(-6816.9160f, -0.8447f, 524287.0f)]
        public void ShouldCalculateTheMagnitude(float x, float y, float expected)
        {
            var magnitude = MathFixedPoint.Magnitude(FixedPointVector2.From(FixedPoint.From(x), FixedPoint.From(y)));

            FixedPoint.ConvertToFloat(magnitude).Should().BeApproximately(expected, 0.01f);
        }
    }
}