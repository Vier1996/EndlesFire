using System;
using System.Collections.Generic;
using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Talents.TalentsSetups
{
    [Serializable]
    public class MovementIncreasingTalent : ITalentSetup
    {
        [field: SerializeField] public TalentType TalentType { get; private set; } = TalentType.none;
        [field: SerializeField] public bool ContainGrades { get; private set; } = true;
        [field: SerializeField, ShowIf(nameof(ContainGrades))] public List<TalentGrade> Grades { get; private set; } = new();
    }
}