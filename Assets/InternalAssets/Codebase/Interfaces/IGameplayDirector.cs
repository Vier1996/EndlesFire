using Codebase.Library.SAD;

namespace InternalAssets.Codebase.Interfaces
{
    public interface IGameplayDirector
    {
        public void Initialize(Entity listeningEntity);
        public void Dispose();
    }
}