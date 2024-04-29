using System;
using InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder;
using InternalAssets.Codebase.Gameplay.Scanning;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    public class PlayerDetectionSystem : MonoBehaviour, IDetectionSystem, IDerivedEntityComponent
    {
        public event Action<ITargetable> OnTargetDetected;
        
        [field: SerializeField] public ScanComponent ScanComponent { get; private set; } = null;
        [field: SerializeField] public ScanCircleView CircleView { get; private set; } = null;

        private IWeaponPresenter _weaponPresenter;
        private bool _isEnabled = false;
        
        public virtual void Bootstrapp(Entity entity)
        {
            ScanComponent.Bootstrapp();
            CircleView.Bootstrapp();

            entity.Components.TryGetAbstractComponent(out _weaponPresenter);
        }

        public void Dispose()
        {
            Disable();
            
            ScanComponent.Dispose();
            CircleView.Dispose();
        }
        
        public IDetectionSystem Enable()
        {
            if (_isEnabled) return this;

            _isEnabled = true;
            
            _weaponPresenter.WeaponUpdated += OnWeaponUpdated;
            ScanComponent.DetectedEnemyUpdated += OnEntityUpdated;
            
            return this;
        }

        public IDetectionSystem Disable()
        {
            if (_isEnabled == false) return this;

            _isEnabled = false;
            
            _weaponPresenter.WeaponUpdated -= OnWeaponUpdated;
            ScanComponent.DetectedEnemyUpdated -= OnEntityUpdated;
            
            return this;
        }
        
        public ITargetable GetCurrentTarget() => ScanComponent.CachedEntity as ITargetable;
        
        public void SetDetectionRadius(float radius)
        {
            CircleView.Show(radius);
            
            ScanComponent.UpdateScanningDistance(radius);
            ScanComponent.LaunchScanning();
        }

        private void OnEntityUpdated(Entity entity) => OnTargetDetected?.Invoke(entity as Enemy);

        private void OnWeaponUpdated(WeaponConfig weaponConfig)
        {
            if (weaponConfig == null)
            {
                ScanComponent.StopScanning();
                return;
            }
            
            SetDetectionRadius(weaponConfig.WeaponStats.BaseRange);
        }
    }
}