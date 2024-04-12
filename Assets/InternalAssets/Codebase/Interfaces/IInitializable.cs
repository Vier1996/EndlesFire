using System;

namespace InternalAssets.Codebase.Interfaces
{
    public interface IInitializable<out T> : IBootstrappable<T>, IDisposable { }
}