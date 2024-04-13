using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Configs.Enemy
{
    public abstract class EnemyConfig : SerializedScriptableObject
    {
        [field: SerializeField, BoxGroup("Type"), PropertyOrder(-1)] public EnemyType Type { get; private set; } = EnemyType.none;
    }
}