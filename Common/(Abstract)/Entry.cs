using System;
using System.ComponentModel;
using System.Timers;
using System.Xml.Serialization;

namespace Imagin.Common
{
    ///<summary>
    /// An object with a date that periodically notifies.
    /// </summary>
    [Serializable]
    public class Entry : NotifiableObject, IEntry
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        protected DateTime date = DateTime.UtcNow;
        /// <summary>
        /// Gets or sets the date of the entry.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return date;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Entry() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public Entry(double interval, bool enabled = false) : base(interval, enabled)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public Entry(DateTime date, double interval = DefaultInterval, bool enabled = false) : base(interval, enabled)
        {
            this.date = date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNotified(ElapsedEventArgs e)
        {
            base.OnNotified(e);
            OnPropertyChanged("Date");
        }
    }
}
