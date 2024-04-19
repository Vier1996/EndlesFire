using System;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;

namespace InternalAssets.Codebase.Interfaces
{
    public interface IWeaponPresenter : IRecycledClass<IWeaponPresenter>
    {
        public event Action<WeaponConfig> WeaponUpdated;

        public void PresentWeapon(WeaponType weaponType);
        
        public void ShowPresenter();
        public void HidePresenter();
    }
}