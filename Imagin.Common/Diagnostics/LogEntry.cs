using System;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// 
    /// </summary>
    public class LogEntry : ObjectBase, IEntry, ILogEntry
    {
        DateTime date = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        [Searchable]
        public DateTime Date
        {
            get => date;
            set => Property.Set(this, ref date, value, () => Date);
        }

        string message = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        [Searchable]
        public string Message
        {
            get => message;
            set => Property.Set(this, ref message, value, () => Message);
        }

        object source = default(object);
        /// <summary>
        /// 
        /// </summary>
        [Searchable]
        public object Source
        {
            get => source;
            set => Property.Set(this, ref source, value, () => Source);
        }

        LogEntryType type = LogEntryType.Message;
        /// <summary>
        /// 
        /// </summary>
        public LogEntryType Type
        {
            get => type;
            set => Property.Set(this, ref type, value, () => Type);
        }

        /// <summary>
        /// 
        /// </summary>
        public LogEntry() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Type"></param>
        public LogEntry(string Message, LogEntryType Type = LogEntryType.Message) : this(Message, null, Type)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="source"></param>
        /// <param name="type"></param>
        public LogEntry(string message, object source, LogEntryType type = LogEntryType.Message) : this()
        {
            Message = message;
            Source = source;
            Type = type;
        }
    }
}
