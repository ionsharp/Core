using System;
using System.Timers;
using System.Xml.Serialization;
using Imagin.Common;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PeriodicalObject : PeriodicalObjectBase
    {
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public new event Timers.ElapsedEventHandler Notified;

        Timer timer;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool Enabled
        {
            get => timer.Enabled;
            set => timer.Enabled = value;
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public double Interval
        {
            get => timer.Interval;
            set => timer.Interval = value;
        }

        /// <summary>
        /// 
        /// </summary>
        protected PeriodicalObject() : base()
        {
            timer = new Timer();
            timer.Elapsed += OnNotified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public PeriodicalObject(double interval = DefaultInterval, bool enabled = false) : this()
        {
            Interval = interval;
            Enabled = enabled;
        }

        void OnNotified(object sender, ElapsedEventArgs e)
        {
            OnNotified(new Timers.ElapsedEventArgs(e.SignalTime));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNotified(Timers.ElapsedEventArgs e)
        {
            Notified?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnUnmanagedDisposed()
        {
            base.OnUnmanagedDisposed();

            timer.Enabled = false;
            timer.Elapsed -= OnNotified;
            timer.Dispose();
        }
    }
}
