using System;
using System.Collections.Generic;
using Codebase.Library.Extension.Native.Types;
using InternalAssets.Codebase.Gameplay.Triggers;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Damage
{
    public class DamagableTrigger : MonoBehaviour
    {
        public event Action<DamageReceiveTrigger> ReceiverFound;

        private Type _receiverType;
        private List<Type> _listeningTypes = new();
        
        public void SetListeningTypes(List<Type> types)
        {
            _listeningTypes.Clear();
            _listeningTypes = types;
        }
        
        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ITrigger iTrigger) && iTrigger is DamageReceiveTrigger damageReceiveTrigger)
            {
                if(IsValid())
                    ReceiverFound?.Invoke(damageReceiveTrigger);
            }
        }

        private bool IsValid()
        {
            _receiverType ??= typeof(IDamageReceiver);
            
            foreach (Type currentType in _listeningTypes)
            {
                if (currentType.InheritsOrImplements(_receiverType))
                    return true;
            }

            return false;
        }
    }
}
