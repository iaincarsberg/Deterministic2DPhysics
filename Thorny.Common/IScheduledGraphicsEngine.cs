using FixedMaths.Core;

namespace Thorny.Common
{
    public interface IScheduledGraphicsEngine
    {
        string Name { get; }
        void Draw(FixedPoint delta, ulong physicsTick);
    }
}