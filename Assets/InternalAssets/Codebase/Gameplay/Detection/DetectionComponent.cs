using System;
using System.Collections.Generic;
using ACS.Core.ServicesContainer;
using Codebase.Library.Extension.Rx;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Detection
{
    public class DetectionComponent : SerializedMonoBehaviour, IInitializable<DetectionComponent>
    {
        public event Action<Entity> DetectedEnemyUpdated;
        
        [OdinSerialize] private List<Type> _listeningEntities = new();
            
        private IDisposable _scanDisposable;
        private EntityWorld _entityWorld;
        private Transform _selfTransform;
        private Entity _cachedEntity;
        
        public DetectionComponent Bootstrapp()
        {
            ServiceContainer.ForCurrentScene().Get(out _entityWorld);

            _selfTransform = transform;
            
            return this;
        }

        public void Dispose() => _scanDisposable?.Dispose();

        public void LaunchScanning()
        {
            _scanDisposable?.Dispose();
            _scanDisposable = RX.LoopedTimer(1f, 1f, Scan);
        }

        public void StopScanning() => _scanDisposable?.Dispose();

        private void Scan()
        {
            Entity entity = _entityWorld.GetClosestEntity(_listeningEntities, _selfTransform.position , 2f);

            if (entity == null || _cachedEntity == entity)
                return;

            DetectedEnemyUpdated?.Invoke(_cachedEntity = entity);
        }
    }
}