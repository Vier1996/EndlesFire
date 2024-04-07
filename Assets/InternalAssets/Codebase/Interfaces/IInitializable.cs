using System;

namespace InternalAssets.Codebase.Interfaces
{
    public interface IInitializable<T> : IBootstrappable<T>, IDisposable { }
}