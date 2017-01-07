using Imagin.Common.Attributes;
using System;
using System.ComponentModel;
using System.Timers;
using System.Xml.Serialization;

namespace Imagin.Common
{
    [Serializable]
    ///<summary>
    /// An object with a date that periodically notifies.
    /// </summary>
    public class Entry : AbstractObject
    {
        #region Properties

        Timer Timer
        {
            get; set;
        }

        [XmlIgnore]
        protected DateTime date = DateTime.UtcNow;
        [Category("General")]
        [Description("The entry date.")]
        public virtual DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public int NotificationInterval
        {
            get; set;
        }

        #endregion

        #region Methods

        protected virtual void OnInitialized()
        {
            this.Timer = new Timer();
            this.Timer.Interval = this.NotificationInterval;
            this.Timer.Elapsed += (s, e) => OnPropertyChanged("Date");
            this.Timer.Start();
        }

        #endregion

        #region Entry

        public Entry(int NotifyEvery = 1000)
        {
            this.NotificationInterval = NotifyEvery;
            this.OnInitialized();
        }

        public Entry(DateTime Date, int NotifyEvery = 1000)
        {
            this.Date = Date;
            this.NotificationInterval = NotifyEvery;
            this.OnInitialized();
        }

        #endregion
    }
}
