using System;
using System.Collections.Generic;

namespace Codebase.Library.SAD
{
    [Serializable]
    public abstract class EntityComponents
    {
        private readonly Dictionary<string, object> _entityComponents = new();

        public abstract EntityComponents Declare(Entity abstractEntity);
        
        public bool TryGetAbstractComponent<T>(out T component)
        { 
            bool containsComponent = _entityComponents.TryGetValue(typeof(T).Name, out object innerComponent);
            
            component = containsComponent ? (T)innerComponent : default;

            return containsComponent;
        }

        public Dictionary<string, object> GetAllComponents() => _entityComponents;

        protected EntityComponents Add(Type type, object component)
        {
            _entityComponents.TryAdd(type.Name, component);
            
            return this;
        }

        protected EntityComponents Add(object component)
        {
            _entityComponents.TryAdd(component.GetType().Name, component);

            return this;
        }
    }
}