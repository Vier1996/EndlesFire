using System;
using System.Collections.Generic;
using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Talents
{
    // Talents can be:
    // 1. Increase MovementSpeed
    // 2. Increase ShootingRate
    // 3. Decrease Recharging
    
    // 4. LightingStrike : Condition (doesnt contains some talent) : if contains this talent chance do drop siblings skill is 80%
    // 5. LightingStrike recharging delay : Condition (contains LightingStrike talent)
    // 6. LightingStrike damage : Condition (contains LightingStrike talent)
    // 7. LightingStrike radius : Condition (contains LightingStrike talent)
    
    // 8. Electrical Bullet : Condition (weapon doesnt contains inlined electrical bullets)
    // 9. Poisoned Bullet : Condition (weapon doesnt contains inlined poisoned bullets)
    // 10. Burned Bullet : Condition (weapon doesnt contains inlined burned bullets)
    
    [CreateAssetMenu(fileName = nameof(TalentSetup), menuName = "App/Configs/Talents/" + nameof(TalentSetup))]
    public class TalentSetup : ScriptableObject
    {
        [field: SerializeField] public TalentType TalentType { get; private set; } = TalentType.none;
        [field: SerializeField] public string TalentNameKey { get; private set; } = "";
        [field: SerializeField] public string TalentDescriptionKey { get; private set; } = "";
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public bool ContainGrades { get; private set; } = true;
        [field: SerializeField, ShowIf(nameof(ContainGrades))] public List<TalentGrade> Grades { get; private set; } = new();

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