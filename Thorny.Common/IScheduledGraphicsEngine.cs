using FixedMaths.Core;

namespace Thorny.Common
{
    public interface IScheduledGraphicsEngine
    {
        void Draw(FixedPoint delta, ulong physicsTick);
    }
}