using System;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingAbs : IClassFixture<ProcessedTableRepositoryFixture>
    {
        [Theory]
        [InlineData(0)]
        [InlineData(5.5)]
        [InlineData(-5.5)]
        public void ShouldConvertToAbsoluteValue(float value)
        {
            FixedPoint.ConvertToFloat(MathFixedPoint.Abs(FixedPoint.From(value))).Should().BeApproximately(Math.Abs(value), 0.05f);
        }
    }
}