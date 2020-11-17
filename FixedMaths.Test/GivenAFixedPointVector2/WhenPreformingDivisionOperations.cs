using System.Numerics;
using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPointVector2
{
    public class WhenPreformingDivisionOperations
    {
        [Theory]
        [InlineData(1.0f, 1.0f, 1.0f, 1.0f)]
        [InlineData(2.0f, 2.0f, 2.0f, 2.0f)]
        [InlineData(0.5f, 0.5f, 0.707106769f, 0.707106769f)]
        public void ShouldPreformDivisionsResultingInSimilarResultsToCSharp(float aX, float aY, float bX, float bY)
        {
            var (actualX, actualY) = FixedPointVector2.From(FixedPoint.From(aX), FixedPoint.From(aY)) / FixedPointVector2.From(FixedPoint.From(bX), FixedPoint.From(bY));
            var expected = new Vector2(aX, aY) / new Vector2(bX, bY);

            actualX.Should().BeApproximately(expected.X, 0.01f);
            actualY.Should().BeApproximately(expected.Y, 0.01f);
        }
    }
}