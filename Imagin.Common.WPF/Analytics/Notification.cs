using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Runtime.Serialization;
using System.Timers;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Imagin.Common.Analytics
{
    [DisplayName(nameof(Notification)), XmlType(nameof(Notification))]
    [Serializable]
    public class Notification : BaseUpdate
    {
        [field: NonSerialized]
        public event EventHandler<EventArgs> Expired;

        [field: NonSerialized]
        ICommand command;
        [XmlIgnore]
        public ICommand Command
        {
            get => command;
            set => this.Change(ref command, value);
        }

        DateTime created = DateTime.Now;
        [XmlAttribute]
        public DateTime Created
        {
            get => created;
            set => this.Change(ref created, value);
        }

        TimeSpan expiration = TimeSpan.Zero;
        public TimeSpan Expiration
        {
            get => expiration;
            set => this.Change(ref expiration, value);
        }

        string icon;
        [XmlIgnore]
        public string Icon
        {
            get => icon;
            set => this.Change(ref icon, value);
        }

        bool isRead;
        [XmlAttribute]
        public bool IsRead
        {
            get => isRead;
            set => this.Change(ref isRead, value);
        }

        [XmlIgnore]
        public string Message => result?.Text;

        Result result;
        [XmlElement]
        public Result Result
        {
            get => result;
            set => this.Change(ref result, value);
        }

        string title;
        [XmlAttribute]
        public string Title
        {
            get => title;
            set => this.Change(ref title, value);
        }

        public string Type => $"{result?.Type}";

        //...

        public Notification() : base() { }

        public Notification(string title, Result result, TimeSpan expiration) : base()
        {
            Title
                = title;
            Result
                = result;
            Expiration
                = expiration;

            if (expiration > TimeSpan.Zero)
                timer.Enabled = true;
        }

        //...

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext input)
        {
            if (expiration > TimeSpan.Zero)
                Reset(1.Seconds(), true);
        }

        protected override void OnUpdate(ElapsedEventArgs e)
        {
            base.OnUpdate(e);
            this.Changed(() => Created);
            if (e.SignalTime - created >= expiration)
            {
                OnExpired();
                return;
            }
        }

        protected virtual void OnExpired() => Expired?.Invoke(this, EventArgs.Empty);

        //...

        [field: NonSerialized]
        ICommand markCommand;
        [XmlIgnore]
        public ICommand MarkCommand => markCommand ??= new RelayCommand<object>(i => IsRead = true, i => i != null);
    }
}