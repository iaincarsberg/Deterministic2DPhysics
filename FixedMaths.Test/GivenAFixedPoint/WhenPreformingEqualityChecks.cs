using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenPreformingEqualityChecks
    {
        [Fact]
        public void ShouldMimicFloatNanBehaviour()
        {
            float.NaN.Should().Be(float.NaN);
            FixedPoint.NaN.Should().Be(FixedPoint.NaN);
        }
    }
}