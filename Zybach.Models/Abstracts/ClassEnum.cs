using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zybach.Models.Abstracts;

public abstract class ClassEnum<T>
{
    public static List<T> All
    {
        get
        {
            var classType = typeof(T);
            var enumValues = classType
                            .GetFields(BindingFlags.Public | BindingFlags.Static)
                            .Where(x => x.GetValue(null)!.GetType() == classType)
                            .Select(x => (T)x.GetValue(null))
                            .Where(x => x != null);

            return enumValues.ToList();
        }
    }
}