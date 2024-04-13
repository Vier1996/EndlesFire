using System;
using Codebase.Gameplay.Sorting;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Configs.Enemy;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.HealthLogic;
using InternalAssets.Codebase.Gameplay.ModelsView;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public abstract class Enemy : Entity, IEnemy, ITargetable, IDamageReceiver
    {
        public EnemyConfig EnemyConfig { get; protected set; }
        
        public virtual Enemy Initialize(EnemyType enemyType)
        {
            EnemyConfigsContainer configsContainer = EnemyConfigsContainer.GetInstance();

            EnemyConfig = configsContainer.Get(enemyType);
            
            configsContainer.Release();
            
            return this;
        }

        public virtual Transform GetTargetTransform() => transform;
        public virtual void EnableMarker() { }
        public virtual void DisableMarker() { }
        public virtual void ReceiveDamage(DamageArgs damageArgs) { }
    }
    
    [Serializable]
    public class EnemyComponents : EntityComponents
    {
        [BoxGroup("Enemy components"), SerializeField] private ModelViewProvider _modelViewProvider;
        [BoxGroup("Enemy components"), SerializeField] private HealthComponent _healthComponent;
        [BoxGroup("Enemy components"), SerializeField] private SortableItem _sortableItem;

        public override EntityComponents Declare(Entity abstractEntity)
        {
            Add(typeof(Entity), abstractEntity);
            Add(_healthComponent);
            Add(_modelViewProvider);
            Add(_sortableItem);
            
            return this;
        }
    }
}