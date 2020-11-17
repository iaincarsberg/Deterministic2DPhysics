using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingSign
    {
        [Fact]
        public void WhenGivenAPositiveNumber()
        {
            MathFixedPoint.Sign(FixedPoint.From(10)).Should().Be(FixedPoint.One);
        }
        
        [Fact]
        public void WhenGivenANegativeNumber()
        {
            MathFixedPoint.Sign(FixedPoint.From(-10)).Should().Be(FixedPoint.NegativeOne);
        }
        
        [Fact]
        public void WhenGivenZero()
        {
            MathFixedPoint.Sign(FixedPoint.From(0)).Should().Be(FixedPoint.Zero);
        }
    }
}