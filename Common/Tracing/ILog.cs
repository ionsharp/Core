namespace Imagin.Common.Tracing
{
    public interface ILog
    {
        void Write(string Source, string Message);

        void Write(LogEntryStatus Status, string Source, string Message);

        void Write(WarningLevel WarningLevel, LogEntryStatus Status, string Source, string Message);
    }
}
