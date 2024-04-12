using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.Presenter
{
    public abstract class AbstractWeaponPresenter : MonoBehaviour, IWeaponPresenter, IDerivedEntityComponent
    {
        public event Action<WeaponConfig> WeaponUpdated;
        public void PresentWeapon(WeaponType weaponType)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}