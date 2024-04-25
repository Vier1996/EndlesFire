using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Items
{
    public class GameItemView : ItemView
    {
        [BoxGroup("General"), SerializeField] private ParticleSystem _orbParticle;
        
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
            _orbParticle.Play();
            
            return base.Enable();
        }
        
        public override ItemView Disable()
        {
            _orbParticle.Stop();
            
            return base.Disable();
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            
            _orbParticle.Play();
        }

        protected override void OnSpawnAnimationStart()
        {
            CollectorsTrigger.DisableInteraction();
        }

        protected override void OnSpawnAnimationComplete()
        {
            CollectorsTrigger.EnableInteraction();
            
            Initialize();
        }
        
        protected override void OnJumpToCollectorComplete() { }
        
        protected override void DispatchCollecting(ICollector collector) => collector.Collect(this);
    }
}