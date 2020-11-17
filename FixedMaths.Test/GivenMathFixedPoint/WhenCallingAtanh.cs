using System;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingAtanh : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingAtanh(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(-1.1d, double.NaN)]
        [InlineData(-1.0d, double.NaN)]
        [InlineData(-0.99d)]
        [InlineData(-0.75d)]
        [InlineData(-0.5d)]
        [InlineData(-0.25d)]
        [InlineData(0.0d)]
        [InlineData(0.25d)]
        [InlineData(0.5d)]
        [InlineData(0.75d)]
        [InlineData(0.99d)]
        [InlineData(1.0d, double.NaN)]
        [InlineData(1.1d, double.NaN)]
        public void ShouldConvertToAbsoluteValue(double value, double? expected = null)
        {
            if (expected.HasValue && double.IsNaN(expected.Value))
            {
                FixedPoint.ConvertToDouble(MathFixedPoint.Atanh(FixedPoint.From(value))).Should().Be(double.NaN);
            }
            else
            {
                _testOutputHelper.WriteLine($"{FixedPoint.ConvertToFloat(FixedPoint.From(value)):0.0} - Fixed.Atanh: {MathFixedPoint.Atanh(FixedPoint.From(value))}");
                _testOutputHelper.WriteLine($"{value:0.0} - Math.Atanh:  {Math.Atanh(value)}");
                
                FixedPoint.ConvertToDouble(MathFixedPoint.Atanh(FixedPoint.From(value))).Should().BeApproximately(Math.Atanh(value), 0.05f);
            }
        }
    }
}