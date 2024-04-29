using System;
using ACS.Core.ServicesContainer;
using Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Factory.Vfx;
using InternalAssets.Codebase.Gameplay.Movement;
using InternalAssets.Codebase.Gameplay.Parents;
using InternalAssets.Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.Interfaces;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Dodge
{
    public class DodgeComponent : MonoBehaviour, IDerivedEntityComponent
    {
        [BoxGroup("EffectPosition"), SerializeField] private Transform _upPosition;
        [BoxGroup("EffectPosition"), SerializeField] private Transform _rightPosition;
        [BoxGroup("EffectPosition"), SerializeField] private Transform _downPosition;
        [BoxGroup("EffectPosition"), SerializeField] private Transform _leftPosition;
        
        private IWeaponPresenter _weaponPresenter;
        private PlayerMovementComponent _playerMovement;
        private SceneAssetParentsContainer _sceneAssetParentsContainer;
        private VfxFactoryProvider _vfxFactoryProvider;
        
        public void Bootstrapp(Entity entity)
        {
            ServiceContainer.Global.Get(out _vfxFactoryProvider);
            ServiceContainer.ForCurrentScene().Get(out _sceneAssetParentsContainer);
            
            entity.Components.TryGetAbstractComponent(out _playerMovement);
            entity.Components.TryGetAbstractComponent(out _weaponPresenter);
            
            _playerMovement.OnDodgeStart += OnDodgeStart;
            _playerMovement.OnDodgeStart += DisableWeaponPresenter;
            _playerMovement.OnDodgeEnd += EnableWeaponPresenter;
        }

        public void Dispose()
        {
            _playerMovement.OnDodgeStart -= OnDodgeStart;
            _playerMovement.OnDodgeStart -= DisableWeaponPresenter;
            _playerMovement.OnDodgeEnd -= EnableWeaponPresenter;
        }

        private void OnDodgeStart()
        {
            Vector3 speed = _playerMovement.Direction;
            SortableParticle effect = null;
            
            if (Math.Abs(speed.x) > Math.Abs(speed.y))
            {
                if (speed.x > 0)
                    effect = CreateDodgeEffect(VfxType.horizontal_dodge, _leftPosition);
                else
                {
                    effect = CreateDodgeEffect(VfxType.horizontal_dodge, _rightPosition);
                    effect.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                if (speed.y > 0)
                    effect = CreateDodgeEffect(VfxType.vertical_dodge, _downPosition);
                else
                {
                    effect = CreateDodgeEffect(VfxType.vertical_dodge, _upPosition);
                    effect.transform.localScale = new Vector3(1, -1, 1);
                }
            }
            
            if(effect != null)
                effect.Play();
        }

        private void EnableWeaponPresenter() => _weaponPresenter.ShowPresenter();
        
        private void DisableWeaponPresenter() => _weaponPresenter.HidePresenter();

        private SortableParticle CreateDodgeEffect(VfxType vfxType, Transform targetTransform)
        {
            SortableParticle effect = _vfxFactoryProvider.SpawnFactoryItem(
                    vfxType, 
                    targetTransform.position, 
                    Quaternion.identity,
                    _sceneAssetParentsContainer.VfxParent);
            
            effect.Play();
            
            return effect;
        }
    }
}
