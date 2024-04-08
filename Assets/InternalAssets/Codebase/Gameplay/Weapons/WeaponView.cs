using System;
using System.Collections.Generic;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons
{
    public class WeaponView : MonoBehaviour, IWeapon, IDisposable
    {
        [SerializeField] private List<Renderer> _renderers = new();
        [SerializeField] protected WeaponSpark WeaponSpark;

        [field: SerializeField, ReadOnly] public bool InShootingStatus { get; private set; } = false;
        [field: SerializeField, ReadOnly] public bool InRechargingStatus { get; private set; } = false;
        
        protected WeaponConfig WeaponConfig;
        
        public virtual void Bootstrapp(WeaponConfig weaponConfig)
        {
            WeaponConfig = weaponConfig;
        }
        
        public virtual void Dispose() { }
        
        public virtual void Fire(Vector3 targetPosition) { }

        public List<Renderer> GetWeaponRenderers() => _renderers;
    }
}
