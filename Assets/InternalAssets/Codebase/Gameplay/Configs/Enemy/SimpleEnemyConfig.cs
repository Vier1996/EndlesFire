using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Configs.Enemy
{
    [CreateAssetMenu(fileName = nameof(SimpleEnemyConfig), menuName = "App/Configs/Enemy/" + nameof(SimpleEnemyConfig))]
    public class SimpleEnemyConfig : EnemyConfig
    {
        [field: SerializeField, BoxGroup("Params")] public float SimpleAttackDamage { get; private set; }
        [field: SerializeField, BoxGroup("Params")] public float SimpleDelay { get; private set; }
    }
}