using System.Collections.Generic;
using InternalAssets.Codebase.Gameplay.Behavior.Enemy;
using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Configs.Enemy
{
    public abstract class EnemyConfig : SerializedScriptableObject
    {
        [field: SerializeField, BoxGroup("Type"), PropertyOrder(-1)] public EnemyType Type { get; private set; } = EnemyType.none;
        [OdinSerialize, BoxGroup("Behavior")] public List<IEnemyBehavior> Behavior { get; private set; } = new();
    }
}