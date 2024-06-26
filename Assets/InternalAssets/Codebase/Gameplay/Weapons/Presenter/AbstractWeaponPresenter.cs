﻿using System;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.Presenter
{
    public abstract class AbstractWeaponPresenter : MonoBehaviour, IWeaponPresenter, IDerivedEntityComponent
    {
        public event Action<WeaponConfig> WeaponUpdated;
        
        public abstract void Bootstrapp(Entity entity);
        public virtual void Dispose() { }
        
        public virtual IWeaponPresenter Enable() => this;
        public virtual IWeaponPresenter Disable() => this;
        public virtual void PresentWeapon(WeaponType weaponType) { }
        public void ShowPresenter() => gameObject.SetActive(true);
        public void HidePresenter() => gameObject.SetActive(false);

        protected void DispatchWeaponUpdatedEvent(WeaponConfig weaponConfig) => 
            WeaponUpdated?.Invoke(weaponConfig);
    }
}