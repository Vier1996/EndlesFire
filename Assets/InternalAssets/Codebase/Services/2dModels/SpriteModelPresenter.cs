using UnityEngine;

namespace InternalAssets.Codebase.Services._2dModels
{
    public class SpriteModelPresenter : MonoBehaviour
    {
        private readonly Vector3 _rightLookingVector = Vector3.one;
        private readonly Vector3 _leftLookingVector = new Vector3(-1, 1, 1);

        private Transform _selfTransform;
        
        public virtual void Awake() => _selfTransform = transform;

        public virtual void SetLookingToRight() => _selfTransform.localScale = _rightLookingVector;

        public virtual void SetLookingToLeft() => _selfTransform.localScale = _leftLookingVector;
    }
}
