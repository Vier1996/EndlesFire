using System;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Triggers
{
    public class CollectorsTrigger : MonoBehaviour
    {
        public event Action<ICollector> Iteracted;

        [SerializeField] private Collider2D _interactionCollider;

        public void EnableInteraction() => _interactionCollider.enabled = true;
        
        public void DisableInteraction() => _interactionCollider.enabled = false;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ICollector interactor))
                Iteracted?.Invoke(interactor);
        }
    }
}