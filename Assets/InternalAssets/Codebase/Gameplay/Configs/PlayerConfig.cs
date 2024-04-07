using System.Collections.Generic;
using Codebase.Library.Extension.ScriptableObject;
using InternalAssets.Codebase.Gameplay.Behavior.Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Configs
{
    [CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "App/Configs/" + nameof(PlayerConfig))]
    public class PlayerConfig : LoadableScriptableObject<PlayerConfig>
    {
        [field: OdinSerialize, BoxGroup("Behavior")] public List<IPlayerBehavior> PlayerBehavior { get; private set; }
    }
}