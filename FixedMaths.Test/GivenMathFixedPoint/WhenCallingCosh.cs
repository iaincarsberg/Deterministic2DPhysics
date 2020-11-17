using System;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FixedMaths.Test.TestGenerator;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingCosh : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingCosh(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [ClassData(typeof(DegreeDataGenerator))]
        public void ShouldConvertToAbsoluteValue(double degrees, double precision)
        {
            var angle = (Math.PI / 2) * degrees / 180.0d;
            var fixedValue = (FixedPoint.Pi / FixedPoint.Two) * FixedPoint.From(degrees) / FixedPoint.From(180);

            _testOutputHelper.WriteLine($"Math:  {angle:0.00} - {Math.Cosh(angle):0.0000}");
            _testOutputHelper.WriteLine($"Fixed: {FixedPoint.ConvertToFloat(fixedValue):0.00} - {FixedPoint.ConvertToFloat(MathFixedPoint.Cosh(fixedValue)):0.0000}");

            FixedPoint.ConvertToDouble(MathFixedPoint.Cosh(fixedValue)).Should().BeApproximately(Math.Cosh(angle), precision);
        }
    }
}