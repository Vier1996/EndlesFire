using System;
using Codebase.Library.SAD;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public class SpearedEnemy : PursuitEnemy
    {
        [SerializeField] private SpearedEnemyComponents _components;
        
        public override Entity Bootstrapp()
        {
            return base.Bootstrapp().BindComponents(_components);
        }
    }
    
    [Serializable]
    public class SpearedEnemyComponents : PursuitEnemyComponents
    {
        public override EntityComponents Declare(Entity abstractEntity)
        {
            base.Declare(abstractEntity);
            
            return this;
        }
    }
}