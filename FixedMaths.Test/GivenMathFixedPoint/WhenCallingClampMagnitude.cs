using System;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingClampMagnitude : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingClampMagnitude(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(0.0f, 0.0f, 10.0f)]
        [InlineData(0.5f, 0.5f, 10.0f)]
        [InlineData(1.0f, 1.0f, 10.0f)]
        [InlineData(1.5f, 1.5f, 10.0f)]
        [InlineData(2.0f, 2.0f, 10.0f)]
        [InlineData(2.5f, 2.5f, 10.0f)]
        [InlineData(3.0f, 3.0f, 10.0f)]
        [InlineData(10.0f, 10.0f, 10.0f)]
        public void ShouldClampTheMagnitude(float x, float y, float clamp)
        {
            var fp = FixedPointVector2.From(FixedPoint.From(x), FixedPoint.From(y));
            var result = FixedPoint.ConvertToFloat(MathFixedPoint.ClampMagnitude(fp, FixedPoint.From(clamp)));

            var expected = (float)Math.Sqrt(Math.Max(Math.Min(x * x + y * y, clamp), -clamp));
            
            result.Should().BeApproximately(expected, 0.3f);
        }
    }
}