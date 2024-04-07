using System;
using System.Linq;

namespace Codebase.Library.Extension.Native.Types
{
    public static class SystemTypeExtensions
    {
        public static bool InheritsOrImplements(this Type type, Type baseType)
        {
            type = type.ResolveGenericType();
            baseType = baseType.ResolveGenericType();

            while (type != typeof(object))
            {
                if (baseType == type || type.HasInterface(baseType)) return true;

                type = type.BaseType.ResolveGenericType();
                if (type == null) return false;
            }

            return false;
        }

        private static Type ResolveGenericType(this Type type)
        {
            if (type is not { IsGenericType: true }) return type;

            var genericType = type.GetGenericTypeDefinition();
            return genericType != type ? genericType : type;
        }

        private static bool HasInterface(this Type type, Type interfaceType) =>
            type.GetInterfaces().Any(i => i.ResolveGenericType() == interfaceType);
    }
}