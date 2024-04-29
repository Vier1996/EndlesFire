using System;
using System.Collections.Generic;
using ACS.Core.ServicesContainer;
using Codebase.Library.Extension.Rx;
using Codebase.Library.Random;
using InternalAssets.Codebase.Gameplay.Factory.itemViews;
using InternalAssets.Codebase.Gameplay.Items;
using InternalAssets.Codebase.Gameplay.Parents;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.Vectors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InternalAssets.Codebase.Gameplay.Directors
{
    public class GameExperienceDirector : MonoBehaviour, IGameplayDirector
    {
        private IDisposable _spawnedDisposable;
        private IDisposable _itemViewsGarbageCollectingDisposable;
        
        private Camera _camera;
        private Entity _listeningEntity;
        private ItemViewsFactoryProvider _factoryProvider;
        private GameExperienceDirectorConfiguration _configuration;
        private SceneAssetParentsContainer _sceneAssetParentsContainer;

        private List<ItemView> _spawnedItems = new();
        
        public async void Initialize(Entity listeningEntity)
        {
            _listeningEntity = listeningEntity;
            _camera = Camera.main;
            _configuration = (await GameExperienceDirectorConfiguration.GetInstanceAsync());
            
            ServiceContainer.Global.Get(out _factoryProvider);
            ServiceContainer.ForCurrentScene().Get(out _sceneAssetParentsContainer);

            InitialSpawn();
            
            _spawnedDisposable = RX.LoopedTimer(_configuration.SpawnDelay, _configuration.SpawnDelay, 
                () => SpawnExperience(_configuration.MinimalDistanceFromPlayer, _configuration.MaximalDistanceFromPlayer));
            
            _itemViewsGarbageCollectingDisposable = RX.LoopedTimer(1f, _configuration.RefreshTime, ClearDistantItems);
        }

        public void Dispose()
        {
            _spawnedDisposable?.Dispose();
            _itemViewsGarbageCollectingDisposable?.Dispose();
            
            _configuration.Release();
        }

        private void InitialSpawn()
        {
            int count = Random.Range(_configuration.InitialMinItemsCount, _configuration.InitialMaxItemsCount);

            for (int i = 0; i < count; i++) 
                SpawnExperience(3f, _configuration.MinimalDistanceFromPlayer);
        }
        
        private void ClearDistantItems()
        {
            for (int i = 0; i < _spawnedItems.Count; i++)
            {
                if (_spawnedItems[i].SelfTransform.position.DistanceXY(_listeningEntity.Transform.position) >= _configuration.ClearDistance)
                {
                    _spawnedItems[i].Despawn();
                    i--;
                }
            }
        }

        private async void SpawnExperience(float minDistance, float maxDistance)
        {
            if(_listeningEntity == null || _spawnedItems.Count >= _configuration.MaxItemsCount) return;

            Vector3 spawnPosition = _listeningEntity.Transform.position.GetPointInRadius(minDistance, maxDistance);
            
            GameExperienceDirectorConfiguration.ChancedGameExperience experienceValue = _configuration.GameExperienceChanced.Random();
            
            ItemView itemView = await _factoryProvider.SpawnFactoryItemAsync(experienceValue.ItemData.ItemType, spawnPosition, Quaternion.identity, _sceneAssetParentsContainer.ItemsParent);
                
            itemView
                .Setup(experienceValue.ItemData, spawnPosition)
                .Enable();
            
            itemView.Despawned += OnItemDespawn;
            
            _spawnedItems.Add(itemView);
        }

        private void OnItemDespawn(ItemView item)
        {
            item.Despawned -= OnItemDespawn;

            _spawnedItems.Remove(item);
        }
    }
}
