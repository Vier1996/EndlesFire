using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Triggers
{
    public class DamageController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Entity entity) == false) return;
            
            entity.Transform.position = Vector3.one;
            entity.GameObject.SetActive(false);
            
            if (entity.TryGetAbstractComponent(out IDamageReceiver damageReceiver))
                damageReceiver.ReceiveDamage(new DamageArgs());
        }
    }
}