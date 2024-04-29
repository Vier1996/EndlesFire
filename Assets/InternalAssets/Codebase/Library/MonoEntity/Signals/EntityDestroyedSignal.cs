using ACS.SignalBus.SignalBus.Parent;
using InternalAssets.Codebase.Library.MonoEntity.Entities;

namespace InternalAssets.Codebase.Library.MonoEntity.Signals
{
    [Signal]
    public class EntityDestroyedSignal
    {
        public Entity Entity { get; private set; }

        public EntityDestroyedSignal(Entity entity) => Entity = entity;
    }
}