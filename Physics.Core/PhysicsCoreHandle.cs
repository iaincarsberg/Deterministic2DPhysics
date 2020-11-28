using System;
using Svelto.ECS;

namespace Physics.Core
{
    public class PhysicsCoreHandle : IPhysicsCoreHandle
    {
        private readonly IEntityFactory _entityFactory;
        private Action<float> _setSimulationSpeed;

        public PhysicsCoreHandle(IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
        }

        public void RegisterSimulationSpeedControl(Action<float> action)
        {
            _setSimulationSpeed = action;
        }

        public void SetSimulationSpeed(float speed)
        {
            _setSimulationSpeed?.Invoke(speed);
        }
    }

    public interface IPhysicsCoreHandle
    {
        void SetSimulationSpeed(float f);
    }
}