namespace Imagin.Common.Tracing
{
    public class LogEntry : Entry
    {
        #region Properties

        string source = string.Empty;
        public string Host
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                OnPropertyChanged("Host");
            }
        }

        string message = string.Empty;
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

        LogEntryStatus status = LogEntryStatus.Info;
        public LogEntryStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        WarningLevel warningLevel = WarningLevel.Moderate;
        public WarningLevel WarningLevel
        {
            get
            {
                return warningLevel;
            }
            set
            {
                warningLevel = value;
                OnPropertyChanged("WarningLevel");
            }
        }

        #endregion

        #region LogEntry

        public LogEntry() : base()
        {
        }

        public LogEntry(string Source, string Message) : base()
        {
            this.Host = Source;
            this.Message = Message;
        }

        public LogEntry(LogEntryStatus Status, string Source, string Message) : base()
        {
            this.Status = Status;
            this.Host = Source;
            this.Message = Message;
        }

        public LogEntry(WarningLevel WarningLevel, LogEntryStatus Status, string Source, string Message) : base()
        {
            this.WarningLevel = WarningLevel;
            this.Status = Status;
            this.Host = Source;
            this.Message = Message;
        }

        #endregion
    }
}
