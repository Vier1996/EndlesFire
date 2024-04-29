using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;

namespace InternalAssets.Codebase.Gameplay.Spawners
{
    public interface IEnemiesSpawner
    { 
        public void Initialize(Entity listeningEntity);
        public void Spawn();
    }
}
