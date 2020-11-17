using System;
using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingCeiling
    {
        [Theory]
        [InlineData(-2.0d)]
        [InlineData(-1.9d)]
        [InlineData(-1.8d)]
        [InlineData(-1.7d)]
        [InlineData(-1.6d)]
        [InlineData(-1.5d)] 
        [InlineData(-1.4d)]
        [InlineData(-1.3d)]
        [InlineData(-1.2d)]
        [InlineData(-1.1d)]
        [InlineData(-1.0d)]
        [InlineData(-0.9d)]
        [InlineData(-0.8d)]
        [InlineData(-0.7d)]
        [InlineData(-0.6d)]
        [InlineData(-0.5d)]
        [InlineData(-0.4d)]
        [InlineData(-0.3d)]
        [InlineData(-0.2d)]
        [InlineData(-0.1d)]
        [InlineData(0.0d)]
        [InlineData(0.1d)]
        [InlineData(0.2d)]
        [InlineData(0.3d)]
        [InlineData(0.4d)]
        [InlineData(0.5d)]
        [InlineData(0.6d)]
        [InlineData(0.7d)]
        [InlineData(0.8d)]
        [InlineData(0.9d)]
        [InlineData(1.0d)]
        [InlineData(1.1d)]
        [InlineData(1.2d)]
        [InlineData(1.3d)]
        [InlineData(1.4d)]
        [InlineData(1.5d)]
        [InlineData(1.6d)]
        [InlineData(1.7d)]
        [InlineData(1.8d)]
        [InlineData(1.9d)]
        [InlineData(2.0d)]
        public void ShouldCeilingToTheSameValueAsTheMathLibrary(double value)
        {
            FixedPoint.ConvertToDouble(MathFixedPoint.Ceiling(FixedPoint.From(value))).Should().BeApproximately(Math.Ceiling(value), 0.01d);
        }
    }
}