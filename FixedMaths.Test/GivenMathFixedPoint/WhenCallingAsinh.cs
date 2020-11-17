using System;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingAsinh : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingAsinh(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(-1.0d, double.NaN)]
        [InlineData(0.0d, double.NaN)]
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
        // Check big angles
        [InlineData(45.0d)]
        [InlineData(90.0d)]
        [InlineData(135.0d)]
        [InlineData(180.0d)]
        [InlineData(225.0d)]
        [InlineData(270.0d)]
        [InlineData(315.0d)]
        [InlineData(360.0d)]
        [InlineData(405.0d)]
        [InlineData(450.0d)]
        [InlineData(495.0d)]
        [InlineData(540.0d)]
        [InlineData(585.0d)]
        [InlineData(630.0d)]
        [InlineData(675.0d)]
        [InlineData(720.0d)]
        [InlineData(765.0d)]
        [InlineData(810.0d)]
        [InlineData(855.0d)]
        [InlineData(900.0d)]
        public void ShouldConvertToAbsoluteValue(double value, double? expected = null)
        {
            if (expected.HasValue && double.IsNaN(expected.Value))
            {
                FixedPoint.ConvertToDouble(MathFixedPoint.Acosh(FixedPoint.From(value))).Should().Be(double.NaN);
            }
            else
            {
                _testOutputHelper.WriteLine($"{FixedPoint.ConvertToFloat(FixedPoint.From(value)):0.0} - Fixed.Asinh: {MathFixedPoint.Asinh(FixedPoint.From(value))}");
                _testOutputHelper.WriteLine($"{value:0.0} - Math.Asinh:  {Math.Asinh(value)}");
                
                FixedPoint.ConvertToDouble(MathFixedPoint.Asinh(FixedPoint.From(value))).Should().BeApproximately(Math.Asinh(value), 0.05f);
            }
        }
    }
}