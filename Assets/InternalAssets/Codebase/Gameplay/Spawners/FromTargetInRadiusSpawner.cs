using ACS.Core.ServicesContainer;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Parents;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Spawners
{
    public class FromTargetInRadiusSpawner : AbstractSpawner
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private float _spawningRadius;

        private Transform _playerTransform;
        private Entity _listeningEntity;
        private SceneAssetParentsContainer _sceneAssetParentsContainer;

        [Button]
        public override void Initialize(Entity listeningEntity)
        {
            _listeningEntity = listeningEntity;

            ServiceContainer.ForCurrentScene().Get(out _sceneAssetParentsContainer);
        }
        
        [Button]
        public override void Spawn()
        {
            if(_listeningEntity == null) return;
            
            Vector3 entityPosition = _listeningEntity.Transform.position;
            float randomAngle = Random.Range(0f, 360f);
            
            float x = entityPosition.x + _spawningRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float y = entityPosition.y + _spawningRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
            
            Vector3 spawnPosition = new Vector3(x, y, 0);
            
            Enemy enemy = LeanPool.Spawn(_enemyPrefab, spawnPosition, Quaternion.identity, _sceneAssetParentsContainer.EnemiesParent);

            enemy.Bootstrapp();
            enemy.Initialize(EnemyType.mini_cyclop);
        }
    }
}
