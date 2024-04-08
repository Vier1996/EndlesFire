using InternalAssets.Codebase.Gameplay.Damage;

namespace InternalAssets.Codebase.Interfaces
{
    public interface IDamageReceiver
    {
        void ReceiveDamage(DamageArgs damageArgs);
    }
}