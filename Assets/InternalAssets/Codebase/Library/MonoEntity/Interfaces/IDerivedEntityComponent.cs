using System;
using InternalAssets.Codebase.Library.MonoEntity.Entities;

namespace InternalAssets.Codebase.Library.MonoEntity.Interfaces
{
    public interface IDerivedEntityComponent : IDisposable
    {
        public void Bootstrapp(Entity entity);
    }
}