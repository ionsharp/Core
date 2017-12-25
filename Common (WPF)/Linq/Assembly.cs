using System.Diagnostics;
using System.Reflection;

namespace Imagin.Common.Linq
{
    public static class AssemblyExtensions
    {
        public static FileVersionInfo GetVersionInfo(this Assembly Assembly)
        {
            return FileVersionInfo.GetVersionInfo(Assembly.Location);
        }
    }
}
