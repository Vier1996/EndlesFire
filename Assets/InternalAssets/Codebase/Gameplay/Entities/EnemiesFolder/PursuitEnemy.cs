using System;
using Codebase.Library.SAD;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public abstract class PursuitEnemy : Enemy
    {
    }

    [Serializable]
    public class PursuitEnemyComponents : EnemyComponents
    {
        public override EntityComponents Declare(Entity abstractEntity)
        {
            base.Declare(abstractEntity);
            
            return this;
        }
    }
}