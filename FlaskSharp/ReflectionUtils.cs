using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskSharp
{
    internal static class ReflectionUtils
    {
        public static object? GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }
    }
}
