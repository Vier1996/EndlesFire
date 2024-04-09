using System;
using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.Configs
{
    [CreateAssetMenu(fileName = nameof(WeaponConfig), menuName = "App/Configs/Weapon/" + nameof(WeaponConfig))]
    public class WeaponConfig : ScriptableObject
    {
        [field: SerializeField, BoxGroup("Type")] public WeaponType WeaponType { get; private set; } = WeaponType.none;
        [field: SerializeField, BoxGroup("Type")] public WeaponView ViewPrefab { get; private set; } = null;
        [field: SerializeField, BoxGroup("Weapon")] public WeaponStats WeaponStats { get; private set; } = new();
        [field: SerializeField, BoxGroup("Weapon store")] public WeaponStoreStats WeaponStoreStats { get; private set; } = new();
        [field: SerializeField, BoxGroup("Ammo")] public WeaponAmmoStats WeaponAmmoStats { get; private set; } = new();
        [field: SerializeField, BoxGroup("Animation")] public WeaponAnimationStats WeaponAnimationStats { get; private set; } = new();
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            string assetPath = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(assetPath, WeaponType.ToString().ToUpper());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
    
    [Serializable]
    public class WeaponStats
    {
        [field: SerializeField] public float BaseRange { get; private set;} = 0f;
        [field: SerializeField] public float BaseRecharging { get; private set;} = 0f;
        [field: SerializeField] public float WaterConsumptionValue { get; private set;} = 0f;
        [field: SerializeField] public float FireDisperse { get; private set;} = 0f;
        [field: SerializeField] public float AimingTime { get; private set;} = 0f;
        [field: SerializeField] public bool IgnoreWallsRaycast { get; private set;} = false;
    }

    [Serializable]
    public class WeaponAmmoStats
    {
        [field: SerializeField] public float Damage { get; private set;} = 0f;
        [field: SerializeField] public float CriticalChance { get; private set;} = 0f;
        [field: SerializeField] public float CriticalDamageMultiplier { get; private set;} = 0f;
        [field: SerializeField] public float BulletMovementSpeed { get; private set;} = 0f;
        [field: SerializeField] public float PushMultiplier { get; private set; } = 1f;
        [field: SerializeField, BoxGroup("AOE")] public bool HaveRadiusDamage { get; private set; } = false;
        [field: SerializeField, BoxGroup("AOE")] public float Radius { get; private set; } = 0f;
        [field: SerializeField, BoxGroup("Auto destroy")] public bool DestructByLifeTime { get; private set; } = false;
        [field: SerializeField, BoxGroup("Auto destroy")] public float LifeTime { get; private set; } = 0f;
    }

    [Serializable]
    public class WeaponStoreStats
    {
        [field: SerializeField, BoxGroup("Queue")] public int BulletsByQueue { get; private set; } = 1;
        [field: SerializeField, BoxGroup("Queue"), ShowIf("@BulletsByQueue > 1"), Min(0.05f)] public float DelayBetweenBullets { get; private set; } = 0f;
        [field: SerializeField, BoxGroup("Queue")] public bool QueueAsOneBullets { get; private set; } = false;
        [field: SerializeField, BoxGroup("Queue")] public bool AnimateEveryShoot { get; private set; } = false;
    }
    
    [Serializable]
    public class WeaponAnimationStats
    {
        [field: SerializeField] public float AnimationOffset { get; private set; } = -0.05f;
        [field: SerializeField] public float AnimationDuration { get; private set; } = 0.3f;
        [field: SerializeField] public float AnimationBalance { get; private set; } = 0.315f;
    }
}