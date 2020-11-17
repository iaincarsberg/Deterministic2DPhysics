using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenCallingToString
    {
        [Fact]
        public void ShouldReturnFloatValue()
        {
            FixedPoint.From(1.23456).ToString().Should().Be("1.2347");
        }
    }
}