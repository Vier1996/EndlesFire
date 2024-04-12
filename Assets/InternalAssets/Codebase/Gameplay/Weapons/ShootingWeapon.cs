using System;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.Extension.Rx;
using Codebase.Library.SAD;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Bullets;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using Lean.Pool;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons
{
    public class ShootingWeapon : WeaponView
    {
        [SerializeField] private Transform _bulletSpawnTransform;
        [SerializeField] private BulletView _bulletPrefab;

        private IDisposable _shootingQueueDisposable;
        private IDisposable _fireDispatchDisposable;
        private ITargetable _currentTarget;
        private IDetectionSystem _detectionSystem;
        
        private bool _shootingInProcess;
        private bool _busyByShooting;
        private float _currentAimingTime;
        private float _currentRechargingTime;

        public override void Bootstrapp(WeaponConfig weaponConfig, Entity ownerEntity)
        {
            base.Bootstrapp(weaponConfig, ownerEntity);

            _detectionSystem = ownerEntity.GetAbstractComponent<IDetectionSystem>();
            
            _shootingInProcess = false;
            _busyByShooting = false;
            _currentAimingTime = weaponConfig.WeaponStats.AimingTime;
            _currentRechargingTime = 0f;

            _fireDispatchDisposable = Observable.EveryUpdate().Subscribe(_ => DispatchCalculations());
           
            _detectionSystem.OnTargetDetected += StartFire;
        }
        
        public override void Dispose()
        {
            _fireDispatchDisposable?.Dispose();
            _shootingQueueDisposable?.Dispose();
            
            WeaponSpark.Dispose();
            
            StopFire();
            
            _detectionSystem.OnTargetDetected -= StartFire;
            
            Destroy(gameObject);
        }

        public override void StartFire(ITargetable target)
        {
            _currentTarget = target;
            _shootingInProcess = true;
        }

        public override void StopFire()
        {
            _shootingInProcess = false;
            _busyByShooting = false;
            _currentAimingTime = 0f;
        }

        private bool TryFire()
        {
            if (_currentTarget == null || _currentTarget.GetTargetTransform() == null)
                return false;

            _busyByShooting = true;
            _currentRechargingTime = 0f;

            Vector3 direction = _currentTarget.GetTargetTransform().position - _bulletSpawnTransform.position;
            
            _shootingQueueDisposable?.Dispose();
            _shootingQueueDisposable = RX.CountedTimer(
                0f,
                WeaponConfig.WeaponStoreStats.DelayBetweenBullets,
                WeaponConfig.WeaponStoreStats.BulletsByQueue,
                bulletIndex =>
                {
                    Vector3 dispersedDirection = WeaponDispersion.Disperse(direction, WeaponConfig.WeaponStats.FireDisperse);

                    if (WeaponConfig.WeaponStoreStats.AnimateEveryShoot)
                        AnimateWeaponFire();
                    
                    SpawnBullet(dispersedDirection);
                }, () =>
                {
                    _busyByShooting = false;
                });
            
            AnimateWeaponFire();
            
            return true;
        }

        private void DispatchCalculations()
        {
            if(_busyByShooting == false) _currentRechargingTime += Time.deltaTime;
            if (_shootingInProcess == false) return;
            
            _currentAimingTime += Time.deltaTime;

            if (
                _currentAimingTime >= WeaponConfig.WeaponStats.AimingTime &&
                _currentRechargingTime >= WeaponConfig.WeaponStats.BaseRecharging) 
                TryFire();
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
        
        [Button]
        protected override void AnimateWeaponFire()
        {
            float backTime = WeaponConfig.WeaponAnimationStats.AnimationDuration * WeaponConfig.WeaponAnimationStats.AnimationBalance;
            float forwardTime = WeaponConfig.WeaponAnimationStats.AnimationDuration - backTime;

            SelfTransform.KillTween();
            SelfTransform.localPosition = DefaultWeaponLocalPosition;
            
            SelfTransform
                .DOLocalMoveX(WeaponConfig.WeaponAnimationStats.AnimationOffset, backTime)
                .OnComplete(() =>
                {
                    SelfTransform.DOLocalMove(DefaultWeaponLocalPosition, forwardTime);
                });
        }

#if UNITY_EDITOR
        [Button]
        private void DebugFire(ITargetable target) => StartFire(target);
        
        [Button]
        private void DebugCancelFire() => StopFire();
#endif
    }
}