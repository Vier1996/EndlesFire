using System;
using Codebase.Library.SAD;
using Cysharp.Threading.Tasks;
using InternalAssets.Codebase.Gameplay.Behavior.Enemy;
using InternalAssets.Codebase.Gameplay.Configs.Enemy;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Services.Animation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public abstract class Enemy : Entity, IEnemy, ITargetable, IDamageReceiver
    {
        public EnemyConfig EnemyConfig { get; protected set; }
        public ITargetable CurrentTarget { get; protected set; }
        
        public virtual async UniTask<Enemy> Initialize(EnemyType enemyType)
        {
            EnemyConfigsContainer configsContainer = await EnemyConfigsContainer.GetInstanceAsync();

            EnemyConfig = configsContainer.Get(enemyType);
            
            configsContainer.Release();
            
            if(TryGetAbstractComponent(out EnemyBehaviorMachine machine))
                machine.Initialize(EnemyConfig);
            
            return this;
        }

        [Button]
        public virtual void SetTarget(ITargetable targetable) => CurrentTarget = targetable;
        public virtual Transform GetTargetTransform() => transform;
        public virtual void EnableMarker() { }
        public virtual void DisableMarker() { }
        public virtual void ReceiveDamage(DamageArgs damageArgs) { }
    }
    
    [Serializable]
    public class EnemyComponents : EntityComponents
    {
        [BoxGroup("Enemy components"), SerializeField] private SpriteSheetAnimator _spriteSheetAnimator;

        public override EntityComponents Declare(Entity abstractEntity)
        {
            Add(typeof(Entity), abstractEntity);
            Add(typeof(EnemyBehaviorMachine), new EnemyBehaviorMachine());
            Add(_spriteSheetAnimator);
            
            return this;
        }
    }
}