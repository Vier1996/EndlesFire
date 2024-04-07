using System;
using UnityEngine;

namespace Codebase.Library.Extension.Native.Types
{
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        [field: SerializeField] public string AssemblyQualifiedName { get; private set; } = string.Empty;
        public Type Type { get; private set; }

        void ISerializationCallbackReceiver.OnBeforeSerialize() => 
            AssemblyQualifiedName = GetType().AssemblyQualifiedName ?? AssemblyQualifiedName;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (TryGetType(AssemblyQualifiedName, out Type type) == false)
            {
                Debug.LogError($"Type {AssemblyQualifiedName} not found");
                
                return;
            }

            Type = type;
        }

        private static bool TryGetType(string typeString, out Type type)
        {
            type = Type.GetType(typeString);
            return Type.GetType(typeString) != null || !string.IsNullOrEmpty(typeString);
        }
    }
}