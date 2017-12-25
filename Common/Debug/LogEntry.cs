using System;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// 
    /// </summary>
    public class LogEntry : BindableObject, IEntry, ILogEntry
    {
        DateTime date = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        [Searchable]
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                SetValue(ref date, value, () => Date);
            }
        }

        string message = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        [Searchable]
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                SetValue(ref message, value, () => Message);
            }
        }

        object source = default(object);
        /// <summary>
        /// 
        /// </summary>
        [Searchable]
        public object Source
        {
            get
            {
                return source;
            }
            set
            {
                SetValue(ref source, value, () => Source);
            }
        }

        LogEntryType type = LogEntryType.Message;
        /// <summary>
        /// 
        /// </summary>
        public LogEntryType Type
        {
            get
            {
                return type;
            }
            set
            {
                SetValue(ref type, value, () => Type);
            }
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
