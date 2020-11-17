using System;
using FixedMaths.Core;
using FluentAssertions;
using Xunit;

namespace FixedMaths.Test.GivenAFixedPoint
{
    public class WithStaticValues
    {
        [Fact]
        public void ShouldStoreValidOneValue()
        {
            FixedPoint.One.Should().Be(FixedPoint.From(1));
        }
        
        [Fact]
        public void ShouldStoreValidTwoValue()
        {
            FixedPoint.Two.Should().Be(FixedPoint.From(2));
        }
        
        [Fact]
        public void ShouldStoreValidNegativeOneValue()
        {
            FixedPoint.NegativeOne.Should().Be(FixedPoint.From(-1));
        }
        
        [Fact]
        public void ShouldStoreValidZeroValue()
        {
            FixedPoint.Zero.Should().Be(FixedPoint.From(0));
        }
        
        [Fact]
        public void ShouldStoreValidPiValue()
        {
            FixedPoint.Pi.Should().Be(FixedPoint.From(Math.PI));
        }

        [Fact]
        public void ShouldStoreMaxValue()
        {
            FixedPoint.MaxValue.Should().Be(FixedPoint.From(524287));
        }
        
        [Fact]
        public void ShouldStoreMinValue()
        {
            FixedPoint.MinValue.Should().Be(FixedPoint.From(-524288));
        }

        [Fact]
        public void ShouldStoreHalfValue()
        {
            FixedPoint.Half.Should().Be(FixedPoint.From(0.5f));
        }

        [Fact]
        public void ShouldStoreATinyKludgeValue()
        {
            FixedPoint.Kludge.ToString().Should().Be("0.0002");
        }
    }
}