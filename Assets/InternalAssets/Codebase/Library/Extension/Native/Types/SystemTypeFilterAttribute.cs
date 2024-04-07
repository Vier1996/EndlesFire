using System;
using UnityEngine;

namespace Codebase.Library.Extension.Native.Types
{
    public class SystemTypeFilterAttribute : PropertyAttribute {
        public Func<Type, bool> Filter { get; }
        
        public SystemTypeFilterAttribute(Type filterType) {
            Filter = type => !type.IsAbstract &&
                             !type.IsInterface &&
                             !type.IsGenericType &&
                             type.InheritsOrImplements(filterType);
        }
    }
}