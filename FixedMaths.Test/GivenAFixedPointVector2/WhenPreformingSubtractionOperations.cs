using System;
using FixedMaths.Core;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenAFixedPointVector2
{
    public class WhenPreformingSubtractionOperations
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenPreformingSubtractionOperations(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(0.0000, -5.0000, 0.0000, -4.7958)]
        [InlineData(0.0000, -4.9939, 0.0000, -4.9221)]
        [InlineData(0.0000, -5.0169, 0.0000, -5.0181)]
        public void ShouldCorrectlySubtractValues(float aX, float aY, float bX, float bY)
        {
            var (actualX, actualY) = FixedPointVector2.From(FixedPoint.From(aX), FixedPoint.From(aY)) - FixedPointVector2.From(FixedPoint.From(bX), FixedPoint.From(bY));

            _testOutputHelper.WriteLine($"({actualX}, {actualY})");
            
            actualX.Should().BeApproximately(aX - bX, 0.01f);
            actualY.Should().BeApproximately(aY - bY, 0.01f);
        }
    }
}