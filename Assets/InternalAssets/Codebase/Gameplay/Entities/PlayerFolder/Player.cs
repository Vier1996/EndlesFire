using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.HealthLogic;
using InternalAssets.Codebase.Interfaces;
using Lean.Pool;
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
        
        public Player Initialize()
        {
            Enable();
            
            HealthComponent healthComponent = GetAbstractComponent<HealthComponent>();
            healthComponent.Initialize(20);
            
            return this;
        }

        public Player Enable()
        {
            if (_isEnabled) return this;

            _isEnabled = true;

            GetAbstractComponent<IDetectionSystem>().Enable();
            GetAbstractComponent<IWeaponPresenter>().Enable().PresentWeapon(WeaponType.prototype_1);
            HealthComponent healthComponent = GetAbstractComponent<HealthComponent>();

            healthComponent.Enable();

            healthComponent.HealthEmpty += OnKilled;

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
            if (TryGetAbstractComponent(out HealthComponent healthComponent) == false) return;
            
            healthComponent.Operate(damageArgs);
        }

        public Transform GetTargetTransform() => Transform;

        public void EnableMarker()
        {
        }

        public void DisableMarker()
        {
        }
        
        protected virtual void OnKilled()
        {
            HealthComponent healthComponent = GetAbstractComponent<HealthComponent>();

            healthComponent.HealthEmpty -= OnKilled;
            healthComponent.Disable();

            Disable();
            
            GameObject.SetActive(false);
        }
    }
}
