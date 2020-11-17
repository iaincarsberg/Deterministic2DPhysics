using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenCallingConvertToInteger
    {
        [Theory]
        [InlineData(-5, -5)]
        [InlineData(0, 0)]
        [InlineData(100, 100)]
        [InlineData(524287, 524287)]
        [InlineData(524288, 524287)]
        [InlineData(-524288, -524288)]
        [InlineData(-524289, -524288)]
        public void ShouldConvertBackIntoSourceNumber(int value, int expected)
        {
            FixedPoint.ConvertToFloat(FixedPoint.From(value)).Should().Be(expected);
        }
    }
}