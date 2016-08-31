using Imagin.Common;
using System;
using System.Timers;

namespace Imagin.Common
{
    [Serializable]
    ///<summary>
    /// An object with a date that periodically notifies.
    /// </summary>
    public class Entry : AbstractObject
    {
        #region Properties

        DateTime date = DateTime.UtcNow;
        public DateTime Date
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

        public int NotifyEvery
        {
            get; set;
        }

        public Timer Timer
        {
            get; private set;
        }

        #endregion

        #region Methods

        protected virtual void OnInitialized()
        {
            this.Timer = new Timer();
            this.Timer.Interval = this.NotifyEvery;
            this.Timer.Elapsed += (s, e) => OnPropertyChanged("Date");
            this.Timer.Start();
        }

        #endregion

        #region Entry

        public Entry(int NotifyEvery = 1000)
        {
            this.NotifyEvery = NotifyEvery;
            this.OnInitialized();
        }

        public Entry(DateTime Date, int NotifyEvery = 1000)
        {
            this.Date = Date;
            this.NotifyEvery = NotifyEvery;
            this.OnInitialized();
        }

        #endregion
    }
}
