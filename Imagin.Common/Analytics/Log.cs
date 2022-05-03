using System.Runtime.CompilerServices;

namespace Imagin.Common.Analytics
{
    public static class Log
    {
        public static void Write<T>(Result result, TraceLevel level = TraceLevel.Normal, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) => Get.Current<ILog>()?.Write<T>(result, level, member, line);
    }
}