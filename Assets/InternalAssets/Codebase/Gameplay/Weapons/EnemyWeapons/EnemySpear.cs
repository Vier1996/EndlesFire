using System;
using System.Collections.Generic;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.SAD;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Triggers;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.EnemyWeapons
{
    public class EnemySpear : WeaponView
    {
        [BoxGroup("Trigger"), SerializeField] private DamagableTrigger _damagableTrigger;
        
        [BoxGroup("Animation Params"), SerializeField] private float _chargeOffset = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _chargeTime = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _strikeDelay = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _strikeOffset = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _strikeDuration = 1f;
        [BoxGroup("Animation Params"), SerializeField] private float _normalizeDuration = 1f;
        
        [BoxGroup("Indicator"), SerializeField] private ParticleSystem _indicatorEffect;
        [BoxGroup("Indicator"), SerializeField] private ParticleSystem _strikeEffect;
        [BoxGroup("Indicator"), SerializeField] private ParticleSystem _strikeFinishedEffect;

        private IDetectionSystem _detectionSystem;
        private ITargetable _currentTarget;
        private IDisposable _fireDispatchDisposable;
        private DamageArgs _args;
        
        private float _currentRechargingTime;
        
        public override void Bootstrapp(WeaponConfig weaponConfig, Entity ownerEntity)
        {
            base.Bootstrapp(weaponConfig, ownerEntity);

            _detectionSystem = ownerEntity.GetAbstractComponent<IDetectionSystem>();
            
            InShootingStatus = false;
            InShootingProcess = false;
            InRechargingStatus = false;
            
            _currentRechargingTime = 0f;

            _damagableTrigger.SetListeningTypes(new List<Type>()
            {
                typeof(Player)
            });
            
            _fireDispatchDisposable = Observable.EveryUpdate().Subscribe(_ => DispatchCalculations());
           
            _detectionSystem.OnTargetDetected += StartFire;
            _damagableTrigger.ReceiverFound += OnReceiverFound;
        }

        public override void Dispose()
        {
            _fireDispatchDisposable?.Dispose();
            
            StopFire();
            
            _detectionSystem.OnTargetDetected -= StartFire;
            _damagableTrigger.ReceiverFound -= OnReceiverFound;

            Destroy(gameObject);
        }

        public override void StartFire(ITargetable target)
        {
            _currentTarget = target;

            if (_currentTarget == null)
            {
                StopFire();
                return;
            }
            
            InShootingProcess = true;
        }

        public override void StopFire()
        {
            InShootingProcess = false;
            InShootingStatus = false;
        }
        
        private void GenerateArgs()
        {
            _args = new DamageArgs()
            {
                Damage = WeaponConfig.WeaponAmmoStats.Damage,
                IsCritical = false,
                Type = DamageType.damage
            };
        }
        
        private void DispatchCalculations()
        {
            if (InShootingProcess == false || InShootingStatus) return;
            
            _currentRechargingTime += Time.deltaTime;

            if (_currentRechargingTime >= WeaponConfig.WeaponStats.BaseRecharging) 
                TryFire();
        }

        private bool TryFire()
        {
            if (_currentTarget == null || _currentTarget.GetTargetTransform() == null)
                return false;

            InShootingStatus = true;
            _currentRechargingTime = 0f;
            
            AnimateWeaponFire();
            
            return true;
        }

        private void ActivateDamageTrigger() => _damagableTrigger.gameObject.SetActive(true);
        private void DeactivateDamageTrigger() => _damagableTrigger.gameObject.SetActive(false);
        
        [Button]
        protected override void AnimateWeaponFire()
        {
            SelfTransform.KillTween();
            SelfTransform.localPosition = DefaultWeaponLocalPosition;
            
            ChargeSpear();
        }

        private void ChargeSpear()
        {
            _indicatorEffect.Play();
            
            SelfTransform
                .DOLocalMoveX(_chargeOffset, _chargeTime)
                .OnComplete(() =>
                {
                    ActivateDamageTrigger();
                    StrikeSpear();
                });
        }

        private void StrikeSpear()
        {
            _strikeEffect.Play();
            
            SelfTransform
                .DOLocalMoveX(_strikeOffset, _strikeDuration)
                .SetDelay(_strikeDelay)
                .OnComplete(() =>
                {
                    _strikeFinishedEffect.Play();
                    
                    DeactivateDamageTrigger();

                    SelfTransform
                        .DOLocalMoveX(0, _normalizeDuration)
                        .OnComplete(() =>
                        {
                            InShootingStatus = false;
                        });
                });
        }
        
        private void OnReceiverFound(DamageReceiveTrigger receiver)
        {
            DeactivateDamageTrigger();
            GenerateArgs();
            
            receiver.DamageReceiver.ReceiveDamage(_args);
        }
    }
}