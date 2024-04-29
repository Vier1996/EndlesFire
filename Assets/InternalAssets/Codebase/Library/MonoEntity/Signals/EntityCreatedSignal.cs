using ACS.SignalBus.SignalBus.Parent;
using InternalAssets.Codebase.Library.MonoEntity.Entities;

namespace InternalAssets.Codebase.Library.MonoEntity.Signals
{
    [Signal]
    public class EntityCreatedSignal
    {
        public Entity Entity { get; private set; }

        public EntityCreatedSignal(Entity entity) => Entity = entity;
    }
}