using System;
using Cinemachine;
using Codebase.Library.Extension.Rx;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Services.Camera
{
    public class CameraCenterer : MonoBehaviour
    {
        private const float DeadZoneWidth = 0.25f;
        private const float DeadZoneHeight = 0.125f;
        
        [BoxGroup("Params"), SerializeField, Range(0f, 1f)] public float maxCameraOffset = 0.5f;
        [BoxGroup("Params"), SerializeField, Range(0.5f, 10f)] public float smoothTime = 0.2f;
        [BoxGroup("Params"), SerializeField] private Transform targetAnchor;
        [BoxGroup("Params"), SerializeField] private CinemachineVirtualCamera virtualCamera;
        
        private float _smoothProgress;
        private float _elapsedTime;
        private float _offsetProgress;
        
        private Vector3 _center;
        private Vector3 _lastPosition;
        private Vector3 _lastAnchorPosition;
        
        private IDisposable _initializeDisposable;
        private ITargetable _currentTarget;
        private IDetectionSystem _detectionSystem;
        private Entity _targetEntity;
        private CinemachineFramingTransposer _cinemachineTransposer;
        
        private bool _initialized;
        
        [Button]
        public void Initialize(Entity entity)
        {
            _targetEntity = entity;
            _detectionSystem = _targetEntity.GetAbstractComponent<IDetectionSystem>();
            _cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            
            SetDeadZoneValues(0, 0);

            _initializeDisposable = RX.Delay(0.25f, Setup);
        }
        
        private void OnDestroy()
        {
            _initializeDisposable?.Dispose();
            
            _initialized = false;
            _detectionSystem.OnTargetDetected -= UpdateTarget;
        }

        private void Setup()
        {
            _lastPosition = _targetEntity.Transform.position;
            _lastAnchorPosition = _lastPosition;
            targetAnchor.position = _lastPosition;
            
            _detectionSystem.OnTargetDetected += UpdateTarget;
            _smoothProgress = 0;
            
            SetDeadZoneValues(DeadZoneWidth, DeadZoneHeight);
            
            _initialized = true;
        }
        
        private void UpdateTarget(ITargetable newTarget)
        {
            if (newTarget != _currentTarget) _elapsedTime = 0f;
            
            _currentTarget = newTarget;
            _lastAnchorPosition = targetAnchor.position;
        }

        private void LateUpdate()
        {
            if (_currentTarget != null)
            {
                if (_smoothProgress < smoothTime)
                    _smoothProgress += Time.fixedDeltaTime;
            }
            else
            {
                if (_smoothProgress > 0)
                    _smoothProgress -= Time.fixedDeltaTime;
            }

            if (_elapsedTime < smoothTime)
                _elapsedTime += Time.fixedDeltaTime;
            
            if(!_initialized || _targetEntity == null) return;

            NormalizeAnchor();
        }

        private void NormalizeAnchor()
        {
            _offsetProgress = GetOffsetValue();

            if (_currentTarget != null) 
                _lastPosition = _currentTarget.GetTargetTransform().position;

            _center = Vector3.Lerp(_lastPosition, _targetEntity.Transform.position, _offsetProgress);
            _offsetProgress = Mathf.Clamp01(_elapsedTime / smoothTime);
            
            targetAnchor.position = Vector3.Lerp(_lastAnchorPosition, _center, _offsetProgress);
        }

        private float GetOffsetValue() => 1f - (maxCameraOffset * (_smoothProgress / smoothTime));

        private void SetDeadZoneValues(float width, float height)
        {
            _cinemachineTransposer.m_DeadZoneWidth = width;
            _cinemachineTransposer.m_DeadZoneHeight = height;
        }
    }
}
