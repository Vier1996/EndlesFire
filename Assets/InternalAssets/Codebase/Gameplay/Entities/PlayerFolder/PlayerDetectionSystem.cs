using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Enemies;
using InternalAssets.Codebase.Gameplay.Scanning;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Gameplay.Weapons.Presenter;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    public class PlayerDetectionSystem : MonoBehaviour, IDerivedEntityComponent
    {
        public event Action<ITargetable> OnTargetDetected;
        
        [field: SerializeField] public ScanComponent ScanComponent { get; private set; } = null;
        [field: SerializeField] public ScanCircleView CircleView { get; private set; } = null;

        private WeaponPresenter _weaponPresenter;
        
        public virtual void Bootstrapp(Entity entity)
        {
            ScanComponent.Bootstrapp();
            CircleView.Bootstrapp();

            _weaponPresenter = entity.GetAbstractComponent<WeaponPresenter>();

            EnableDetection();
            OnWeaponUpdated(_weaponPresenter.CurrentView?.WeaponConfig);
            
            _weaponPresenter.WeaponUpdated += OnWeaponUpdated;
        }

        public void Dispose()
        {
            _weaponPresenter.WeaponUpdated -= OnWeaponUpdated;

            DisableDetection();
            
            ScanComponent.Dispose();
            CircleView.Dispose();
        }

        private void EnableDetection() => ScanComponent.DetectedEnemyUpdated += OnEntityUpdated;

        private void DisableDetection() => ScanComponent.DetectedEnemyUpdated -= OnEntityUpdated;

        private void OnEntityUpdated(Entity entity) => OnTargetDetected?.Invoke(entity as Enemy);

        private void OnWeaponUpdated(WeaponConfig weaponConfig)
        {
            if (weaponConfig == null)
            {
                ScanComponent.StopScanning();
                return;
            }
            
            float newRadius = weaponConfig.WeaponStats.BaseRange;
            
            CircleView.Show(newRadius);
            
            ScanComponent.UpdateScanningDistance(newRadius);
            ScanComponent.LaunchScanning();
        }
    }
}