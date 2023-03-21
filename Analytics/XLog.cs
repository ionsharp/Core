using System.Runtime.CompilerServices;

namespace Imagin.Core.Analytics;

public static class XLog
{
    public static void Write<T>(this ILog input, Result result, ResultLevel level = ResultLevel.Normal, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
    {
        if (input.Enabled)
            input.Add(new LogEntry(level, typeof(T).Name, result, member, line));
    }
}