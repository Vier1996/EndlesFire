using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.EnemyWeapons
{
    public abstract class EnemyWeaponView : MonoBehaviour, IWeapon
    {
        public virtual void StartFire(ITargetable target) { }
        public virtual void StopFire() { }

        public abstract void AnimateWeaponFire();
    }
}