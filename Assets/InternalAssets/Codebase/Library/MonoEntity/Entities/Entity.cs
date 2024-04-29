using ACS.Core.ServicesContainer;
using ACS.SignalBus.SignalBus;
using InternalAssets.Codebase.Library.MonoEntity.EntityComponent;
using InternalAssets.Codebase.Library.MonoEntity.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity.Signals;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace InternalAssets.Codebase.Library.MonoEntity.Entities
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        [BoxGroup("Entity"), SerializeField] private bool _selfActivated = false;
        
        public GameObject GameObject { get; private set; }
        public Transform Transform { get; private set; }
        public EntityComponents Components { get; private set; } = new DefaultEntityComponents();
        public bool IsBootstrapped { get; private set; }

        private ISignalBusService _signalBusService;
        
        private void Start()
        {
            if (_selfActivated)
                Bootstrapp();
        }

        public virtual Entity Bootstrapp(EntityComponents components = null)
        {
            if (IsBootstrapped) return this;

            ServiceContainer.Core.Get(out _signalBusService);
            
            IsBootstrapped = true;
            Transform = transform;
            GameObject = gameObject;
            
            if (components != null)
                Components = components.Declare(this);
            
            Components
                .GetAllComponents()
                .ForEach(cmp =>
                {
                    if (cmp.Value is IDerivedEntityComponent derivedEntityComponent)
                        derivedEntityComponent.Bootstrapp(this);
                });
            
            _signalBusService.Fire(new EntityCreatedSignal(this));
            
            return this;
        }

        public void Dispose()
        {
            Components
                .GetAllComponents()
                .ForEach(cmp =>
                {
                    if (cmp.Value is IDerivedEntityComponent derivedEntityComponent)
                        derivedEntityComponent.Dispose();
                });
            
            _signalBusService.Fire(new EntityDestroyedSignal(this));
        }

        [Button("Kill entity")]
        private void OnDestroy() => Dispose();
    }
}
