using System;
using UnityEngine;

namespace Codebase.Library.SAD
{
    public interface IDerivedEntityComponent : IDisposable
    {
        public virtual void Bootstrapp(Entity entity) { }
    }
}