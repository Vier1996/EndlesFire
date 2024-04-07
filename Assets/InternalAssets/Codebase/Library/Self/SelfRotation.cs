using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Codebase.Library.Self
{
    public class SelfRotation : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationVector = new(0, 1, 0);
        [SerializeField] private bool _randomSpeed;

        [ShowIf(nameof(_randomSpeed)), SerializeField] private float _minSpeed;
        [ShowIf(nameof(_randomSpeed)), SerializeField] private float _maxSpeed;
        
        public float Speed;
        private bool IsPaused = false;

        private void Start()
        {
            if (_randomSpeed)
                Speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);
        }

        public void SetAsPaused() => IsPaused = true;

        public void SetAsNonPaused() => IsPaused = false;

        private void Update()
        {
            if(IsPaused)
                return;
            
            transform.Rotate(_rotationVector, Speed * Time.deltaTime);
        }
    }
}
