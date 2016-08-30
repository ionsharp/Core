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

        public Entry()
        {
            this.OnInitialized();
        }

        public Entry(int NotifyEvery)
        {
            this.NotifyEvery = NotifyEvery;
            this.OnInitialized();
        }

        #endregion
    }
}
