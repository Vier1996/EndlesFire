using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    public class Player : Entity, IDamageReceiver, ITargetable, IRecycledClass<Player>
    {
        [SerializeField] private PlayerComponents _playerComponents;

        private bool _isEnabled = false;
        
        [Button]
        public override Entity Bootstrapp() => 
            base.Bootstrapp().BindComponents(_playerComponents);
        
        [Button]
        public Player Initialize()
        {
            Enable();
            
            return this;
        }

        public Player Enable()
        {
            if (_isEnabled) return this;

            _isEnabled = true;

            GetAbstractComponent<IDetectionSystem>().Enable();
            GetAbstractComponent<IWeaponPresenter>().Enable();

            return this;
        }

        public Player Disable()
        {
            if (_isEnabled == false) return this;

            _isEnabled = false;

            GetAbstractComponent<IDetectionSystem>().Disable();
            GetAbstractComponent<IWeaponPresenter>().Disable();
            
            return this;
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
