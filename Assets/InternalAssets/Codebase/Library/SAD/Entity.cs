using System;
using ACS.Core.ServicesContainer;
using Codebase.Library.Addressable;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace Codebase.Library.SAD
{
    public abstract class Entity : MonoBehaviour, IInitializable<Entity>, IComponentReferenceInstance
    {
        public GameObject GameObject { get; private set; }
        public Transform Transform { get; private set; }
        public EntityComponents Components => _components;
        
        private EntityComponents _components = new DefaultEntityComponents();
        private EntityWorld _entityWorld;
        
        public virtual Entity Bootstrapp()
        {
            if(ServiceContainer.For(this).TryGetService(out _entityWorld))
                _entityWorld.AddEntity(this);
            
            Transform = transform;
            GameObject = gameObject;
            
            return this;
        }

        public void Dispose()
        {
        }
        
        public Entity BindComponents(EntityComponents components = null)
        {
            if (components == null)
                return this;
            
            _components = components.Declare(this);

            foreach (var innerComponent in _components.GetAllComponents())
            {
                if(innerComponent.Value is IDerivedEntityComponent derivedEntityComponent) 
                    derivedEntityComponent.Bootstrapp(this);
            }
            
            return this;
        }

        public bool TryGetAbstractComponent<T>(out T component)
        {
            if (_components.TryGetAbstractComponent(out T receivedComponent))
            {
                component = receivedComponent;
                return true;
            }

            component = default;
            return false;
        }
        
        public T GetAbstractComponent<T>() where T : class
        {
            if (_components.TryGetAbstractComponent(out T receivedComponent))
                return receivedComponent;

            throw new ArgumentException($"Can not get component with Name:[{typeof(T).Name}]");
        }

        public GameObject GetObject() => gameObject;
    }
}
