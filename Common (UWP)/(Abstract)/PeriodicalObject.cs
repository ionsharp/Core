using Imagin.Common.Timers;
using System;
using System.Xml.Serialization;
using Windows.UI.Xaml;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class PeriodicalObject : PeriodicalObjectBase
    {
        /// <summary>
        /// 
        /// </summary>
        DispatcherTimer Timer
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets whether or not to enable notifications.
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool Enabled
        {
            get
            {
                try
                {
                    //May fail if an attempt is made to serialize property
                    return Timer?.IsEnabled ?? false;
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    Timer.Start();
                }
                else Timer.Stop();
            }
        }

        /// <summary>
        /// The period of time (in milliseconds) between notifications.
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public double Interval
        {
            get
            {
                try
                {
                    //May fail if an attempt is made to serialize property
                    return Timer?.Interval.Milliseconds ?? 0;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                Timer.Interval = System.TimeSpan.FromMilliseconds(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected PeriodicalObject() : base()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += OnNotified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifiableObject"/> class and enables (or disables) timer with given interval.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public PeriodicalObject(double interval = DefaultInterval, bool enabled = false) : this()
        {
            Interval = interval;
            Enabled = enabled;
        }

        void OnNotified(object sender, object e)
        {
            OnNotified(new ElapsedEventArgs(DateTime.UtcNow));
        }
    }
}
