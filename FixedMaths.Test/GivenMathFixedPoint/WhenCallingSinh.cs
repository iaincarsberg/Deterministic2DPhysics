using System;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FixedMaths.Test.TestGenerator;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingSinh : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingSinh(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [ClassData(typeof(DegreeDataGenerator))]
        public void ShouldConvertToAbsoluteValue(double degrees, double precision)
        {
            var angle = (Math.PI / 2) * degrees / 180.0d;
            var fixedValue = (FixedPoint.Pi / FixedPoint.Two) * FixedPoint.From(degrees) / FixedPoint.From(180);

            _testOutputHelper.WriteLine($"Math:  {angle:0.00} - {Math.Sinh(angle):0.0000}");
            _testOutputHelper.WriteLine($"Fixed: {FixedPoint.ConvertToFloat(fixedValue):0.00} - {FixedPoint.ConvertToFloat(MathFixedPoint.Sinh(fixedValue)):0.0000}");

            FixedPoint.ConvertToDouble(MathFixedPoint.Sinh(fixedValue)).Should().BeApproximately(Math.Sinh(angle), precision);
        }
    }
}