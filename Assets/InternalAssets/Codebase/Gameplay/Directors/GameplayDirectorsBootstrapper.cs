using System.Collections.Generic;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace InternalAssets.Codebase.Gameplay.Directors
{
    public class GameplayDirectorsBootstrapper : SerializedMonoBehaviour
    {
        [OdinSerialize] private List<IGameplayDirector> _directors = new();
        public void Initialize(Entity listeningEntity) => 
            _directors.ForEach(dir => dir.Initialize(listeningEntity));
    }
}