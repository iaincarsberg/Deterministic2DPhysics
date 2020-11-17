using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenCallingConvertToDouble
    {
        [Theory]
        [InlineData(-5.0d, -5.0d)]
        [InlineData(0.0d, 0.0d)]
        [InlineData(1.85d, 1.85d)]
        [InlineData(524287.0d, 524287.0d)]
        [InlineData(524287.99999d, 524287.0d)]
        [InlineData(524288.0d, 524287.0d)]
        [InlineData(524288.99999d, 524287.0d)]
        [InlineData(-524288.0d, -524288.0d)]
        [InlineData(-524288.99999d, -524288.0d)]
        [InlineData(-524289.0d, -524288.0d)]
        [InlineData(-524289.99999d, -524288.0d)]
        public void ShouldConvertBackIntoSourceNumber(double value, double expected)
        {
            FixedPoint.ConvertToDouble(FixedPoint.From(value)).Should().BeApproximately(expected, 0.05f);
        }
    }
}