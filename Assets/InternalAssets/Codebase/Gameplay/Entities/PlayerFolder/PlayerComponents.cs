using System;
using Codebase.Gameplay.Sorting;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Behavior.Player;
using InternalAssets.Codebase.Gameplay.Dodge;
using InternalAssets.Codebase.Gameplay.HealthLogic;
using InternalAssets.Codebase.Gameplay.ModelsView;
using InternalAssets.Codebase.Gameplay.Movement;
using InternalAssets.Codebase.Gameplay.Weapons.Presenter;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    [Serializable]
    public class PlayerComponents : EntityComponents
    {
        [BoxGroup("General"), SerializeField] private PlayerMovementComponent _playerMovementComponent;
        [BoxGroup("General"), SerializeField] private ModelViewProvider _modelViewProvider;
        [BoxGroup("General"), SerializeField] private DodgeComponent _dodgeComponent;
        [BoxGroup("General"), SerializeField] private WeaponPresenter _weaponPresenter;
        [BoxGroup("General"), SerializeField] private SortableItem _sortableItem;
        [BoxGroup("General"), SerializeField] private PlayerDetectionSystem _playerDetectionSystem;
        [BoxGroup("General"), SerializeField] private HealthComponent _healthComponent;
        
        [BoxGroup("Physic"), SerializeField] private Rigidbody2D _rigidbody2D;
        
        public override EntityComponents Declare(Entity abstractEntity)
        {
            Add(abstractEntity);
            Add(_healthComponent);
            Add(_rigidbody2D);
            Add(_sortableItem);
            Add(_modelViewProvider);
            Add(typeof(PlayerBehaviorMachine), new PlayerBehaviorMachine());
            Add(_playerMovementComponent);
            Add(typeof(IWeaponPresenter), _weaponPresenter);
            Add(_dodgeComponent);
            Add(typeof(IDetectionSystem), _playerDetectionSystem);
            
            return this;
        }
    }
}