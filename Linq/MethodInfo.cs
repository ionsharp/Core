using System.Reflection;

namespace Imagin.Core.Linq;

public static class XMethodInfo
{
    public static bool IsEvent(this MethodInfo method) => (method.Name.StartsWith("add") || method.Name.StartsWith("remove")) && method.IsSpecialName;

    public static bool IsGetter(this MethodInfo method) => method.Name.StartsWith("get") && method.IsSpecialName;

    public static bool IsSetter(this MethodInfo method) => method.Name.StartsWith("set") && method.IsSpecialName;
}