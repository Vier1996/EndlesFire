using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        public void ToIdle()
        {
            _animator.SetBool(PlayerAnimationProperties.MoveProperty, false);
            _animator.SetBool(PlayerAnimationProperties.IdleProperty, true);
        }

        public void ToMovement()
        {
            _animator.SetBool(PlayerAnimationProperties.MoveProperty, true);
            _animator.SetBool(PlayerAnimationProperties.IdleProperty, false);
        }
        
        public void ToDodge()
        {
            _animator.SetBool(PlayerAnimationProperties.MoveProperty, false);
            _animator.SetBool(PlayerAnimationProperties.IdleProperty, false);
            _animator.SetTrigger(PlayerAnimationProperties.Dodge);
        }
    }
}