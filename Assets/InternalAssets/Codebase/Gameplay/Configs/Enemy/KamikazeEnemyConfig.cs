using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Configs.Enemy
{
    [CreateAssetMenu(fileName = nameof(KamikazeEnemyConfig), menuName = "App/Configs/Enemy/" + nameof(KamikazeEnemyConfig))]
    public class KamikazeEnemyConfig : EnemyConfig
    {
        [field: SerializeField] public float DetonateAttackDamage { get; private set; }
        [field: SerializeField] public float DetonateDelay { get; private set; }
    }
}