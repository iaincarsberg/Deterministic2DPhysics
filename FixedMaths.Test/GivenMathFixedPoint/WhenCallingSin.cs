using System;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FixedMaths.Test.TestGenerator;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingSin : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingSin(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        // Check angels that aren't going to be included in the 1024 steps 
        [InlineData(0.01d)]
        [InlineData(0.02d)]
        [InlineData(0.03d)]
        [InlineData(0.04d)]
        [InlineData(0.05d)]
        [InlineData(0.06d)]
        [InlineData(0.07d)]
        [InlineData(0.08d)]
        [InlineData(0.09d)]
        [InlineData(0.10d)]
        [InlineData(0.11d)]
        [InlineData(0.12d)]
        [InlineData(0.13d)]
        [InlineData(0.14d)]
        [InlineData(0.15d)]
        [InlineData(0.16d)]
        [InlineData(0.17d)]
        [InlineData(0.18d)]
        [InlineData(0.19d)]
        [InlineData(0.20d)]
        // Check big angles
        [ClassData(typeof(DegreeDataGenerator))]
        public void ShouldConvertToAbsoluteValue(double degrees, double? precision = null)
        {
            var angle = (Math.PI / 2) * degrees / 180.0d;
            var fixedValue = (FixedPoint.Pi / FixedPoint.Two) * FixedPoint.From(degrees) / FixedPoint.From(180);
            
            _testOutputHelper.WriteLine($"Math:  {angle:0.00} - {Math.Sin(angle):0.0000}");
            _testOutputHelper.WriteLine($"Fixed: {FixedPoint.ConvertToFloat(fixedValue):0.00} - {FixedPoint.ConvertToFloat(MathFixedPoint.Sin(fixedValue)):0.0000}");

            FixedPoint.ConvertToDouble(MathFixedPoint.Sin(fixedValue)).Should().BeApproximately(Math.Sin(angle), precision ?? 0.05f);
        }
    }
}