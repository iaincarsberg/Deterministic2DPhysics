using System;
using FixedMaths.Core;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingAtan2
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingAtan2(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(-1.0d, -1.0d)]
        [InlineData(0.0d, 0.0d)]
        [InlineData(0.0d, 0.1d)]
        [InlineData(0.1d, 0.0d)]
        [InlineData(0.1d, 0.1d)]
        [InlineData(1.0d, 1.0d)]
        [InlineData(Math.PI, 1.0d)]
        [InlineData(Math.PI, 2.0d)]
        [InlineData(Math.PI, Math.PI)]
        [InlineData(Math.PI, -Math.PI)]
        [InlineData(Math.PI, 5.0d)]
        [InlineData(5.0d, 3.0d)]
        public void ShouldConvertToAbsoluteValue(double x, double y)
        {
            _testOutputHelper.WriteLine($"{FixedPoint.From(x)} - {FixedPoint.From(y)} = {FixedPoint.ConvertToDouble(MathFixedPoint.Atan2(FixedPoint.From(y), FixedPoint.From(x)))}");
            _testOutputHelper.WriteLine($"{x} - {y} = {Math.Atan2(y, x)}");
            
            FixedPoint.ConvertToDouble(MathFixedPoint.Atan2(FixedPoint.From(y), FixedPoint.From(x))).Should().BeApproximately(Math.Atan2(y, x), 0.1f);
        }
    }
}