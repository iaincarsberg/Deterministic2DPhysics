using System;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingAtan : IClassFixture<ProcessedTableRepositoryFixture>
    {
        [Theory]
        [InlineData(-1.1d, double.NaN)]
        [InlineData(-1.0d, -1.0d)]
        [InlineData(-0.75d, -0.75d)]
        [InlineData(-0.5d, -0.5d)]
        [InlineData(-0.25d, -0.25d)]
        [InlineData(0.0d, 0.0d)]
        [InlineData(0.25d, 0.25d)]
        [InlineData(0.5d, 0.5d)]
        [InlineData(0.75d, 0.75d)]
        [InlineData(1.0d, 1.0d)]
        [InlineData(1.1d, double.NaN)]
        public void ShouldConvertToAbsoluteValue(double value, double expected)
        {
            if (double.IsNaN(expected))
            {
                FixedPoint.ConvertToDouble(MathFixedPoint.Atan(FixedPoint.From(value))).Should().Be(double.NaN);
            }
            else
            {
                FixedPoint.ConvertToDouble(MathFixedPoint.Atan(FixedPoint.From(value))).Should().BeApproximately(Math.Atan(expected), 0.05f);
            }
        }
    }
}