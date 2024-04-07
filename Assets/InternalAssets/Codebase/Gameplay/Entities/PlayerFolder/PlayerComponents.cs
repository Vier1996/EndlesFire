﻿using System;
using Codebase.Gameplay.Sorting;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Behavior.Player;
using InternalAssets.Codebase.Gameplay.Dodge;
using InternalAssets.Codebase.Gameplay.Movement;
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
        [BoxGroup("General"), SerializeField] private SortableItem _sortableItem;
        
        [BoxGroup("Physic"), SerializeField] private Rigidbody2D _rigidbody2D;
        
        public override EntityComponents Declare(Entity abstractEntity)
        {
            Add(abstractEntity);
            Add(_rigidbody2D);
            Add(_sortableItem);
            Add(typeof(PlayerBehaviorMachine), new PlayerBehaviorMachine());
            Add(_playerMovementComponent);
            Add(_playerAnimator);
            Add(_dodgeComponent);
            
            return this;
        }
    }
}