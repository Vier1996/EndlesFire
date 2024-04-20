using Codebase.Library.SAD;

namespace InternalAssets.Codebase.Gameplay.Spawners
{
    public interface IEnemiesSpawner
    { 
        public void Initialize(Entity listeningEntity);
        public void Spawn();
    }
}
