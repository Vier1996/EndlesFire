using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Items
{
    public class WeaponItemData : ItemView
    {
        private WeaponData _weaponData;
        
        public override ItemView Setup(IItemData data, Vector3 spawnPosition, bool withSpawnAnimation = true)
        {
            base.Setup(data, spawnPosition, withSpawnAnimation);
            
            if (withSpawnAnimation)
            {
                AnimatedSpawning();
                return this;
            }
            
            Initialize();
            
            return this;
        }
        
        public override ItemView Enable()
        {
            CollectorsTrigger.Iteracted += OnInteracted;
            
            return base.Enable();
        }
        
        public override ItemView Disable()
        {
            CollectorsTrigger.Iteracted -= OnInteracted;
            
            return base.Disable();
        }

        protected override void Initialize()
        {
            CollectorsTrigger.EnableInteraction();
        }

        protected override void OnSpawnAnimationStart()
        {
            CollectorsTrigger.DisableInteraction();
        }

        protected override void OnSpawnAnimationComplete()
        {
            Initialize();
        }
        
        protected override void DispatchCollecting(ICollector collector) => collector.Collect(this);

        private void OnInteracted(ICollector collector)
        {
            DispatchCollecting(collector);
            Despawn();
        }
    }
}
