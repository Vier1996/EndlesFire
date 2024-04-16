using System;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Services.Animation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Items
{
    public class ResourceItemView : ItemView
    {
        [BoxGroup("General"), SerializeField] private SpriteSheetAnimator _modelRenderer;
        [BoxGroup("Effects"), SerializeField] private ParticleSystem _trailParticle;
        [BoxGroup("Effects"), SerializeField] private ParticleSystem _idleParticle;
        
        public override ItemView Bootstrapp()
        {
            _modelRenderer.Bootstrapp();
            
            return base.Bootstrapp();
        }
        
        public override void Dispose()
        {
            _modelRenderer.Dispose();
            
            _trailParticle.Stop();
            _idleParticle.Stop();
            
            base.Dispose();
        }

        public override void Setup(ItemData data, Vector3 spawnPosition, bool withSpawnAnimation = true)
        {
            base.Setup(data, spawnPosition, withSpawnAnimation);
            
            if (withSpawnAnimation)
            {
                AnimatedSpawning();
                return;
            }
            
            Initialize();
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            
            _idleParticle.Play();
        }

        protected override void OnSpawnAnimationStart()
        {
            CollectorsTrigger.DisableInteraction();

            _trailParticle.Play();
        }

        protected override void OnSpawnAnimationComplete()
        {
            CollectorsTrigger.EnableInteraction();
            
            Initialize();
        }

        protected override void OnJumpToCollectorStart()
        {
            _idleParticle.Stop();
        }

        protected override void OnJumpToCollectorComplete()
        {
            _trailParticle.Play();
        }
        
        protected override void DispatchCollecting(ICollector collector) => collector.Collect(this);
    }
}