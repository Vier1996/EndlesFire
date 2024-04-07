using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Codebase.Library.Extension.Native.Types.Editor
{
    [CustomPropertyDrawer(typeof(SerializableType))]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        private SystemTypeFilterAttribute _typeFilter;
        private string[] _typeNames, _typeFullNames;

        private void Initialize()
        {
            if (_typeFullNames != null) return;

            _typeFilter = (SystemTypeFilterAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(SystemTypeFilterAttribute));

            Type[] filteredTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => _typeFilter == null ? DefaultFilter(t) : _typeFilter.Filter(t))
                .ToArray();

            _typeNames = filteredTypes.Select(t => t.ReflectedType == null ? t.Name : $"t.ReflectedType.Name + t.Name")
                .ToArray();
            _typeFullNames = filteredTypes.Select(t => t.AssemblyQualifiedName).ToArray();
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize();
            
            SerializedProperty typeIdProperty = property.FindPropertyRelative("assemblyQualifiedName");
            
            if(typeIdProperty == null)
                return;

            if (string.IsNullOrEmpty(typeIdProperty.stringValue))
            {
                typeIdProperty.stringValue = _typeFullNames.First();
                property.serializedObject.ApplyModifiedProperties();
            }

            int currentIndex = Array.IndexOf(_typeFullNames, typeIdProperty.stringValue);
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, _typeNames);

            if (selectedIndex >= 0 && selectedIndex != currentIndex)
            {
                typeIdProperty.stringValue = _typeFullNames[selectedIndex];
                property.serializedObject.ApplyModifiedProperties();
            }
        }
        
        private static bool DefaultFilter(Type type) => !type.IsAbstract && !type.IsInterface && !type.IsGenericType;
    }
}