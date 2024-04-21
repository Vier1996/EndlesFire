using System;
using System.Collections.Generic;
using InternalAssets.Codebase.Gameplay.Enums;

namespace InternalAssets.Codebase.Gameplay.Talents
{
    public interface ITalentSetup
    {
        public TalentType TalentType { get; }
        public bool ContainGrades { get; }
        public List<TalentGrade> Grades { get; }
    }

    [Serializable]
    public class TalentGrade
    {
        
    }
    
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
}