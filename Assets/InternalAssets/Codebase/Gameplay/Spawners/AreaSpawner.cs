using InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Spawners
{
    public class AreaSpawner : MonoBehaviour, IEnemiesSpawner
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _spawnParent;
        [SerializeField] private SpawnAreaCombiner _spawnAreaCombiner;
        
        private Vector3 _size = Vector3.one;
        
        [Button]
        public void Spawn()
        {
            if (_spawnAreaCombiner.TryGetAvailablePoint(_size, false, out Vector3 position))
            {
                Enemy enemy = LeanPool.Spawn(_enemyPrefab, position, Quaternion.identity, _spawnParent);

                enemy.Bootstrapp();
                enemy.Initialize(EnemyType.mini_cyclop);
            }
        }
    }
}