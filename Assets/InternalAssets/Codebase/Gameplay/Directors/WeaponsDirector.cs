using System;
using ACS.Core.ServicesContainer;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Items;
using InternalAssets.Codebase.Gameplay.Parents;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.Vectors;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Directors
{
    public class WeaponsDirector : MonoBehaviour, IGameplayDirector
    {
        [SerializeField] private WeaponItemView _weaponItemViewPrefav;
        
        private Entity _listeningEntity;
        private SceneAssetParentsContainer _sceneAssetParentsContainer;
        
        public void Initialize(Entity listeningEntity)
        {
            _listeningEntity = listeningEntity;
            
            ServiceContainer.ForCurrentScene().Get(out _sceneAssetParentsContainer);
        }

        public void Dispose()
        {
        }

        [Button]
        private void SpawnWeapon(float minDistance, float maxDistance, IItemData itemData)
        {
            Vector3 spawnPosition = _listeningEntity.Transform.position.GetPointInRadius(minDistance, maxDistance);

            ItemView itemView = LeanPool.Spawn(_weaponItemViewPrefav, spawnPosition, Quaternion.identity, _sceneAssetParentsContainer.ItemsParent);
                
            itemView
                .Setup(itemData, spawnPosition)
                .Enable();
        }
    }
}