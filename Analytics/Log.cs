using System.Runtime.CompilerServices;

namespace Imagin.Core.Analytics
{
    public static class Log
    {
        public static void Write<T>(Result result, ResultLevel level = ResultLevel.Normal, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) => Get.Current<ILog>()?.Write<T>(result, level, member, line);
    }
}