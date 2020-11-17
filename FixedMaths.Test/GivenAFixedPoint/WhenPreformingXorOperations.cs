using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenPreformingXorOperations
    {
        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        public void ShouldProduceTheSameResultAtCSharpsXorOperator(int a, int b)
        {
            FixedPoint.ConvertToInteger(FixedPoint.From(a) ^ FixedPoint.From(b)).Should().Be(a ^ b);
        }
    }
}