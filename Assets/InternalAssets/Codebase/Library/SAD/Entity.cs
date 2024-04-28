using System;
using ACS.Core.ServicesContainer;
using Codebase.Library.Addressable;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Codebase.Library.SAD
{
    public abstract class Entity : MonoBehaviour, IEntity, IInitializable<Entity>, IComponentReferenceInstance
    {
        [BoxGroup("Entity"), SerializeField] private bool _selfActivated = false;
        
        public GameObject GameObject { get; private set; }
        public Transform Transform { get; private set; }
        public EntityComponents Components => _components;
        public bool IsBootstapped { get; private set; }

        private EntityComponents _components = new DefaultEntityComponents();
        private EntityWorld _entityWorld;

        private void Start()
        {
            if (_selfActivated)
                Bootstrapp();
        }

        public virtual Entity Bootstrapp()
        {
            if (IsBootstapped) return this;

            IsBootstapped = true;
            
            if(ServiceContainer.For(this).TryGetService(out _entityWorld))
                _entityWorld.AddEntity(this);
            
            Transform = transform;
            GameObject = gameObject;
            
            foreach (var innerComponent in _components.GetAllComponents())
            {
                if(innerComponent.Value is IDerivedEntityComponent derivedEntityComponent) 
                    derivedEntityComponent.Bootstrapp(this);
            }
            
            return this;
        }

        [Button("Kill entity")]
        private void OnDestroy() => Dispose();

        public void Dispose()
        {
            _entityWorld?.RemoveEntity(this);

            foreach (var innerComponent in _components.GetAllComponents())
            {
                if(innerComponent.Value is IDerivedEntityComponent derivedEntityComponent) 
                    derivedEntityComponent.Dispose();
            }
        }
        
        public Entity BindComponents(EntityComponents components = null)
        {
            if (components == null)
                return this;
            
            _components = components.Declare(this);
            
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

            Debug.LogError($"Can not get component with Name:[{typeof(T).Name}]");
            return null;
        }

        public GameObject GetObject() => GameObject;
    }
}
