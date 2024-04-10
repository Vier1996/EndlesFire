using System.Collections.Generic;
using Codebase.Library.Extension.ScriptableObject;
using InternalAssets.Codebase.Gameplay.Behavior.Enemy;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace InternalAssets.Codebase.Gameplay.Configs.Enemy
{
    public abstract class EnemyConfig : LoadableScriptableObject<EnemyConfig>
    {
        [BoxGroup("Behavior"), OdinSerialize] public List<IEnemyBehavior> Behavior { get; private set; } = new();
    }
}