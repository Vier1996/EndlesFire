using System;
using System.Collections.Generic;
using Codebase.Library.Extension.ScriptableObject;
using Codebase.Library.Random;
using InternalAssets.Codebase.Gameplay.Items;
using Sirenix.Serialization;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Directors
{
    [CreateAssetMenu(fileName = nameof(GameExperienceDirectorConfiguration), menuName = "App/Configs/GameplayDirector/" + nameof(GameExperienceDirectorConfiguration))]
    public class GameExperienceDirectorConfiguration : LoadableScriptableObject<GameExperienceDirectorConfiguration>
    {
        [field: SerializeField] public float MinimalDistanceFromPlayer { get; private set; }
        [field: SerializeField] public float MaximalDistanceFromPlayer { get; private set; }
        [field: SerializeField] public float MaxItemsCount { get; private set; }
        [field: OdinSerialize] public List<ChancedGameExperience> GameExperienceChanced { get; private set; } = new();

        [Serializable]
        public class ChancedGameExperience : IContainsPercent
        {
            [SerializeField] private int _weight = 0;
            [field: SerializeField] public ItemData ItemData { get; private set; } = new();

            public int GetWeight() => _weight;
        }
    }
}