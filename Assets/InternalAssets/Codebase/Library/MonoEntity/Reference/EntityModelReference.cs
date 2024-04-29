using System;
using Codebase.Library.Addressable;
using InternalAssets.Codebase.Library.MonoEntity.Entities;

namespace InternalAssets.Codebase.Library.MonoEntity.Reference
{
    [Serializable]
    public class EntityModelReference : AssetComponentReference<Entity>
    {
        public EntityModelReference(string guid) : base(guid) { }
    }
}