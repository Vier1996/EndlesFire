using System;
using Cysharp.Threading.Tasks.Triggers;
using InternalAssets.Codebase.Gameplay.Scanning;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public class EnemyDetectionSystem : MonoBehaviour, IDetectionSystem, IDerivedEntityComponent
    {
        public event Action<ITargetable> OnTargetDetected;
        
        [field: SerializeField] public ScanComponent ScanComponent { get; private set; } = null;
        
        private bool _isEnabled = false;
        
        public virtual void Bootstrapp(Entity entity) => ScanComponent.Bootstrapp();

        public void Dispose() => ScanComponent.Dispose();

        public IDetectionSystem Enable()
        {
            if (_isEnabled) return this;

            _isEnabled = true;
            
            ScanComponent.DetectedEnemyUpdated += OnEntityUpdated;

            return this;
        }

        public IDetectionSystem Disable()
        {
            if (_isEnabled == false) return this;

            _isEnabled = false;
            
            ScanComponent.DetectedEnemyUpdated -= OnEntityUpdated;
            
            return this;
        }

        public void SetDetectionRadius(float radius) => OnDetectionRadiusUpdated(radius);

        public ITargetable GetCurrentTarget() => ScanComponent.CachedEntity as ITargetable;

        private void OnEntityUpdated(Entity entity) => OnTargetDetected?.Invoke(entity as ITargetable);

        private void OnDetectionRadiusUpdated(float radius)
        {
            if (radius <= 0)
            {
                ScanComponent.StopScanning();
                return;
            }
            
            ScanComponent.UpdateScanningDistance(radius);
            ScanComponent.LaunchScanning();
        }
    }
}