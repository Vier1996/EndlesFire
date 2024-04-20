using System;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.CustomComponents;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.Vectors;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public class EnemyTranslationComponent : MonoBehaviour, IDerivedEntityComponent
    {
        private IDisposable _translationDisposable;
        private ITargetable _targetTransform;
        private Transform _selfTransform;
        private PhysicPairComponent _physicPairComponent;
        
        private Vector3 _rawDirection;
        private Vector3 _directionVector;

        private float _maxSpeed = 5f;
        private float _triggerDistance;
        
        private Action _completeCallback = null;
        
        public void Bootstrapp(Entity entity)
        {
            _selfTransform = entity.Transform;
            
            entity.TryGetAbstractComponent(out _physicPairComponent);
        }

        public void Dispose()
        {
            _translationDisposable?.Dispose();
            _selfTransform.KillTween();
        }

        public void CancelTranslate()
        {
            _translationDisposable?.Dispose();
            _completeCallback = null;
        }

        public EnemyTranslationComponent WithParams(float maxSpeed, float triggerDistance = 1.5f)
        {
            _maxSpeed = maxSpeed * UnityEngine.Random.Range(0.7f, 1.5f);
            _triggerDistance = triggerDistance;

            return this;
        }

        public void TransferTo(ITargetable targetable, Action onComplete = null)
        {
            _translationDisposable?.Dispose();

            _targetTransform = targetable;
            _completeCallback = onComplete;
            
            _translationDisposable = Observable.EveryFixedUpdate().Subscribe(_ => TranslateFrame());
        }

        private void TranslateFrame()
        {
            if (_targetTransform == null || _targetTransform.GetTargetTransform() == null)
            {
                CancelTranslate();
                return;
            }

            _rawDirection = _targetTransform.GetTargetTransform().position - _selfTransform.position;

            if (_rawDirection == Vector3.zero)
                return;

            if (_targetTransform.GetTargetTransform().position.DistanceXY(_selfTransform.position) <= _triggerDistance)
            {
                _completeCallback?.Invoke();
                return;
            }

            _directionVector = _rawDirection.normalized * _maxSpeed * Time.fixedDeltaTime;
            _directionVector = Vector3.ClampMagnitude(_directionVector, _maxSpeed);
            _directionVector.z = 0;

            _physicPairComponent.Rigidbody.MovePosition(_selfTransform.position + _directionVector);
        }
    }
}