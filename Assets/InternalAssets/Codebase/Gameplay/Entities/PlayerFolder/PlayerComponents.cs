using System;
using Codebase.Gameplay.Sorting;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Behavior.Player;
using InternalAssets.Codebase.Gameplay.Dodge;
using InternalAssets.Codebase.Gameplay.Movement;
using InternalAssets.Codebase.Gameplay.Weapons.Presenter;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Services._2dModels;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    [Serializable]
    public class PlayerComponents : EntityComponents
    {
        [BoxGroup("General"), SerializeField] private PlayerMovementComponent _playerMovementComponent;
        [BoxGroup("General"), SerializeField] private PlayerAnimator _playerAnimator;
        [BoxGroup("General"), SerializeField] private DodgeComponent _dodgeComponent;
        [BoxGroup("General"), SerializeField] private WeaponPresenter _weaponPresenter;
        [BoxGroup("General"), SerializeField] private SpriteModelPresenter _spriteModelPresenter;
        [BoxGroup("General"), SerializeField] private SortableItem _sortableItem;
        [BoxGroup("General"), SerializeField] private PlayerDetectionSystem _playerDetectionSystem;
        
        [BoxGroup("Physic"), SerializeField] private Rigidbody2D _rigidbody2D;
        
        public override EntityComponents Declare(Entity abstractEntity)
        {
            Add(abstractEntity);
            Add(_rigidbody2D);
            Add(_sortableItem);
            Add(typeof(PlayerBehaviorMachine), new PlayerBehaviorMachine());
            Add(_playerMovementComponent);
            Add(_spriteModelPresenter);
            Add(_weaponPresenter);
            Add(_playerAnimator);
            Add(_dodgeComponent);
            Add(type: typeof(IDetectionSystem), _playerDetectionSystem);
            
            return this;
        }
    }
}