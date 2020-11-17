using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenPreformingGreaterThanOperations
    {
        [Theory]
        [InlineData(0.0f, 0.0f, false)]
        [InlineData(1.0f, 0.0f, true)]
        [InlineData(0.0f, 1.0f, false)]
        [InlineData(-1.0f, 0.0f, false)]
        [InlineData(0.0f, -1.0f, true)]
        [InlineData(-1.0f, 1.0f, false)]
        [InlineData(1.0f, -1.0f, true)]
        public void ShouldBeGreaterThan(float a, float b, bool expected)
        {
            (FixedPoint.From(a) > FixedPoint.From(b)).Should().Be(expected);
        }
        
        [Theory]
        [InlineData(0.0f, 0.0f, true)]
        [InlineData(1.0f, 0.0f, true)]
        [InlineData(0.0f, 1.0f, false)]
        [InlineData(-1.0f, 0.0f, false)]
        [InlineData(0.0f, -1.0f, true)]
        [InlineData(-1.0f, 1.0f, false)]
        [InlineData(1.0f, -1.0f, true)]
        public void ShouldBeGreaterThanOrEqual(float a, float b, bool expected)
        {
            (FixedPoint.From(a) >= FixedPoint.From(b)).Should().Be(expected);
        }
    }
}