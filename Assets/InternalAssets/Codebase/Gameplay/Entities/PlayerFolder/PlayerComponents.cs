using System;
using Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Gameplay.Behavior.Player;
using InternalAssets.Codebase.Gameplay.Configs;
using InternalAssets.Codebase.Gameplay.Dodge;
using InternalAssets.Codebase.Gameplay.HealthLogic;
using InternalAssets.Codebase.Gameplay.ModelsView;
using InternalAssets.Codebase.Gameplay.Movement;
using InternalAssets.Codebase.Gameplay.Weapons.Presenter;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.EntityComponent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    [Serializable]
    public class PlayerComponents : EntityComponents
    {
        [field: SerializeField, BoxGroup("General")] public Transform TargetTransform { get; private set; }

        [BoxGroup("General"), SerializeField] private PlayerMovementComponent _playerMovementComponent;
        [BoxGroup("General"), SerializeField] private ModelViewProvider _modelViewProvider;
        [BoxGroup("General"), SerializeField] private DodgeComponent _dodgeComponent;
        [BoxGroup("General"), SerializeField] private WeaponPresenter _weaponPresenter;
        [BoxGroup("General"), SerializeField] private PlayerDetectionSystem _playerDetectionSystem;
        [BoxGroup("General"), SerializeField] private HealthComponent _healthComponent;
        
        [BoxGroup("Physic"), SerializeField] private Rigidbody2D _rigidbody2D;
        
        public override EntityComponents Declare(Entity abstractEntity)
        {
            PlayerConfig playerConfig = PlayerConfig.GetInstance();
            
            Add(abstractEntity);
            Add(playerConfig.CharacteristicsContainer.Clone());
            Add(_healthComponent);
            Add(_rigidbody2D);
            Add(_modelViewProvider);
            Add(typeof(PlayerBehaviorMachine), new PlayerBehaviorMachine());
            Add(_playerMovementComponent);
            Add(typeof(IWeaponPresenter), _weaponPresenter);
            Add(_dodgeComponent);
            Add(typeof(IDetectionSystem), _playerDetectionSystem);
            
            playerConfig.Release();
            
            return this;
        }
    }
}