using System.Diagnostics;
using System.Reflection;

namespace Imagin.Common.Extensions
{
    public static class AssemblyExtensions
    {
        public static FileVersionInfo GetVersionInfo(this Assembly Assembly)
        {
            return FileVersionInfo.GetVersionInfo(Assembly.Location);
        }
    }
}
