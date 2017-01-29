namespace Imagin.Common.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public class LogEntry : Entry, ILogEntry
    {
        string message = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        object source = default(object);
        /// <summary>
        /// 
        /// </summary>
        public object Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                OnPropertyChanged("Source");
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
                type = value;
                OnPropertyChanged("Type");
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
        public LogEntry(string message, object source, LogEntryType type = LogEntryType.Message) : base()
        {
            Message = message;
            Source = source;
            Type = type;
        }
    }
}
