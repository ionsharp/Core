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

        Timer Timer
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool Enabled
        {
            get
            {
                return Timer.Enabled;
            }
            set
            {
                Timer.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public double Interval
        {
            get
            {
                return Timer.Interval;
            }
            set
            {
                Timer.Interval = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected PeriodicalObject() : base()
        {
            Timer = new Timer();
            Timer.Elapsed += OnNotified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public PeriodicalObject(double interval = DefaultInterval, bool enabled = false) : this()
        {
            Timer = new Timer();
            Timer.Elapsed += OnNotified;

            Interval = interval;
            Enabled = enabled;
        }

        void OnNotified(object sender, System.Timers.ElapsedEventArgs e)
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

            Timer.Enabled = false;
            Timer.Elapsed -= OnNotified;
            Timer.Dispose();
        }
    }
}
