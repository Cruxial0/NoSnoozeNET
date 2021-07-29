using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoSnoozeNET.Extensions.Reflection
{
    public static class ReflectionExt
    {
        public static TPropertyType ConvertToChildObject<TInstanceType, TPropertyType>(this PropertyInfo propertyInfo, TInstanceType instance)
            where TInstanceType : class, new()
        {
            if (instance == null)
                instance = Activator.CreateInstance<TInstanceType>();

            //var p = (Child)propertyInfo;
            return (TPropertyType)propertyInfo.GetValue(instance);

        }
    }
}
