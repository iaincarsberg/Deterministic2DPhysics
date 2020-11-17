using System;
using System.Numerics;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingNormalize : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingNormalize(ITestOutputHelper testOutputHelper)
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
        public void ShouldNormalizeVector(float x, float y)
        {
            var vector = FixedPointVector2.From(FixedPoint.From(x), FixedPoint.From(y));
            var (actualX, actualY) = vector.Normalize();
            var expected = Vector2.Normalize(new Vector2(x, y));

            _testOutputHelper.WriteLine($"Fixed.Dot       {FixedPointVector2.Dot(vector, vector)}");
            _testOutputHelper.WriteLine($"Fixed.Round     {MathFixedPoint.Round(FixedPoint.From(x))}, {MathFixedPoint.Round(FixedPoint.From(y))}");
            _testOutputHelper.WriteLine($"Fixed.Magnitude {MathFixedPoint.Magnitude(vector)}");
            _testOutputHelper.WriteLine($"Vector2.Dot     {Vector2.Dot(new Vector2(x, y), new Vector2(x, y))}");
            _testOutputHelper.WriteLine($"Vector2.Length  {new Vector2(x, y).Length()}");

            _testOutputHelper.WriteLine($"actual   ({actualX:0.00000}, {actualY:0.00000})");
            _testOutputHelper.WriteLine($"expected ({expected.X:0.00000}, {expected.Y:0.00000})");

            if (float.IsNaN(expected.X) || float.IsNaN(expected.Y))
            {
                actualX.Should().BeApproximately(0.0f, 0.01f);
                actualY.Should().BeApproximately(0.0f, 0.01f);
            }
            else
            {
                actualX.Should().BeApproximately(expected.X, 0.01f);
                actualY.Should().BeApproximately(expected.Y, 0.01f);
            }
        }
        
        [Theory]
        [InlineData(1.0f, 0.0f, 1)]
        [InlineData(1.0f, 0.0f, 100)]
        [InlineData(1.0f, 0.0f, 200)]
        [InlineData(1.0f, 0.0f, 300)]
        [InlineData(1.0f, 0.0f, 400)]
        [InlineData(1.0f, 0.0f, 500)]
        [InlineData(1.0f, 0.0f, 600)]
        [InlineData(1.0f, 0.0f, 700)]
        [InlineData(1.0f, 0.0f, 800)]
        [InlineData(1.0f, 0.0f, 900)]
        [InlineData(1.0f, 0.0f, 1000)]
        [InlineData(-1.0f, 0.0f, 1)]
        [InlineData(-1.0f, 0.0f, 100)]
        [InlineData(-1.0f, 0.0f, 200)]
        [InlineData(-1.0f, 0.0f, 300)]
        [InlineData(-1.0f, 0.0f, 400)]
        [InlineData(-1.0f, 0.0f, 500)]
        [InlineData(-1.0f, 0.0f, 600)]
        [InlineData(-1.0f, 0.0f, 700)]
        [InlineData(-1.0f, 0.0f, 800)]
        [InlineData(-1.0f, 0.0f, 900)]
        [InlineData(-1.0f, 0.0f, 1000)]
        [InlineData(0.0f, 1.0f, 1)]
        [InlineData(0.0f, 1.0f, 100)]
        [InlineData(0.0f, 1.0f, 200)]
        [InlineData(0.0f, 1.0f, 300)]
        [InlineData(0.0f, 1.0f, 400)]
        [InlineData(0.0f, 1.0f, 500)]
        [InlineData(0.0f, 1.0f, 600)]
        [InlineData(0.0f, 1.0f, 700)]
        [InlineData(0.0f, 1.0f, 800)]
        [InlineData(0.0f, 1.0f, 900)]
        [InlineData(0.0f, 1.0f, 1000)]
        [InlineData(0.0f, -1.0f, 1)]
        [InlineData(0.0f, -1.0f, 100)]
        [InlineData(0.0f, -1.0f, 200)]
        [InlineData(0.0f, -1.0f, 300)]
        [InlineData(0.0f, -1.0f, 400)]
        [InlineData(0.0f, -1.0f, 500)]
        [InlineData(0.0f, -1.0f, 600)]
        [InlineData(0.0f, -1.0f, 700)]
        [InlineData(0.0f, -1.0f, 800)]
        [InlineData(0.0f, -1.0f, 900)]
        [InlineData(0.0f, -1.0f, 1000)]
        [InlineData(-1.0f, 0.0f, 4096)]
        [InlineData(0.0f, 1.0f, 4096)]
        [InlineData(0.0f, -1.0f, 4096)]
        [InlineData(-0.5775f, 0.0f, 4096)]
        public void ShouldScaleVectorThenNormalizeItBackIntoTheOriginalVector(float x, float y, int scale)
        {
            var value = FixedPointVector2.From(FixedPoint.From(x), FixedPoint.From(y)) * FixedPoint.From(scale);
            
            var (normalX, normalY) = value.Normalize();
            _testOutputHelper.WriteLine($"normal: ({normalX}, {normalY})");

            normalX.Should().BeApproximately(x, 0.01f);
            normalY.Should().BeApproximately(y, 0.01f);
        }
    }
}