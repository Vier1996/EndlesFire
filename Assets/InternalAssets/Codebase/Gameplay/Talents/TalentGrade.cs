using System;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Talents
{
    [Serializable]
    public class TalentGrade
    {
        [field: SerializeField] public int Level { get; private set; } = 1;
        [field: SerializeField] public float Percent { get; private set; } = 1f;
    }
}