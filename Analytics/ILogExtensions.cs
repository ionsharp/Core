using System.Runtime.CompilerServices;

namespace Imagin.Core.Analytics
{
    public static class ILogExtensions
    {
        public static void Write<T>(this ILog input, Result result, ResultLevel level = ResultLevel.Normal, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            if (input.Enabled)
            {
                var logEntry = new LogEntry(level, typeof(T).Name, result, member, line);
                input.Add(logEntry);
            }
        }
    }
}