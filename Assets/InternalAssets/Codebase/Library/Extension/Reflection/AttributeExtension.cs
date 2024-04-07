using System;
using System.Linq;

namespace Codebase.Library.Extension.Reflection
{
    public static class AttributeExtension
    {
        public static bool TryGetAttribute<T>(this Type type, out T attribute) where T : Attribute
        {
            T attr = (T)Attribute.GetCustomAttribute(type, typeof(T));

            if (attr == null)
            {
                attribute = null;
                return false;
            }
            
            attribute = attr;
            return true;
        }
        
        public static bool TryGetAttribute<T>(this Enum @enum, out T attribute) where T : Attribute
        {
            Type type = @enum.GetType();
            string name = Enum.GetName(type, @enum);

            T attr = type
                .GetField(name)
                .GetCustomAttributes(false)
                .OfType<T>()
                .SingleOrDefault();
            
            if (attr == null)
            {
                attribute = null;
                return false;
            }
            
            attribute = attr;
            return true;
        }
    }
}
