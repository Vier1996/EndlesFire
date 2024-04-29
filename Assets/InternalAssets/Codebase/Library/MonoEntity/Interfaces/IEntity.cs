using System;
using InternalAssets.Codebase.Library.Addressable;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.EntityComponent;
using UnityEngine;

namespace InternalAssets.Codebase.Library.MonoEntity.Interfaces
{
    public interface IEntity : IComponentReferenceInstance, IDisposable
    {
        public Transform Transform { get; }
        public EntityComponents Components { get; }
        public bool IsBootstrapped { get; }
        
        public Entity Bootstrapp(EntityComponents components = null);
    }
}