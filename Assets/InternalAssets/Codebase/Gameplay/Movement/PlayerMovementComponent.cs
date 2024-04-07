using System;
using System.Collections;
using Codebase.Gameplay.Sorting;
using Codebase.Library.SAD;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Behavior.Player;
using InternalAssets.Codebase.Gameplay.Behavior.Player.States;
using InternalAssets.Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Services.Input;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Movement
{
    public class PlayerMovementComponent : MonoBehaviour, IDerivedEntityComponent
    {
        public Action OnDodgeStart;
        public Action OnDodgeEnd;

        [BoxGroup("Footstep"), SerializeField] private SortableParticle _footstepEffect; 
        [BoxGroup("Footstep"), SerializeField] private Transform _footstepSpawnPosition;
        [BoxGroup("Footstep"), SerializeField] private float _footstepTriggerDistance = 0.5f;
        [BoxGroup("Dodge"), SerializeField] private ParticleSystem _dodgeEnabledParticle;
        [BoxGroup("Dodge"), SerializeField] private SpriteRenderer _dodgeIndicator;
        [BoxGroup("DodgeParams"), SerializeField] private float timeToDodge = 200f;
        [BoxGroup("DodgeParams"), SerializeField] private float dodgeDist = 2f;
        [BoxGroup("DodgeParams"), SerializeField, Range(0f, 5f)] private float _dodgeJoystickDeviation;
        [BoxGroup("Physic"), SerializeField] private CircleCollider2D _movementCollider;
        [BoxGroup("Sorting"), SerializeField] private SortableItem _playerSortableComponent;

        public bool IsInitialized { get; private set; } = false;
        public Vector3 Direction => _lastDirection;
        
        private PlayerBehaviorMachine _playerBehaviorMachine;
        private Rigidbody2D _movableRigidbody;
        private IInputService _inputService;
        private WaitForSeconds _dodgeDelay = new(0.5f);
        private Sequence _dodgeIndicatorSequence;
        
        private readonly float _distMultiplierPerFixedUpdate = 2.5f;
        private float _speedProperty;
        private bool _isDodgeOnCoolDown = false;
        private float _currentFootstepDistance;
        
        private TimeSpan _tickOnUp = TimeSpan.Zero;
        private TimeSpan _tickOnDown = TimeSpan.Zero;
        private Vector2 _lastDirection = Vector2.zero;
        private Vector2 _speed = Vector2.zero;
        private Vector3 _lastPosition = Vector3.zero;
        
        private bool _isCanMove = true;
        
        public void Bootstrapp(Entity playerEntity)
        {
            _inputService = new MobilePlayerInputService();

            _speedProperty = 2f;//speedStat.Value;
            
            _playerBehaviorMachine = playerEntity.GetAbstractComponent<PlayerBehaviorMachine>();
            _movableRigidbody = playerEntity.GetAbstractComponent<Rigidbody2D>();
            
            _currentFootstepDistance = _footstepTriggerDistance;
            
            IsInitialized = true;
        }

        public void Dispose()
        {
            IsInitialized = false;
        }

        public void EnableMovement()
        {
            _movementCollider.enabled = true;
        }
        
        public void DisableMovement()
        {
            _movementCollider.enabled = false;
            _playerBehaviorMachine.Machine.SwitchBehavior<IdlePlayerBehavior>();
        }

        public void SwitchOnUpdate() => IsInitialized = true;
        
        public void SwitchOffUpdate() => IsInitialized = false;
        
        private void FixedUpdate()
        {
            if (!IsInitialized || !_isCanMove) return;
            
            Vector2 axis = _inputService.Axis.normalized;
            
            _speed = axis * _speedProperty;
            
            if (axis != Vector2.zero)
            {
                if (_tickOnUp == TimeSpan.Zero && _tickOnDown == TimeSpan.Zero)
                    _tickOnDown = new TimeSpan(DateTime.UtcNow.Ticks);

                _lastDirection = axis;
            }
            else if (_tickOnUp == TimeSpan.Zero && _tickOnDown != TimeSpan.Zero)
                    _tickOnUp = new TimeSpan(DateTime.UtcNow.Ticks);
            
            if (CheckOnDodge())
            {
                Dodge();
                return;
            }

            if (_speed != Vector2.zero) UpdatePositionTick();
            else TickWithoutUpdate();
        }

        private bool CheckOnDodge()
        {
            void RefreshTick()
            {
                _tickOnUp = TimeSpan.Zero;
                _tickOnDown = TimeSpan.Zero;
            }
            
            if (_isDodgeOnCoolDown)
            {
                RefreshTick();
                return false;
            }

            if (_tickOnDown == TimeSpan.Zero || _tickOnUp == TimeSpan.Zero)  return false;
            
            var dif = _tickOnUp.TotalMilliseconds - _tickOnDown.TotalMilliseconds;
            float joystickDistance = _inputService.DragDistance;
            
            RefreshTick();
            
            if (dif <= timeToDodge && dif > 0  && joystickDistance >= _dodgeJoystickDeviation) 
                return true;
            
            return false;
        }

        private void Dodge()
        {
            if (_isDodgeOnCoolDown) return;
            
            OnDodgeStart?.Invoke();
            
            IsInitialized = false;
            
            StartCoroutine(EnableDodgeCooldown());

            _playerBehaviorMachine.Machine.SwitchBehavior<PlayerDodgeBehaviour>();
            
            DodgeMovement(() =>
            {
                OnDodgeEnd?.Invoke();
                IsInitialized = true;
            });
        }

        private async void DodgeMovement(Action callBack)
        {
            float duration = 1000;
            
            while (duration > 0)
            {
                if (!_isCanMove) break;
                
                await UniTask.WaitForFixedUpdate();

                if (_movableRigidbody == null)
                    break;
                
                duration -= 50f;
                var point = _movableRigidbody.position + _lastDirection.normalized * (dodgeDist * _distMultiplierPerFixedUpdate * Time.deltaTime);
                _movableRigidbody.MovePosition(point);
            }
            
            callBack?.Invoke();
        }
        
        private void UpdatePositionTick()
        {
            _movableRigidbody.velocity = Vector2.zero;
            _movableRigidbody.MovePosition(_movableRigidbody.position + _speed * Time.fixedDeltaTime);

            _playerBehaviorMachine.Machine.SwitchBehavior<MovementPlayerBehavior>();

            float stepDistance = Vector2.Distance(_lastPosition, _movableRigidbody.position);
            
            if ((_currentFootstepDistance -= stepDistance) <= 0) 
                ApplyFootstep();

            _lastPosition = _movableRigidbody.position;
        }

        private void TickWithoutUpdate()
        {
            _playerBehaviorMachine.Machine.SwitchBehavior<IdlePlayerBehavior>();
        }

        private void ApplyFootstep()
        {
            _currentFootstepDistance = _footstepTriggerDistance;

            SortableParticle sortableParticle = LeanPool.Spawn(_footstepEffect, _footstepSpawnPosition.position, Quaternion.identity, null);
            sortableParticle.ChangeSortingOrder(_playerSortableComponent.LastOrder - 15);
            sortableParticle.Play();
        }

        private IEnumerator EnableDodgeCooldown()
        {
            _dodgeIndicator.DOKill();
            _dodgeIndicatorSequence?.Kill();
            _dodgeIndicatorSequence = DOTween.Sequence();
            _dodgeIndicator.DOFade(0, 0.15f);
            _isDodgeOnCoolDown = true;
            
            yield return _dodgeDelay;
            
            _isDodgeOnCoolDown = false;
            _dodgeEnabledParticle.Stop();
            _dodgeEnabledParticle.Play();
            
            if(_dodgeIndicatorSequence != null && _dodgeIndicatorSequence.IsActive())
                _dodgeIndicatorSequence.Append(_dodgeIndicator.DOFade(1, 0.15f).SetLoops(7, LoopType.Yoyo));
        }
    }
}
