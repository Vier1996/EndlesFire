using System;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.Extension.Rx;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.Vectors;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public class EnemyTranslationComponent : MonoBehaviour, IDerivedEntityComponent
    {
        private IDisposable _translationDisposable;
        private IDisposable _updatePathDisposable;
        private Transform _enemyTransform;
        private ITargetable _targetTransform;
        private NavMeshPath _path;

        private Vector3 _rawDirection;
        private Vector3 _directionVector;
        private Vector3 _nextPosition;

        private int _currentPathIndex = 1;
        private float _maxSpeed = 5f;
        private float _triggerDistance;
        
        private Action _completeCallback = null;
        
        public void Bootstrapp(Entity entity)
        {
            _enemyTransform = entity.Transform;
        }

        public void Dispose()
        {
            _translationDisposable?.Dispose();
            _updatePathDisposable?.Dispose();

            _enemyTransform.KillTween();
        }

        public void CancelTranslate()
        {
            _translationDisposable?.Dispose();
            _updatePathDisposable?.Dispose();
            
            _path = null;
            _currentPathIndex = 1;
            
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

            RecalculatePath();

            _translationDisposable = Observable.EveryFixedUpdate().Subscribe(_ => TranslateFrame());
            _updatePathDisposable = RX.LoopedTimer(0f, 1f, RecalculatePath);
        }

        private void TranslateFrame()
        {
            if (_targetTransform == null)
            {
                CancelTranslate();
                return;
            }

            _nextPosition = GetNextPoint();
            _rawDirection = _nextPosition - _enemyTransform.position;

            if (_rawDirection == Vector3.zero)
                return;

            if (_targetTransform.GetTargetTransform().position.DistanceXY(_enemyTransform.position) <= _triggerDistance)
            {
                _completeCallback?.Invoke();
                return;
            }

            _directionVector = _rawDirection.normalized * _maxSpeed * Time.fixedDeltaTime;
            _directionVector = Vector3.ClampMagnitude(_directionVector, _maxSpeed);
            _directionVector.z = 0;

            _enemyTransform.position += _directionVector;
        }

        private Vector3 GetNextPoint()
        {
            for (int i = _currentPathIndex; i < _path.corners.Length; i++)
            {
                if (_path.corners[i].DistanceXY(_enemyTransform.position) > 0.1f)
                {
                    _currentPathIndex = i;

                    return _path.corners[i];
                }
            }

            return _enemyTransform.position;
        }

        private void RecalculatePath()
        {
            _path = new NavMeshPath();
            _currentPathIndex = 1;

            NavMesh
                .CalculatePath(
                    _enemyTransform.position,
                    GetRandomPointInCircle(_targetTransform.GetTargetTransform().position,
                        0.01f),
                    NavMesh.AllAreas, 
                    _path);
        }

        Vector3 GetRandomPointInCircle(Vector3 centerPoint, float radius)
        {
            float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            float phi = UnityEngine.Random.Range(0f, Mathf.PI);

            float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float z = radius * Mathf.Cos(phi);

            Vector3 randomPoint = centerPoint + new Vector3(x, 1f, z);
            return randomPoint;
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (_path?.corners == null)
                return;

            for (int i = 0; i < _path.corners.Length - 1; i++)
                UnityEngine.Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red);
        }
#endif
    }
}