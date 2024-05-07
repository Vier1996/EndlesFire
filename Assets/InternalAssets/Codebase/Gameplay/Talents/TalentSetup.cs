using System;
using System.Collections.Generic;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Library.GameConditions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Talents
{
    [CreateAssetMenu(fileName = nameof(TalentSetup), menuName = "App/Configs/Talents/" + nameof(TalentSetup))]
    public class TalentSetup : SerializedScriptableObject
    {
        [field: SerializeField] public TalentType TalentType { get; private set; } = TalentType.none;
        [field: SerializeField] public TalentActivityType TalentActivityType { get; private set; } = TalentActivityType.none;
        
        [field: SerializeField] public string TalentNameKey { get; private set; } = "";
        [field: SerializeField] public string TalentDescriptionKey { get; private set; } = "";
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public bool ContainGrades { get; private set; } = true;
        
        [field: SerializeField, ShowIf(nameof(ContainGrades))] public List<TalentGrade> Grades { get; private set; } = new();
        [field: OdinSerialize, ShowIf(nameof(ContainGrades))] public List<IGameCondition> PickActions { get; private set; } = new();

        public TalentGrade GetGrade(int level)
        {
            if (level > Grades.Count || level <= 0)
                throw new IndexOutOfRangeException($"Level is [{level}], out of range TalentSetups:[{TalentType}]");

            return Grades[level - 1];
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            string assetPath = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(assetPath, TalentType.ToString().ToUpper());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}