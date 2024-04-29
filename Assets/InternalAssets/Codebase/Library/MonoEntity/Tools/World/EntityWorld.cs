using System;
using System.Collections.Generic;
using ACS.Core.ServicesContainer;
using ACS.SignalBus.SignalBus;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.Signals;
using InternalAssets.Codebase.Library.Vectors;
using JetBrains.Annotations;
using UnityEngine;

namespace InternalAssets.Codebase.Library.MonoEntity.Tools.World
{
    public class EntityWorld : IDisposable
    {
        private readonly ISignalBusService _signalBusService;
        
        private readonly List<Entity> _mAliveEntities = new();

        public EntityWorld()
        {
            ServiceContainer.Core.Get(out _signalBusService);
            
            _signalBusService.Subscribe<EntityCreatedSignal>(AddEntity);
            _signalBusService.Subscribe<EntityDestroyedSignal>(RemoveEntity);
        }
        
        public void Dispose()
        {
            _signalBusService.Unsubscribe<EntityCreatedSignal>(AddEntity);
            _signalBusService.Unsubscribe<EntityDestroyedSignal>(RemoveEntity);
        }

        [CanBeNull]
        public T GetClosestEntity<T>(Vector3 point, float distance) where T : Entity
        {
            T outputEntity = null;
            float minimalLength = 100f;
            
            for (int i = 0; i < _mAliveEntities.Count; i++)
            {
                T entity = _mAliveEntities[i] as T;
                
                if (entity == null)
                    continue;

                float distanceBetweenEntities = point.DistanceXY(entity.Transform.position);

                if (distanceBetweenEntities <= distance && distanceBetweenEntities < minimalLength)
                {
                    outputEntity = entity;
                    minimalLength = distanceBetweenEntities;
                }
            }

            return outputEntity;
        }
        
        [CanBeNull]
        public Entity GetClosestEntity(List<Type> entityTypes, Vector3 point, float distance)
        {
            Entity outputEntity = null;
            float minimalLength = 100f;
            
            for (int i = 0; i < _mAliveEntities.Count; i++)
            {
                Entity entity = _mAliveEntities[i];
                
                if (entity.gameObject.activeSelf == false || entityTypes.Contains(entity.GetType()) == false) continue;
                if (entity == null) continue;

                float distanceBetweenEntities = point.DistanceXY(entity.Transform.position);

                if (distanceBetweenEntities <= distance && distanceBetweenEntities < minimalLength)
                {
                    outputEntity = entity;
                    minimalLength = distanceBetweenEntities;
                }
            }

            return outputEntity;
        }
        
        private void AddEntity(EntityCreatedSignal signal)
        {
            if(_mAliveEntities.Contains(signal.Entity) == false)
                _mAliveEntities.Add(signal.Entity);
        }
        
        private void RemoveEntity(EntityDestroyedSignal signal) 
        {
            if(_mAliveEntities.Contains(signal.Entity))
                _mAliveEntities.Remove(signal.Entity);
        }
    }
}
