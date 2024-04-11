using InternalAssets.Codebase.Gameplay.Damage;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public class InvisiblePuppet : Enemy
    {
        public override void ReceiveDamage(DamageArgs damageArgs)
        {
            UnityEngine.Debug.Log($"Получил пизды на {damageArgs.Damage} урона");
        }
    }
}
