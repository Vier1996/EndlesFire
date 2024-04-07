using System;
using Codebase.Gameplay.Sorting;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.Movement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Dodge
{
    public class DodgeComponent : MonoBehaviour, IDerivedEntityComponent
    {
        [BoxGroup("Effect"), SerializeField] private ParticleSystem _horizontalDodge;
        [BoxGroup("Effect"), SerializeField] private ParticleSystem _verticalDodge;
        [BoxGroup("EffectPosition"), SerializeField] private Transform _upPosition;
        [BoxGroup("EffectPosition"), SerializeField] private Transform _rightPosition;
        [BoxGroup("EffectPosition"), SerializeField] private Transform _downPosition;
        [BoxGroup("EffectPosition"), SerializeField] private Transform _leftPosition;
        
        private Entity _caster;
        private SortableItem _playerSortableComponent;
        private PlayerMovementComponent _playerMovement;
        
        public void Bootstrapp(Entity entity)
        {
            _caster = entity;
            
            entity.TryGetAbstractComponent(out _playerMovement);
            entity.TryGetAbstractComponent(out _playerSortableComponent);
            
            _playerMovement.OnDodgeStart += OnDodgeStart;
        }

        public void Dispose()
        {
            _playerMovement.OnDodgeStart -= OnDodgeStart;
        }

        private void OnDodgeStart()
        {
            Vector3 speed = _playerMovement.Direction;
            ParticleSystem effect = null;
            
            if (Math.Abs(speed.x) > Math.Abs(speed.y))
            {
                if (speed.x > 0)
                    effect = CreateDodgeEffect(_leftPosition, _horizontalDodge);
                else
                {
                    effect = CreateDodgeEffect(_rightPosition, _horizontalDodge);
                    effect.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                if (speed.y > 0)
                    effect = CreateDodgeEffect(_downPosition, _verticalDodge);
                else
                {
                    effect = CreateDodgeEffect(_upPosition, _verticalDodge);
                    effect.transform.localScale = new Vector3(1, -1, 1);
                }
            }
            
            if(effect != null)
                effect.Play();
        }

        private ParticleSystem CreateDodgeEffect(Transform targetTransform, ParticleSystem prefab)
        {
            ParticleSystem effect = Instantiate(prefab, _caster.Transform);
            Renderer particleRendererModule = effect.GetComponent<Renderer>();

            particleRendererModule.sortingOrder = _playerSortableComponent.LastOrder - 15;
            effect.transform.position = targetTransform.position;

            return effect;
        }
    }
}
