using System.Collections.Generic;
using System.Linq;
using FixedMaths.Core;
using FluentAssertions;
using Physics.Core.Loggers;
using Physics.Core.Loggers.Data;
using Xunit;

namespace Physics.Core.Test.Loggers
{
    public class GivenAFixedPointVector2Logger
    {
        private readonly List<FixedPointVector2> _resultAt0;
        private readonly List<FixedPointVector2> _resultAt1;
        private readonly List<FixedPointVector2> _resultAt2;
        private readonly List<FixedPointVector2> _resultAt3;
        private readonly List<FixedPointVector2> _resultAt4;
        private readonly List<FixedPointVector2> _resultAt5;
        private readonly List<FixedPointVector2> _resultAt6;
        private readonly List<FixedPointVector2> _resultAt7;

        public GivenAFixedPointVector2Logger()
        {
            var logger = new FixedPointVector2Logger();
            
            logger.DrawCross(FixedPointVector2.NegativeOne, 0, Colour.Aqua, 5);
            logger.DrawCross(FixedPointVector2.Zero, 0, Colour.Aqua, 3);
            logger.DrawCross(FixedPointVector2.One, 0, Colour.Aqua, 6);

            _resultAt0 = logger.GetPoints(0).Select(x => x.Point).ToList();
            _resultAt1 = logger.GetPoints(1).Select(x => x.Point).ToList();
            _resultAt2 = logger.GetPoints(2).Select(x => x.Point).ToList();
            _resultAt3 = logger.GetPoints(3).Select(x => x.Point).ToList();
            _resultAt4 = logger.GetPoints(4).Select(x => x.Point).ToList();
            _resultAt5 = logger.GetPoints(5).Select(x => x.Point).ToList();
            _resultAt6 = logger.GetPoints(6).Select(x => x.Point).ToList();
            _resultAt7 = logger.GetPoints(7).Select(x => x.Point).ToList();
        }

        [Fact]
        public void ShouldHaveTwoValuesAtTick0()
        {
            _resultAt0.Count.Should().Be(3);
            _resultAt0.Should().Contain(FixedPointVector2.NegativeOne);
            _resultAt0.Should().Contain(FixedPointVector2.Zero);
            _resultAt0.Should().Contain(FixedPointVector2.One);
        }

        [Fact]
        public void ShouldHaveTwoValuesAtTick1()
        {
            _resultAt1.Count.Should().Be(3);
            _resultAt1.Should().Contain(FixedPointVector2.NegativeOne);
            _resultAt1.Should().Contain(FixedPointVector2.Zero);
            _resultAt1.Should().Contain(FixedPointVector2.One);
        }
        
        [Fact]
        public void ShouldHaveTwoValuesAtTick2()
        {
            _resultAt2.Count.Should().Be(3);
            _resultAt2.Should().Contain(FixedPointVector2.NegativeOne);
            _resultAt2.Should().Contain(FixedPointVector2.Zero);
            _resultAt2.Should().Contain(FixedPointVector2.One);
        }
        
        [Fact]
        public void ShouldHaveTwoValuesAtTick3()
        {
            _resultAt3.Count.Should().Be(3);
            _resultAt3.Should().Contain(FixedPointVector2.NegativeOne);
            _resultAt3.Should().Contain(FixedPointVector2.Zero);
            _resultAt3.Should().Contain(FixedPointVector2.One);
        }
        
        [Fact]
        public void ShouldHaveTwoValuesAtTick4()
        {
            _resultAt4.Count.Should().Be(2);
            _resultAt4.Should().Contain(FixedPointVector2.NegativeOne);
            _resultAt4.Should().Contain(FixedPointVector2.One);
        }
        
        [Fact]
        public void ShouldHaveTwoValuesAtTick5()
        {
            _resultAt5.Count.Should().Be(2);
            _resultAt5.Should().Contain(FixedPointVector2.NegativeOne);
            _resultAt5.Should().Contain(FixedPointVector2.One);
        }
        
        [Fact]
        public void ShouldHaveTwoValuesAtTick6()
        {
            _resultAt6.Count.Should().Be(1);
            _resultAt5.Should().Contain(FixedPointVector2.One);
        }
        
        [Fact]
        public void ShouldHaveTwoValuesAtTick7()
        {
            _resultAt7.Count.Should().Be(0);
        }
    }
}