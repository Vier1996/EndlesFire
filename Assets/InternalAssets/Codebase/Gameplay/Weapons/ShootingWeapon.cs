using InternalAssets.Codebase.Gameplay.Bullets;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons
{
    public class ShootingWeapon : WeaponView
    {
        [SerializeField] private Transform _bulletSpawnTransform;
        [SerializeField] private BulletView _bulletPrefab;

        public override void Dispose()
        {
            WeaponSpark.Dispose();
            
            Destroy(gameObject);
        }

        public override void Fire(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - _bulletSpawnTransform.position;
            direction = WeaponDispersion.Disperse(direction, WeaponConfig.WeaponStats.FireDisperse);
            
            SpawnBullet(direction);
        }
        
        private void SpawnBullet(Vector3 direction)
        {
            Vector3 spawnPosition = _bulletSpawnTransform.position;
            BulletView spawnedBullet = LeanPool.Spawn(_bulletPrefab);

            spawnedBullet.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
            
            spawnedBullet.Bootstrapp(WeaponConfig.WeaponAmmoStats, direction);

            if (WeaponSpark != null)
                WeaponSpark.ShowSpark();
        }

#if UNITY_EDITOR
        [Button]
        private void DebugFire(Transform targetTransform)
        {
            Fire(targetTransform.position);
        }
#endif
    }
}