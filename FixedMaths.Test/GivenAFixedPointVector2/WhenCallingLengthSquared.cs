using System;
using System.Numerics;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenAFixedPointVector2
{
    public class WhenCallingLengthSquared : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingLengthSquared(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, 1)]
        [InlineData(0, -1)]
        [InlineData(1, 1)]
        [InlineData(1, -1)]
        [InlineData(-1, 1)]
        [InlineData(-1, -1)]
        [InlineData(100, 2)]
        [InlineData(100, 10)]
        [InlineData(100, 20)]
        [InlineData(100, -2)]
        [InlineData(100, -10)]
        [InlineData(100, -20)]
        [InlineData(2, 100)]
        [InlineData(10, 100)]
        [InlineData(20, 100)]
        [InlineData(-2, 100)]
        [InlineData(-10, 100)]
        [InlineData(-20, 100)]
        [InlineData(0.7074481f, 0.7074481f)]
        [InlineData(0.7074481f, 0.1f)]
        [InlineData(0.0f, 0.5f)]
        [InlineData(0.0f, -0.5f)]
        [InlineData(0.5f, 0.0f)]
        [InlineData(-0.5f, 0.0f)]
        [InlineData(0.5f, -0.5f)]
        [InlineData(0.5f, 0.5f)]
        [InlineData(-0.5f, 0.5f)]
        [InlineData(-0.5f, -0.5f)]
        [InlineData(0, -0.07155067)]
        [InlineData(0, -0.20415139)]
        [InlineData(0, 0.0012210013)]
        public void ShouldFindLengthSquaredOfVector(float x, float y)
        {
            var vector = FixedPointVector2.From(FixedPoint.From(x), FixedPoint.From(y));
            var actual = FixedPoint.ConvertToFloat(vector.LengthSquared());
            var expected = new Vector2(x, y).LengthSquared();

            _testOutputHelper.WriteLine($"actual   ({actual:0.00000})");
            _testOutputHelper.WriteLine($"expected ({expected:0.00000})");

            if (float.IsNaN(expected))
            {
                actual.Should().BeApproximately(0.0f, 0.01f);
            }
            else
            {
                actual.Should().BeApproximately(expected, 0.05f);
            }
        }
    }
}