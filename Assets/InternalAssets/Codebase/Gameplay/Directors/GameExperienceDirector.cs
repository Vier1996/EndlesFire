using ACS.Core.ServicesContainer;
using Codebase.Library.Random;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Factory.itemViews;
using InternalAssets.Codebase.Gameplay.Parents;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Directors
{
    public class GameExperienceDirector : MonoBehaviour, IGameplayDirector
    {
        private Entity _listeningEntity;
        private ItemViewsFactoryProvider _factoryProvider;
        private GameExperienceDirectorConfiguration _configuration;
        private SceneAssetParentsContainer _sceneAssetParentsContainer;
        
        public async void Initialize(Entity listeningEntity)
        {
            _listeningEntity = listeningEntity;
            _configuration = (await GameExperienceDirectorConfiguration.GetInstanceAsync());
            
            ServiceContainer.Global.Get(out _factoryProvider);
            ServiceContainer.ForCurrentScene().Get(out _sceneAssetParentsContainer);
        }

        public void Dispose()
        {
            _configuration.Release();
        }
        
        [Button]
        private async void SpawnExperience()
        {
            if(_listeningEntity == null) return;
            
            Vector3 entityPosition = _listeningEntity.Transform.position;
            float randomAngle = Random.Range(0f, 360f);
            
            float x = entityPosition.x + Random.Range(_configuration.MinimalDistanceFromPlayer, _configuration.MaximalDistanceFromPlayer) * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float y = entityPosition.y + Random.Range(_configuration.MinimalDistanceFromPlayer, _configuration.MaximalDistanceFromPlayer) * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
            Vector3 spawnPosition = new Vector3(x, y, 0);

            GameExperienceDirectorConfiguration.ChancedGameExperience experienceValue = _configuration.GameExperienceChanced.Random();
            
            (await _factoryProvider.SpawnFactoryItemAsync(experienceValue.ItemData.ItemType, spawnPosition, Quaternion.identity, _sceneAssetParentsContainer.ItemsParent))
                .Setup(experienceValue.ItemData, spawnPosition)
                .Enable();
        }
    }
}
