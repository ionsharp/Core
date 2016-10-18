namespace Imagin.Common.Logging
{
    public class LogEntry : Entry
    {
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

        string host = string.Empty;
        public string Host
        {
            get
            {
                return host;
            }
            set
            {
                host = value;
                OnPropertyChanged("Host");
            }
        }

        public LogEntry() : base()
        {
        }

        public LogEntry(string Host, string Message) : base()
        {
            this.Host = Host;
            this.Message = Message;
        }

        public LogEntry(LogEntryStatus Status, string Host, string Message) : base()
        {
            this.Status = Status;
            this.Host = Host;
            this.Message = Message;
        }
    }
}
