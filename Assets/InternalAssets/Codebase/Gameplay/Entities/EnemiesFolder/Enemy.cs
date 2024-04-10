using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public abstract class Enemy : Entity, IEnemy, ITargetable, IDamageReceiver
    {
        public virtual Transform GetTargetTransform() => transform;

        public virtual void EnableMarker() { }
        public virtual void DisableMarker() { }
        public virtual void ReceiveDamage(DamageArgs damageArgs) { }
    }
    
    [Serializable]
    public class EnemyComponents : EntityComponents
    {
        public override EntityComponents Declare(Entity abstractEntity)
        {
            Add(abstractEntity);
            
            return this;
        }
    }
}