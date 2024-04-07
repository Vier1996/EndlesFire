using System;
using Codebase.Library.Addressable;

namespace Codebase.Library.SAD
{
    [Serializable]
    public class EntityModelReference : AssetComponentReference<Entity>
    {
        public EntityModelReference(string guid) : base(guid)
        {
        }
    }
}