using System;
using System.ComponentModel;
using System.Timers;
using System.Xml.Serialization;

namespace Imagin.Common
{
    [Serializable]
    public class NotifiableObject : AbstractObject, INotifiable
    {
        [Browsable(false)]
        [XmlIgnore]
        public Timer NotifyTimer
        {
            get; private set;
        }

        protected virtual void OnInitialized()
        {
            this.NotifyTimer = new Timer();
            this.NotifyTimer.Interval = 1000.0;
            this.NotifyTimer.Elapsed += (s, e) => this.OnNotified(e);
        }

        public virtual void OnNotified(ElapsedEventArgs e)
        {
        }

        public NotifiableObject() : base()
        {
            this.OnInitialized();
        }

        public NotifiableObject(bool IsNotifyEnabled) : base()
        {
            this.OnInitialized();
            this.NotifyTimer.Enabled = IsNotifyEnabled;
        }

        public NotifiableObject(double Interval, bool IsNotifyEnabled = false) : base()
        {
            this.OnInitialized();
            this.NotifyTimer.Interval = Interval;
            this.NotifyTimer.Enabled = IsNotifyEnabled;
        }
    }
}
