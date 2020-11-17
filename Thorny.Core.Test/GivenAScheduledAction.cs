using FluentAssertions;
using Xunit;

namespace Thorny.Core.Test
{
    public class GivenAScheduledAction
    {
        [Fact]
        public void WhenCreatedUsingEnforcedFrequency()
        {
            var lastActionedTick = 0UL;
            var actionedTicks = 0;
            var subject = ScheduledAction.From((tick) =>
            {
                lastActionedTick = tick;
                actionedTicks += 1;
            }, 100, true);
            
            subject.Tick(0);
            lastActionedTick.Should().Be(0L);
            actionedTicks.Should().Be(0);
            subject.RemainingDelta.Should().Be(0);
            
            // Preform an incomplete tick
            subject.Tick(99);
            lastActionedTick.Should().Be(0L);
            actionedTicks.Should().Be(0);
            subject.RemainingDelta.Should().Be(99);
            
            // Then complete it
            subject.Tick(100);
            lastActionedTick.Should().Be(1L);
            actionedTicks.Should().Be(1);
            subject.RemainingDelta.Should().Be(0);
            
            // Skip two ticks
            subject.Tick(300);
            lastActionedTick.Should().Be(3L);
            actionedTicks.Should().Be(3);
            subject.RemainingDelta.Should().Be(0);
        }
        
        [Fact]
        public void WhenCreatedUsingNonEnforcedFrequency()
        {
            var lastActionedTick = 0UL;
            var actionedTicks = 0;
            var subject = ScheduledAction.From((tick) =>
            {
                lastActionedTick = tick;
                actionedTicks += 1;
            }, 100, false);
            
            subject.Tick(0);
            lastActionedTick.Should().Be(0L);
            actionedTicks.Should().Be(0);
            subject.RemainingDelta.Should().Be(0);
            
            // Preform an incomplete tick
            subject.Tick(99);
            lastActionedTick.Should().Be(0L);
            actionedTicks.Should().Be(0);
            subject.RemainingDelta.Should().Be(99);
            
            // Then complete it
            subject.Tick(100);
            lastActionedTick.Should().Be(1L);
            actionedTicks.Should().Be(1);
            subject.RemainingDelta.Should().Be(0);
            
            // Skip two ticks
            subject.Tick(300);
            lastActionedTick.Should().Be(3L);
            actionedTicks.Should().Be(2);
            subject.RemainingDelta.Should().Be(0);
        }
    }
}