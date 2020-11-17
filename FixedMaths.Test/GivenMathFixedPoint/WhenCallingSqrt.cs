using System;
using System.Diagnostics;
using FixedMaths.Core;
using FixedMaths.Test.ClassFixtures;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenMathFixedPoint
{
    public class WhenCallingSqrt : IClassFixture<ProcessedTableRepositoryFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenCallingSqrt(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(0.0f, 0.5f)]
        [InlineData(0.1f, 0.5f)]
        [InlineData(0.2f, 0.5f)]
        [InlineData(0.3f, 0.9f)]
        [InlineData(0.4f, 0.9f)]
        [InlineData(0.5f, 0.9f)]
        [InlineData(0.6f, 0.9f)]
        [InlineData(0.7f, 0.9f)]
        [InlineData(0.8f, 0.9f)]
        [InlineData(0.9f, 1.0f)]
        [InlineData(1.0f, 0.5f)]
        [InlineData(1.1f, 0.5f)]
        [InlineData(1.2f, 0.5f)]
        [InlineData(1.3f, 0.5f)]
        [InlineData(1.4f, 0.5f)]
        [InlineData(1.5f, 0.5f)]
        [InlineData(1.6f, 0.5f)]
        [InlineData(1.7f, 0.5f)]
        [InlineData(1.8f, 0.5f)]
        [InlineData(1.9f, 0.5f)]
        [InlineData(2.0f, 0.5f)]
        [InlineData(2.1f, 0.5f)]
        [InlineData(2.2f, 0.5f)]
        [InlineData(2.3f, 0.5f)]
        [InlineData(2.4f, 0.5f)]
        [InlineData(2.5f, 0.5f)]
        [InlineData(1000.0f, 0.01f)]
        [InlineData(524287.0f, 0.01f)]
        [InlineData(-1.0f, 0.5f)]
        public void ShouldConvertToAbsoluteValue(float value, float precision)
        {
            var fixedPoint = FixedPoint.From(value);
            
            var stopwatch = Stopwatch.StartNew();

            var lookupResult = MathFixedPoint.Sqrt(fixedPoint);
            var lookupTableDelta = stopwatch.ElapsedMilliseconds;
            
            stopwatch.Restart();
            
            var loopResult = MathFixedPoint.Sqrt2(fixedPoint);
            var loopDelta = stopwatch.ElapsedMilliseconds;
            
            stopwatch.Restart();

            var expected = (float) Math.Sqrt(value);
            var systemDelta = stopwatch.ElapsedMilliseconds;

            _testOutputHelper.WriteLine($"lookupTableDelta: {lookupTableDelta}, loopDelta: {loopDelta}, systemDelta: {systemDelta}");
            
            
            if (float.IsNaN(expected))
            {
                FixedPoint.ConvertToFloat(lookupResult).Should().Be(float.NaN);
                //FixedPoint.ConvertToFloat(loopResult).Should().Be(float.NaN);
            }
            else
            {
                FixedPoint.ConvertToFloat(lookupResult).Should().BeApproximately(expected, precision);
                FixedPoint.ConvertToFloat(loopResult).Should().BeApproximately(expected, precision);
            }
        }

        [Theory]
        [InlineData(1.0f)]
        [InlineData(100.0f)]
        [InlineData(100.5f)]
        [InlineData(524287.0f)]
        public void ShouldBenchmarkVariousMethods(float value)
        {
            
            const int loops = 1000000;
            var fixedPoint = FixedPoint.From(value);
            
            var stopwatch = Stopwatch.StartNew();

            
            for (var i = 0; i < loops; i++)
            {
                MathFixedPoint.Sqrt(fixedPoint);
            }
            var lookupTableDelta = stopwatch.ElapsedMilliseconds;
            
            stopwatch.Restart();
            
            for (var i = 0; i < loops; i++)
            {
                MathFixedPoint.Sqrt2(fixedPoint);
            }
            var loopDelta = stopwatch.ElapsedMilliseconds;
            
            stopwatch.Restart();

            for (var i = 0; i < loops; i++)
            {
                Math.Sqrt(value);
            }
            var systemDelta = stopwatch.ElapsedMilliseconds;

            _testOutputHelper.WriteLine($"{loops} iterations, lookup-table: {lookupTableDelta}ms, calculate: {loopDelta}ms, Math.Sqrt: {systemDelta}ms");
        }
    }
}