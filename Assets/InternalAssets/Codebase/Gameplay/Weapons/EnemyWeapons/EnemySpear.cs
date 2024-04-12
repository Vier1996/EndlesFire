using System;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.SAD;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.EnemyWeapons
{
    public class EnemySpear : WeaponView
    {
        [BoxGroup("Animation Params"), SerializeField] private float _chargeOffset = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _chargeTime = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _strikeDelay = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _strikeOffset = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _strikeDuration = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _normalizeDuration = 1f;

        private IDetectionSystem _detectionSystem;
        private ITargetable _currentTarget;
        private IDisposable _fireDispatchDisposable;
        
        private float _currentRechargingTime;
        private bool _fireInProcess;
        private bool _busyByFire;
        
        public override void Bootstrapp(WeaponConfig weaponConfig, Entity ownerEntity)
        {
            base.Bootstrapp(weaponConfig, ownerEntity);

            _detectionSystem = ownerEntity.GetAbstractComponent<IDetectionSystem>();
            
            _fireInProcess = false;
            _busyByFire = false;
            _currentRechargingTime = 0f;

            _fireDispatchDisposable = Observable.EveryUpdate().Subscribe(_ => DispatchCalculations());
           
            _detectionSystem.OnTargetDetected += StartFire;
        }
        
        public override void StartFire(ITargetable target)
        {
            _currentTarget = target;
            _fireInProcess = true;
        }

        public override void StopFire()
        {
            _fireInProcess = false;
            _busyByFire = false;
        }
        
        private void DispatchCalculations()
        {
            if (_fireInProcess == false || _busyByFire) return;
            
            _currentRechargingTime += Time.deltaTime;

            if (_currentRechargingTime >= WeaponConfig.WeaponStats.BaseRecharging) 
                TryFire();
        }

        private bool TryFire()
        {
            if (_currentTarget == null || _currentTarget.GetTargetTransform() == null)
                return false;

            _busyByFire = true;
            _currentRechargingTime = 0f;
            
            AnimateWeaponFire();
            
            return true;
        }
        
        private void ActivateDamageTrigger() { }
        private void DeactivateDamageTrigger() { }
        
        [Button]
        protected override void AnimateWeaponFire()
        {
            SelfTransform.KillTween();
            SelfTransform.localPosition = DefaultWeaponLocalPosition;
            
            ChargeSpear();
        }

        private void ChargeSpear(Action onComplete = null)
        {
            SelfTransform
                .DOLocalMoveX(_chargeOffset, _chargeTime)
                .OnComplete(() =>
                {
                    ActivateDamageTrigger();
                    StrikeSpear(onComplete);
                });
        }

        private void StrikeSpear(Action onComplete = null) =>
            SelfTransform
                .DOLocalMoveX(_strikeOffset, _strikeDuration)
                .SetDelay(_strikeDelay)
                .OnComplete(() =>
                {
                    DeactivateDamageTrigger();

                    SelfTransform
                        .DOLocalMoveX(0, _normalizeDuration)
                        .OnComplete(() => onComplete?.Invoke());
                });
    }
}