using System;
using Codebase.Gameplay.Sorting;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.SAD;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.ModelsView;
using InternalAssets.Codebase.Gameplay.Weapons.Configs;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.Presenter
{
    public class EnemyWeaponPresenter : AbstractWeaponPresenter, IDerivedEntityComponent, IWeaponPresenter
    {
        [field: SerializeField] public WeaponView CurrentView { get; private set; } = null;

        private IDisposable _lateUpdateDisposable;
        private Transform _selfTransform;
        private Enemy _enemy;
        private SortableItem _sortableItemOfOwner;
        private ModelViewProvider _modelViewProvider;
        private ITargetable _currentTarget;

        private WeaponLookingType _lookingType = WeaponLookingType.none;
        private Vector3 _direction;
        
        private readonly Vector3 _defaultDirection = Vector3.one;
        private readonly Vector3 _flippedDirection = new Vector3(1f, -1f, 1f);
        
        private float _lastAngle = 0;
        private bool _isEnabled = false;
        
        public void Bootstrapp(Entity entity)
        {
            _enemy = entity as Enemy;
            _selfTransform = transform;
            
            _lookingType = WeaponLookingType.marked_target;
            
            _sortableItemOfOwner = entity.GetAbstractComponent<SortableItem>();
            _modelViewProvider = entity.GetAbstractComponent<ModelViewProvider>();
        }

        public override void Dispose()
        {
            _lateUpdateDisposable?.Dispose();
            
            _isEnabled = false;

            _selfTransform.KillTween();
        }

        public override IWeaponPresenter Enable()
        {
            if(_isEnabled) return this;

            _isEnabled = true;
            
            _lateUpdateDisposable?.Dispose();
            _lateUpdateDisposable = Observable.EveryLateUpdate().Subscribe(_ => OnLateUpdate());
            
            return this;
        }

        public override IWeaponPresenter Disable()
        {
            if(_isEnabled == false) return this;

            _isEnabled = false;
            
            _lateUpdateDisposable?.Dispose();
            
            return this;
        }

        public void SetPresentersTarget(ITargetable targetable) => _currentTarget = targetable;

        [Button]
        public override void PresentWeapon(WeaponType weaponType)
        {
            if (CurrentView != null)
            {
                _sortableItemOfOwner.RemoveRenderers(CurrentView.GetWeaponRenderers());
                CurrentView.Dispose();
            }
            
            WeaponConfigsContainer container = WeaponConfigsContainer.GetInstance();
            WeaponConfig config = container.GetConfig(weaponType);

            CurrentView = Instantiate(config.ViewPrefab, _selfTransform);
            CurrentView.Bootstrapp(config, _enemy);

            _sortableItemOfOwner.AddRenderers(CurrentView.GetWeaponRenderers());
            
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
        
        private void OnLateUpdate()
        {
            if(_lookingType == WeaponLookingType.none && _selfTransform == null) return;

            float angle = 0; 
            
            switch (_lookingType)
            {
                case WeaponLookingType.marked_target:
                    
                    if (_currentTarget == null || _currentTarget.GetTargetTransform() == null) 
                        return;

                    _direction = _currentTarget.GetTargetTransform().position - _selfTransform.position;
                    
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

        private float GetAngleByDirection() => Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
    }
}