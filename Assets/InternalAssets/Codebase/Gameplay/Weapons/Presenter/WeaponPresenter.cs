using System;
using ACS.Core.ServicesContainer;
using ACS.SignalBus.SignalBus;
using Codebase.Gameplay.Sorting;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.ModelsView;
using InternalAssets.Codebase.Gameplay.Movement;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.Interfaces;
using InternalAssets.Codebase.Services._2dModels;
using InternalAssets.Codebase.Signals;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.Presenter
{
    public class WeaponPresenter : AbstractWeaponPresenter, IDerivedEntityComponent
    {
        public WeaponView CurrentView { get; private set; } = null;
        
        private IDisposable _lateUpdateDisposable;
        private ITargetable _target;
        private PlayerMovementComponent _playerMovementComponent;
        private ModelViewProvider _modelViewProvider;
        private IDetectionSystem _detectionSystem;
        private Transform _selfTransform;
        private Entity _entity;
        private ISignalBusService _signalBusService;
        
        private WeaponLookingType _lookingType = WeaponLookingType.none;
        private Vector3 _direction;
        private readonly Vector3 _defaultDirection = Vector3.one;
        private readonly Vector3 _flippedDirection = new Vector3(1f, -1f, 1f);
        
        private float _lastAngle = 0;
        private float _lerpSpeed = 0.25f;
        private bool _isEnabled = false;

        public override void Bootstrapp(Entity entity)
        {
            ServiceContainer.Core.Get(out _signalBusService);
            
            _entity = entity;
            _selfTransform = transform;

            _entity.Components.TryGetAbstractComponent(out _playerMovementComponent);
            _entity.Components.TryGetAbstractComponent(out _modelViewProvider);
            _entity.Components.TryGetAbstractComponent(out _detectionSystem);
            
            SetJoystickListening();
            
            _signalBusService.Subscribe<WeaponItemRegistred>(OnWeaponItemRegistred);
        }

        public override void Dispose()
        {
            Disable();
            
            _signalBusService.Unsubscribe<WeaponItemRegistred>(OnWeaponItemRegistred);
        }

        public override IWeaponPresenter Enable()
        {
            if(_isEnabled) return this;

            _isEnabled = true;
            
            _lateUpdateDisposable?.Dispose();
            _lateUpdateDisposable = Observable.EveryLateUpdate().Subscribe(_ => OnLateUpdate());
            
            _detectionSystem.OnTargetDetected += SetTargetListening;
            
            return this;
        }
        public override IWeaponPresenter Disable()
        {
            if(_isEnabled == false) return this;

            _isEnabled = false;
            
            CurrentView.StopFire();
            
            _detectionSystem.OnTargetDetected -= SetTargetListening;
            
            return this;
        }

        [Button]
        public override void PresentWeapon(WeaponType weaponType)
        {
            if (CurrentView != null) CurrentView.Dispose();

            WeaponConfigsContainer container = WeaponConfigsContainer.GetInstance();
            WeaponConfig config = container.GetConfig(weaponType);

            CurrentView = Instantiate(config.ViewPrefab, _selfTransform);
            CurrentView.Bootstrapp(config, _entity);
            
            DispatchWeaponUpdatedEvent(config);
            
            container.Release();
        }
        
        public void Break(DirectionType directionType)
        {
            switch (directionType)
            {
                case DirectionType.right:
                    _selfTransform.rotation = Quaternion.Euler(0, 0, 0);
                    _selfTransform.localScale = _defaultDirection;
                    break;
                
                case DirectionType.left:
                    _selfTransform.rotation = Quaternion.Euler(0, 0, 180);
                    _selfTransform.localScale = _flippedDirection;
                    break;
            }
        }
        
        private void SetTargetListening(ITargetable target)
        {
            _target = target;
            _lookingType = WeaponLookingType.marked_target;
        }

        private void OnLateUpdate()
        {
            if(_lookingType == WeaponLookingType.none && _selfTransform == null) return;
            
            float angle = 0; 
            
            switch (_lookingType)
            {
                case WeaponLookingType.joystick:
                    
                    if(_playerMovementComponent.CurrentSpeedV3 == Vector3.zero)
                        return;
                    
                    _direction = _playerMovementComponent.CurrentSpeedV3 - _selfTransform.localPosition;
                    break;
                
                case WeaponLookingType.marked_target:
                    
                    if (_target == null || _target.GetTargetTransform() == null) 
                        SetJoystickListening();

                    if(_target != null && _target.GetTargetTransform() != null)
                        _direction = _target.GetTargetTransform().position - _selfTransform.position;
                    
                    break;
            }
            
            angle = GetAngleByDirection();

            if (Math.Abs(angle - _lastAngle) > 0.0001f)
            {
                Vector3 targetAngle = new Vector3(0, 0, angle);
                
                _selfTransform.DORotate(targetAngle, 0.1f);

                if (angle > 90 || angle < -90)
                {
                    _selfTransform.localScale = _flippedDirection;
                    _modelViewProvider.ModelView.SpriteModelPresenter.SetLookingToLeft();
                }
                else
                {
                    _selfTransform.localScale = _defaultDirection;
                    _modelViewProvider.ModelView.SpriteModelPresenter.SetLookingToRight();
                }
            }

            _lastAngle = angle;

        }

        private void OnWeaponItemRegistred(WeaponItemRegistred signal) => PresentWeapon(signal.Type);

        private void SetJoystickListening()
        {
            _target = null;
            _lookingType = WeaponLookingType.joystick;
        }

        private float GetAngleByDirection() => Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        private void OnDestroy() => _selfTransform.KillTween();
    }
}
