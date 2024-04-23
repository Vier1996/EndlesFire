namespace InternalAssets.Codebase.Gameplay.Enums
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
    
    public enum TalentType
    {
        none = 0,
        
        electrical_bullet = 1,
        poisoned_bullet = 2,
        burned_bullet = 3,
        
        movement_increasing_speed = 20,
        increasing_shooting_speed = 21,
        decreasing_recharging = 22,
        
        lightning_strike = 30,
        lightning_strike_decrease_recharge = 31,
        lightning_strike_damage = 32,
        lightning_strike_radius = 33,
    }
}