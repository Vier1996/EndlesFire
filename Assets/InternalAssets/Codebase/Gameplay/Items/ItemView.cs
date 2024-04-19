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
        [BoxGroup("General"), SerializeField] protected LayerMask WorkingLayer;
        
        [BoxGroup("General"), SerializeField] protected bool IsSelfActivated = false;
        [BoxGroup("General"), OdinSerialize, ShowIf(nameof(IsSelfActivated))] protected ItemData InnerItemData;
        
        [BoxGroup("Trigger"), SerializeField] protected CollectorsTrigger CollectorsTrigger;
        
        [BoxGroup("Animation"), SerializeField] private Ease _animationEase;
        [BoxGroup("Animation"), SerializeField] private float _jumpToPlayerDuration = 0.5f;

        protected ItemData ItemData = null;
        
        private IDisposable _jumpToEntityDisposable;
        private Transform _selfTransform;

        private void Awake() => _selfTransform = transform;

        private void Start()
        {
            if(IsSelfActivated)
                Enable();
        }
        
        public virtual ItemView Enable()
        {
            if(IsSelfActivated)
                Setup(InnerItemData, _selfTransform.position, false);
            
            return this;
        }

        public virtual ItemView Disable()
        {
            _jumpToEntityDisposable?.Dispose();
            _selfTransform.KillTween();
            
            return this;
        }

        public ItemData GetCollectableData() => ItemData;

        public virtual ItemView Setup(ItemData data, Vector3 spawnPosition, bool withSpawnAnimation = true)
        {
            ItemData = data;
            _selfTransform.position = spawnPosition;

            return this;
        }

        protected virtual void Initialize()
        {
            CollectorsTrigger.Iteracted += JumpToCollector;
        }
        
        protected void AnimatedSpawning()
        {
            OnSpawnAnimationStart();
            
            _selfTransform.KillTween();
            _selfTransform.localScale = Vector3.zero;

            Vector2 selfPos = _selfTransform.position;
            Vector2 targetPosition = selfPos + Random.insideUnitCircle * 2f;
            Vector2 direction = targetPosition - selfPos;
            float distance = direction.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(selfPos, direction.normalized, distance, WorkingLayer);
            
            if (hit) distance = hit.distance - 0.25f;

            targetPosition = selfPos + direction.normalized * distance;

            _selfTransform
                .DOScale(Vector3.one, 0.5f)
                .SetEase(_animationEase);
            
            _selfTransform
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
            
            CollectorsTrigger.Iteracted -= JumpToCollector;

            _selfTransform.KillTween();
            
            Vector3 from = _selfTransform.position;
            float timer = 0f;
            
            _jumpToEntityDisposable?.Dispose();
            _jumpToEntityDisposable = Observable.EveryUpdate().Subscribe(_ => DispatchJumpingToPlayer());

            _selfTransform
                .DOScale(0.25f, _jumpToPlayerDuration)
                .SetEase(Ease.Linear);
            
            return;

            void DispatchJumpingToPlayer()
            {
                Vector3 to = collector.GetCollectorAnchor().position;
                float progress = Mathf.Clamp01((timer += Time.deltaTime) / _jumpToPlayerDuration);

                _selfTransform.position = CustomAnimation.GetArcPosition(from, to, height: 2f, progress);

                if (progress >= 1f)
                {
                    OnJumpToCollectorComplete();
                    DispatchCollecting(collector);
                    Despawn();
                }
            }
        }

        private void Despawn()
        {
            Disable();
            
            LeanPool.Despawn(gameObject);
        }
    }
}
