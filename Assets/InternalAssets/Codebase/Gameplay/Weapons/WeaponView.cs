using System;
using System.Collections.Generic;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons
{
    public abstract class WeaponView : MonoBehaviour, IWeapon, IDisposable
    {
        [field: SerializeField, ReadOnly] public bool InShootingStatus { get; protected set; } = false;
        [field: SerializeField, ReadOnly] public bool InShootingProcess { get; protected set; } = false;
        [field: SerializeField, ReadOnly] public bool InRechargingStatus { get; protected set; } = false;

        public WeaponConfig WeaponConfig { get; private set; }
        
        protected Entity OwnerEntity;
        protected Transform SelfTransform;
        protected Vector3 DefaultWeaponLocalPosition;

        public virtual void Bootstrapp(WeaponConfig weaponConfig, Entity ownerEntity)
        {
            WeaponConfig = weaponConfig;
            OwnerEntity = ownerEntity;
            
            SelfTransform = transform;
            DefaultWeaponLocalPosition = SelfTransform.localPosition;
        }
        
        public virtual void Dispose() { }
        
        public virtual void StartFire(ITargetable target) { }
        public virtual void StopFire() { }
        
        protected abstract void AnimateWeaponFire();
    }
}
