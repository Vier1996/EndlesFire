using System;
using Codebase.Gameplay.Sorting;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.SAD;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Movement;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Services._2dModels;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.Presenter
{
    public class WeaponPresenter : MonoBehaviour, IWeaponPresenter, IDerivedEntityComponent
    {
        public event Action<WeaponConfig> WeaponUpdated;
        
        public WeaponView CurrentView { get; private set; } = null;
        
        private ITargetable _target;
        private PlayerMovementComponent _playerMovementComponent;
        private SpriteModelPresenter _spriteModelPresenter;
        private SortableItem _sortableItemOfOwner;
        private IDetectionSystem _detectionSystem;
        private Transform _selfTransform;
        private Entity _entity;

        private WeaponLookingType _lookingType = WeaponLookingType.none;
        private Vector3 _direction;
        
        private readonly Vector3 _defaultDirection = Vector3.one;
        private readonly Vector3 _flippedDirection = new Vector3(1f, -1f, 1f);
        
        private float _lastAngle = 0;
        private float _lerpSpeed = 0.25f;
        
        public void Bootstrapp(Entity entity)
        {
            _entity = entity;
            _selfTransform = transform;

            _playerMovementComponent = _entity.GetAbstractComponent<PlayerMovementComponent>();
            _spriteModelPresenter = _entity.GetAbstractComponent<SpriteModelPresenter>();
            _sortableItemOfOwner = _entity.GetAbstractComponent<SortableItem>();
            _detectionSystem = _entity.GetAbstractComponent<IDetectionSystem>();
            
            SetJoystickListening();
            
            _detectionSystem.OnTargetDetected += SetTargetListening;
        }

        public void Dispose() => _detectionSystem.OnTargetDetected -= SetTargetListening;

        [Button]
        public void PresentWeapon(WeaponType weaponType)
        {
            if (CurrentView != null)
            {
                _sortableItemOfOwner.RemoveRenderers(CurrentView.GetWeaponRenderers());
                CurrentView.Dispose();
            }
            
            WeaponConfigsContainer container = WeaponConfigsContainer.GetInstance();
            WeaponConfig config = container.GetConfig(weaponType);

            CurrentView = Instantiate(config.ViewPrefab, _selfTransform);
            CurrentView.Bootstrapp(config, _entity);

            _sortableItemOfOwner.AddRenderers(CurrentView.GetWeaponRenderers());
            
            WeaponUpdated?.Invoke(config);
            
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

        private void LateUpdate()
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
                    _spriteModelPresenter.SetLookingToLeft();
                }
                else
                {
                    _selfTransform.localScale = _defaultDirection;
                    _spriteModelPresenter.SetLookingToRight();
                }
            }

            _lastAngle = angle;

        }
        
        private void SetJoystickListening()
        {
            _target = null;
            _lookingType = WeaponLookingType.joystick;
        }

        private float GetAngleByDirection() => Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        private void OnDestroy() => _selfTransform.KillTween();
    }
}
