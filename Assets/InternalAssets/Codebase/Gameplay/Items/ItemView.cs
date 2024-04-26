using System;
using Codebase.Library.Animation;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Triggers;
using InternalAssets.Codebase.Interfaces;
using Lean.Pool;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InternalAssets.Codebase.Gameplay.Items
{
    public abstract class ItemView : SerializedMonoBehaviour, IRecycledClass<ItemView>, ICollectable
    {
        public event Action<ItemView> Despawned;

        [field: SerializeField, BoxGroup("General")] public Transform SelfTransform { get; private set; }

        [BoxGroup("General"), SerializeField] protected LayerMask WorkingLayer;
        [BoxGroup("General"), SerializeField] protected bool IsSelfActivated = false;
        [BoxGroup("General"), OdinSerialize, ShowIf(nameof(IsSelfActivated))] protected IItemData ItemData;
        
        [BoxGroup("Trigger"), SerializeField] protected CollectorsTrigger CollectorsTrigger;
        
        [BoxGroup("Animation"), SerializeField] private Ease _animationEase;
        [BoxGroup("Animation"), SerializeField] private float _jumpToPlayerDuration = 0.5f;

        private IDisposable _jumpToEntityDisposable;
        
        private void Start()
        {
            if (IsSelfActivated)
            {
                Setup(ItemData, SelfTransform.position, false);
                Enable();
            }
        }
        
        public virtual ItemView Setup(IItemData data, Vector3 spawnPosition, bool withSpawnAnimation = true)
        {
            ItemData = data;
            SelfTransform.position = spawnPosition;

            return this;
        }
        
        public virtual ItemView Enable() => this;

        public virtual ItemView Disable()
        {
            _jumpToEntityDisposable?.Dispose();
            SelfTransform.KillTween();
            
            return this;
        }
        
        public IItemData GetCollectableData() => ItemData;
        
        public virtual void Despawn()
        {
            Disable();
            
            LeanPool.Despawn(gameObject);
            
            Despawned?.Invoke(this);
        }

        protected virtual void Initialize()
        {
            CollectorsTrigger.InteractionStarted += JumpToCollector;
        }
        
        protected void AnimatedSpawning()
        {
            OnSpawnAnimationStart();
            
            SelfTransform.KillTween();
            SelfTransform.localScale = Vector3.zero;

            Vector2 selfPos = SelfTransform.position;
            Vector2 targetPosition = selfPos + Random.insideUnitCircle * 2f;
            Vector2 direction = targetPosition - selfPos;
            float distance = direction.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(selfPos, direction.normalized, distance, WorkingLayer);
            
            if (hit) distance = hit.distance - 0.25f;

            targetPosition = selfPos + direction.normalized * distance;

            SelfTransform
                .DOScale(Vector3.one, 0.5f)
                .SetEase(_animationEase);
            
            SelfTransform
                .DOJump(targetPosition, 1f, 1, 0.5f)
                .SetEase(_animationEase)
                .OnComplete(OnSpawnAnimationComplete);
        }
        
        protected virtual void OnSpawnAnimationStart() { }
        protected virtual void OnSpawnAnimationComplete() { }
        protected virtual void OnJumpToCollectorStart() { }
        protected virtual void OnJumpToCollectorComplete() { }
        protected abstract void DispatchCollecting(ICollector collector);

        private void JumpToCollector(ICollector collector)
        {
            OnJumpToCollectorStart();
            
            CollectorsTrigger.InteractionStarted -= JumpToCollector;

            SelfTransform.KillTween();
            
            Vector3 from = SelfTransform.position;
            float timer = 0f;
            
            _jumpToEntityDisposable?.Dispose();
            _jumpToEntityDisposable = Observable.EveryUpdate().Subscribe(_ => DispatchJumpingToPlayer());

            SelfTransform
                .DOScale(0.25f, _jumpToPlayerDuration)
                .SetEase(Ease.Linear);
            
            return;

            void DispatchJumpingToPlayer()
            {
                Vector3 to = collector.GetCollectorAnchor().position;
                float progress = Mathf.Clamp01((timer += Time.deltaTime) / _jumpToPlayerDuration);

                SelfTransform.position = CustomAnimation.GetArcPosition(from, to, height: 2f, progress);

                if (progress >= 1f)
                {
                    OnJumpToCollectorComplete();
                    DispatchCollecting(collector);
                    Despawn();
                }
            }
        }
    }
}
