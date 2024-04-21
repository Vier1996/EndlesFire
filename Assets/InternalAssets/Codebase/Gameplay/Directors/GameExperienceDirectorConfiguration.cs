using System;
using System.Collections.Generic;
using Codebase.Library.Extension.ScriptableObject;
using Codebase.Library.Random;
using InternalAssets.Codebase.Gameplay.Items;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Directors
{
    [CreateAssetMenu(fileName = nameof(GameExperienceDirectorConfiguration), menuName = "App/Configs/GameplayDirector/" + nameof(GameExperienceDirectorConfiguration))]
    public class GameExperienceDirectorConfiguration : LoadableScriptableObject<GameExperienceDirectorConfiguration>
    {
        [field: SerializeField, BoxGroup("Spawn params")] public float MinimalDistanceFromPlayer { get; private set; }
        [field: SerializeField, BoxGroup("Spawn params")] public float MaximalDistanceFromPlayer { get; private set; }
        [field: SerializeField, BoxGroup("Spawn params")] public float SpawnDelay { get; private set; }
        [field: SerializeField, BoxGroup("Spawn params")] public int MaxItemsCount { get; private set; }
        [field: SerializeField, BoxGroup("Initial spawn")] public int InitialMinItemsCount { get; private set; }
        [field: SerializeField, BoxGroup("Initial spawn")] public int InitialMaxItemsCount { get; private set; }
        [field: SerializeField, BoxGroup("Garbage Collecting")] public float RefreshTime { get; private set; }
        [field: SerializeField, BoxGroup("Garbage Collecting")] public float ClearDistance { get; private set; }
        [field: OdinSerialize, BoxGroup("View params")] public List<ChancedGameExperience> GameExperienceChanced { get; private set; } = new();

        [Serializable]
        public class ChancedGameExperience : IContainsPercent
        {
            [SerializeField] private int _weight = 0;
            [field: SerializeField] public ItemData ItemData { get; private set; } = new();

            public int GetWeight() => _weight;
        }
    }
}