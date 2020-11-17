using System;
using FixedMaths.Core;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WhenPreformingAdditionOperations
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WhenPreformingAdditionOperations(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(2, 8, 10)]
        [InlineData(3, 7, 10)]
        [InlineData(-3, -7, -10)]
        [InlineData(-3, 7, 4)]
        [InlineData(524287, 10, 524287)]
        [InlineData(-524288, -10, -524288)]
        [InlineData(524287, 524287, 524287)]
        [InlineData(524287, -524288, -1)]
        [InlineData(-524288, 524287, -1)]
        [InlineData(-524288, -524288, -524288)]
        public void WholeNumberAdditionFromIntegers(int a, int b, int expected)
        {
            var result = FixedPoint.From(a) + FixedPoint.From(b);

            _testOutputHelper.WriteLine($"result: {result}");
            
            FixedPoint.ConvertToInteger(result).Should().Be(expected);
        }

        [Theory]
        [InlineData(2.5f, 7.5f, 10.0f)]
        [InlineData(3.5f, 6.5f, 10.0f)]
        [InlineData(-3.5f, -6.5f, -10.0f)]
        [InlineData(-3.5f, 7.5f, 4.0f)]
        [InlineData(524287.0f, 10.0f, 524287.0f)]
        [InlineData(-524288.0f, -10.0f, -524288.0f)]
        [InlineData(5.0f, float.NaN, float.NaN)]
        [InlineData(0.71351f, 524287.0f, 524287.0f)]
        public void WholeNumberAdditionFromFloats(float a, float b, float expected)
        {
            var result = FixedPoint.From(a) + FixedPoint.From(b);

            _testOutputHelper.WriteLine($"result: {result}");

            if (float.IsNaN(expected))
            {
                FixedPoint.ConvertToFloat(result).Should().Be(float.NaN);
            }
            else
            {
                FixedPoint.ConvertToFloat(result).Should().BeApproximately(expected, 0.05f);
            }
        }
    }
}