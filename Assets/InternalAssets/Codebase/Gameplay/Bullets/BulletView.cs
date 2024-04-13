using System;
using Codebase.Library.Extension.Rx;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using Lean.Pool;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Bullets
{
    public class BulletView : MonoBehaviour, IDisposable
    {
        [BoxGroup("Effects"), SerializeField] private SortableParticle _bulletInteractionEffect;
        
        private IDisposable _movementDisposable;
        private IDisposable _autoDespawnDisposable;
     
        private WeaponAmmoStats _ammoStat;
        private Transform _selfTransform;
        private IDamageReceiver _owner;
        
        private Vector3 _direction;
        private Vector3 _bulletSpeed;
        private DamageArgs _args;
        private bool _isInteracted = false;
        
        public void Bootstrapp(IDamageReceiver owner, WeaponAmmoStats ammoStat, Vector3 direction)
        {
            _autoDespawnDisposable?.Dispose();

            _selfTransform = transform;
            _owner = owner;
            _ammoStat = ammoStat;
            _direction = direction;
            _isInteracted = false;
            
            GenerateArgs();
            NormalizeDirection();
            PrepareMovement();

            if (ammoStat.DestructByLifeTime)
                _autoDespawnDisposable = RX.Delay(ammoStat.LifeTime, Despawn);
        }
        
        public void Dispose()
        {
            _autoDespawnDisposable?.Dispose(); 
            
            StopMovement();
        }

        private void GenerateArgs()
        {
            _args = new DamageArgs()
            {
                Damage = _ammoStat.Damage,
                IsCritical = false
            };
        }

        private void NormalizeDirection() =>
            _selfTransform.rotation = Quaternion.Euler(
                180, 
                180, 
                Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg
            );

        public void PrepareMovement()
        {
            _bulletSpeed = _direction.normalized * (Time.fixedDeltaTime * _ammoStat.BulletMovementSpeed);
            
            _movementDisposable = Observable
                .EveryFixedUpdate()
                .Subscribe(_ => ApplyTranslation());
        }

        private void ApplyTranslation()
        {
            if (_selfTransform != null)
                _selfTransform.position += _bulletSpeed * Time.timeScale;
        }
        
        private void StopMovement() => _movementDisposable?.Dispose();

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (_isInteracted) return;

            if (other.TryGetComponent(out IDamageReceiver receiver))
            {
                if (_owner == receiver) return;
                
                _isInteracted = true;

                receiver.ReceiveDamage(_args);

                CreateSplittingEffect();
                Despawn();
            }
        }

        protected void CreateSplittingEffect(Action onComplete = null)
        {
            SortableParticle sortableParticle = LeanPool.Spawn(_bulletInteractionEffect);
            Transform effectTransform = sortableParticle.transform;
            
            effectTransform.position = _selfTransform.position;
            sortableParticle.Play();
            
            onComplete?.Invoke();
        }

        private void Despawn()
        {
            Dispose();
            
            LeanPool.Despawn(gameObject);
        }
    }
}