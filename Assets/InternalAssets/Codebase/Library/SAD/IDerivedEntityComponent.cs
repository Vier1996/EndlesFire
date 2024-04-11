using System;
using UnityEngine;

namespace Codebase.Library.SAD
{
    public interface IDerivedEntityComponent : IDisposable
    {
        public void Bootstrapp(Entity entity) { }
    }
}