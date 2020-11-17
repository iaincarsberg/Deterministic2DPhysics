namespace Thorny.Common
{
    public interface IScheduledPhysicsEngine
    {
        string Name { get; }
        void Execute(ulong tick);
    }
}