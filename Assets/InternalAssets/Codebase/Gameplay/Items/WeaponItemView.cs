using System;
using ACS.Core.ServicesContainer;
using ACS.Dialog.Dialogs;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using InternalAssets.Codebase.Dialogs.ChangeWeaponDialog;
using InternalAssets.Codebase.Gameplay.Parents;
using InternalAssets.Codebase.Gameplay.ProgressTimers;
using InternalAssets.Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Interfaces;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Items
{
    public class WeaponItemView : ItemView
    {
        [BoxGroup("WeaponView Components"), SerializeField] private ProgressTimerView _progressTimerView;
        [BoxGroup("WeaponView Model"), SerializeField] private Transform _weaponModelTransform;
        [BoxGroup("WeaponView Model"), SerializeField] private float _baseScale = 1f;
        [BoxGroup("WeaponView Model"), SerializeField] private float _pulseScale = 1.5f;
        [BoxGroup("WeaponView Model"), SerializeField] private float _pulseDuration = 0.75f;
        [BoxGroup("WeaponView Effects"), SerializeField] private ParticleSystem _idleParticle;
        [BoxGroup("WeaponView Effects"), SerializeField] private SortableParticle _removeEffect;

        private WeaponData _weaponData;
        private ICollector _currentCollector;
        
        public override ItemView Setup(IItemData data, Vector3 spawnPosition, bool withSpawnAnimation = true)
        {
            base.Setup(data, spawnPosition, withSpawnAnimation);

            if (data is not WeaponData weaponData)
                throw new InvalidCastException("Can not convert IItemData to WeaponData");

            _weaponData = weaponData;
            
            _progressTimerView.HideTimer(instant: true);
                
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
            CollectorsTrigger.InteractionStarted += OnInteractionStarted;
            CollectorsTrigger.InteractionFinished += OnInteractionFinished;
            _progressTimerView.ProgressComplete += OnCatchProgressComplete;

            _progressTimerView.SetUnlockTime(_weaponData.UnlockTime);
            _progressTimerView.Enable();

            PulseWeaponModel();
            
            return base.Enable();
        }

        public override ItemView Disable()
        {
            CollectorsTrigger.InteractionStarted -= OnInteractionStarted;
            CollectorsTrigger.InteractionFinished -= OnInteractionFinished;
            _progressTimerView.ProgressComplete -= OnCatchProgressComplete;

            _idleParticle.Stop();
            _progressTimerView.Disable();
            
            BreakPulse();
            
            return base.Disable();
        }

        protected override void Initialize()
        {
            CollectorsTrigger.EnableInteraction();
            
            _idleParticle.Play();
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

        private void PulseWeaponModel()
        {
            BreakPulse();
            
            _weaponModelTransform
                .DOScale(_pulseScale, _pulseDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void BreakPulse()
        {
            _weaponModelTransform.KillTween();
            _weaponModelTransform.localScale = Vector3.one * _baseScale;
        }
        
        private void OnInteractionStarted(ICollector collector)
        {
            _currentCollector = collector;
            
            _progressTimerView.BeginInteract();
        }
        
        private void OnInteractionFinished(ICollector collector)
        {
            _currentCollector = null;
            
            _progressTimerView.StopInteract();
        }
        
        private void OnCatchProgressComplete()
        {
            ServiceContainer.Core.Get(out IDialogService dialogService);
            
            dialogService.CallDialog(typeof(ChangeWeaponDialog), new ChangeWeaponDialogArgs()
            {
                ChooseCallback = ExecuteWeaponChange
            });
        }

        [Button]
        private void ExecuteWeaponChange(bool status)
        {
            if(status)
                DispatchCollecting(_currentCollector);
            
            Disable();
            Despawn();
        }

        public override void Despawn()
        {
            ServiceContainer.ForCurrentScene().Get(out SceneAssetParentsContainer sceneAssetParentsContainer);
            
            SortableParticle particle = LeanPool.Spawn(_removeEffect, SelfTransform);
            particle.transform.localPosition = Vector3.zero;
            particle.transform.SetParent(sceneAssetParentsContainer.VfxParent);
            particle.Play();
            
            base.Despawn();
        }
    }
}
