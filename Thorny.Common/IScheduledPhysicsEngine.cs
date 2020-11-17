namespace Thorny.Common
{
    public interface IScheduledPhysicsEngine
    {
        void Execute(ulong tick);
    }
}