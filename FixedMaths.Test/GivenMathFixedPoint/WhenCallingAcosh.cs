using System;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FixedMaths.Test.TestGenerator;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingAcosh : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingAcosh(ITestOutputHelper testOutputHelper)
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
        [ClassData(typeof(PositiveOnlyDegreeDataGenerator))]
        public void ShouldConvertToAbsoluteValue(double value, double? expected = null)
        {
            if (expected.HasValue && double.IsNaN(expected.Value))
            {
                FixedPoint.ConvertToDouble(MathFixedPoint.Acosh(FixedPoint.From(value))).Should().Be(double.NaN);
            }
            else
            {
                _testOutputHelper.WriteLine($"{FixedPoint.ConvertToFloat(FixedPoint.From(value)):0.0} - Fixed.Acosh: {MathFixedPoint.Acosh(FixedPoint.From(value))}");
                _testOutputHelper.WriteLine($"{value:0.0} - Math.Acosh:  {Math.Acosh(value)}");
                
                FixedPoint.ConvertToDouble(MathFixedPoint.Acosh(FixedPoint.From(value))).Should().BeApproximately(Math.Acosh(value), 0.05f);
            }
        }
    }
}