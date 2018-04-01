using System;
using System.Timers;
using System.Xml.Serialization;

namespace Imagin.Common
{
    ///<summary>
    /// Specifies a <see cref="PeriodicalObject"/> with a <see cref="DateTime"/> that notifies periodically (implements <see cref="IEntry"/>).
    /// </summary>
    [Serializable]
    public class Entry : PeriodicalObject, IEntry
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        protected readonly DateTime _date;
        /// <summary>
        /// Gets or sets the date of the entry.
        /// </summary>
        public DateTime Date => _date;

        /// <summary>
        /// 
        /// </summary>
        public Entry() : this(null, null, false) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public Entry(DateTime? date, TimeSpan? interval = null, bool enabled = false) : base(interval, enabled) => _date = date ?? DateTime.UtcNow;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElapsed(ElapsedEventArgs e)
        {
            base.OnElapsed(e);
            Property.Raise(this, () => Date);
        }
    }
}
