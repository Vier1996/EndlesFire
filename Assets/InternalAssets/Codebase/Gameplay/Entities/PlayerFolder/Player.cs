using InternalAssets.Codebase.Gameplay.Characteristics;
using InternalAssets.Codebase.Gameplay.Configs;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.HealthLogic;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.EntityComponent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    public class Player : Entity, IDamageReceiver, ITargetable, IRecycledClass<Player>
    {
        [SerializeField] private PlayerComponents _playerComponents;

        private IDetectionSystem _detectionSystem;
        private IWeaponPresenter _weaponPresenter;
        private HealthComponent _healthComponent;
        
        private bool _isEnabled = false;
        
        [Button]
        public override Entity Bootstrapp(EntityComponents components = null) => base.Bootstrapp(_playerComponents);

        public Player Initialize()
        {
            Components.TryGetAbstractComponent(out _healthComponent);
            Components.TryGetAbstractComponent(out _detectionSystem);
            Components.TryGetAbstractComponent(out _weaponPresenter);

            _healthComponent.Initialize(20);
         
            Enable();
            
            return this;
        }

        public Player Enable()
        {
            if (_isEnabled) return this;

            _isEnabled = true;

            _detectionSystem.Enable();
            _weaponPresenter.Enable().PresentWeapon(WeaponType.prototype_1);
            _healthComponent.Enable();

            _healthComponent.HealthEmpty += OnKilled;

            return this;
        }

        public Player Disable()
        {
            if (_isEnabled == false) return this;

            _isEnabled = false;

            _detectionSystem.Disable();
            _weaponPresenter.Disable();
            
            return this;
        }

        public void ReceiveDamage(DamageArgs damageArgs)
        {
            if (_healthComponent == null) return;
            
            _healthComponent.Operate(damageArgs);
        }

        public Transform GetTargetTransform() => _playerComponents.TargetTransform;

        public void EnableMarker()
        {
        }

        public void DisableMarker()
        {
        }
        
        protected virtual void OnKilled()
        {
            _healthComponent.HealthEmpty -= OnKilled;
            _healthComponent.Disable();

            Disable();
            
            GameObject.SetActive(false);
        }
        
#if UNITY_EDITOR

        [Button]
        private void GetCharacteristicsInfo()
        {
            Components.TryGetAbstractComponent(out CharacteristicsContainer characteristicsContainer);
            
            characteristicsContainer 
                .GetAll()
                .ForEach(crt => Debug.Log($"Characteristic:[{crt.Type}] has value:[{crt.Value}]"));
        }

#endif
    }
}
