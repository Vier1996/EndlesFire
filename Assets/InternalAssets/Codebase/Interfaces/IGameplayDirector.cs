using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;

namespace InternalAssets.Codebase.Interfaces
{
    public interface IGameplayDirector
    {
        public void Initialize(Entity listeningEntity);
        public void Dispose();
    }
}