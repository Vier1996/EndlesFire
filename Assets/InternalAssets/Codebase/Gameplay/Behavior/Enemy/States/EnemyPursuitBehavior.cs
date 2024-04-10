﻿using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Library.Behavior;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Behavior.Enemy.States
{
    public class EnemyPursuitBehavior : EnemyBehaviorState
    {
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
        
        public EnemyPursuitBehavior(EnemyPursuitBehavior other) => IsDefaultBehavior = other.IsDefaultBehavior;

        public override void Construct(EntityComponents components)
        {
            PlayerComponents = components as PlayerComponents;

            if (PlayerComponents == null)
                throw new ArgumentException("Components are null or can not convert to [EnemyComponents]");
        }
        
        public override void Enter(BehaviorComponents behaviorComponents = null)
        {
        }

        public override void Exit()
        {
        }
    }
}