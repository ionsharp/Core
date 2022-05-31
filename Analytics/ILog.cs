namespace Imagin.Core.Analytics
{
    public interface ILog
    {
        int Count { get; }

        bool Enabled { get; }

        void Clear();

        void Add(LogEntry input);

        Result Save();
    }
}