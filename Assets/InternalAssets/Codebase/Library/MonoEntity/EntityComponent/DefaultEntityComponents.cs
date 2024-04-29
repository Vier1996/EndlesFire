using InternalAssets.Codebase.Library.MonoEntity.Entities;

namespace InternalAssets.Codebase.Library.MonoEntity.EntityComponent
{
    public class DefaultEntityComponents : EntityComponents
    {
        public override EntityComponents Declare(Entity abstractEntity) => this;
    }
}