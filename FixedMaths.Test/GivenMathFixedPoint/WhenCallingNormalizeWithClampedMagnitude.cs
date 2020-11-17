using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingNormalizeWithClampedMagnitude : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingNormalizeWithClampedMagnitude(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
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
            
            var (normalX, normalY) = value.NormalizeWithClampedMagnitude(FixedPoint.MaxValue);
            _testOutputHelper.WriteLine($"normal: ({normalX}, {normalY})");

            normalX.Should().BeApproximately(x, 0.01f);
            normalY.Should().BeApproximately(y, 0.01f);
        }
    }
}