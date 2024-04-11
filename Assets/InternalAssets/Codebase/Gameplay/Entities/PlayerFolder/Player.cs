using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    public class Player : Entity, IDamageReceiver, ITargetable
    {
        [SerializeField] private PlayerComponents _playerComponents;

        [Button]
        public override Entity Bootstrapp()
        {
            return base.Bootstrapp().BindComponents(_playerComponents);
        }

        public void ReceiveDamage(DamageArgs damageArgs)
        {
            UnityEngine.Debug.Log($"Получил пизды на {damageArgs.Damage} урона");
        }

        public Transform GetTargetTransform() => Transform;

        public void EnableMarker()
        {
        }

        public void DisableMarker()
        {
        }
    }
}
