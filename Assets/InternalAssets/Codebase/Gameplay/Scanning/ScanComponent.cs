using System;
using System.Collections.Generic;
using ACS.Core.ServicesContainer;
using Codebase.Library.Extension.Rx;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Scanning
{
    public class ScanComponent : SerializedMonoBehaviour, IInitializable<ScanComponent>
    {
        public event Action<Entity> DetectedEnemyUpdated;
        
        [OdinSerialize] private List<Type> _listeningEntities = new();
            
        private IDisposable _scanDisposable;
        private EntityWorld _entityWorld;
        private Transform _selfTransform;
        private Entity _cachedEntity;

        private float _scanningDistance = 0;
        private bool _inScanningStatus = false;
        
        public ScanComponent Bootstrapp()
        {
            ServiceContainer.ForCurrentScene().Get(out _entityWorld);

            _selfTransform = transform;
            
            return this;
        }

        public void Dispose() => _scanDisposable?.Dispose();

        public void UpdateScanningDistance(float scanningDistance)
        {
            _scanningDistance = scanningDistance;
        }

        public void LaunchScanning()
        {
            if(_inScanningStatus)
                return;

            _inScanningStatus = true;
            
            _scanDisposable?.Dispose();
            _scanDisposable = RX.LoopedTimer(1f, 0.5f, Scan);
        }

        public void StopScanning()
        {
            if(_inScanningStatus == false)
                return;

            _inScanningStatus = false;
            
            _scanDisposable?.Dispose();
        }

        private void Scan()
        {
            if(_scanningDistance <= 0)
                return;
            
            Entity entity = _entityWorld.GetClosestEntity(_listeningEntities, _selfTransform.position, _scanningDistance);

            if (_cachedEntity == entity)
                return;

            DetectedEnemyUpdated?.Invoke(_cachedEntity = entity);
        }
    }
}