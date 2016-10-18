using System;
using System.ComponentModel;
using System.Timers;
using System.Xml.Serialization;

namespace Imagin.Common.Scheduling
{
    [Serializable]
    public class ScheduledEvent : NamedObject
    {
        #region Properties

        [field:NonSerialized]
        public event EventHandler<EventArgs> Executed;

        string description = string.Empty;
        [Category("General")]
        [Description("The description for the scheduled event.")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        int interval = 1000;
        /// <summary>
        /// Length of time in milliseconds between queries.
        /// </summary>
        [Browsable(false)]
        public int Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
                OnPropertyChanged("Interval");
                this.Timer.Interval = value;
            }
        }

        DateTime? lastExecution = null;
        public DateTime? LastExecution
        {
            get
            {
                return this.lastExecution;
            }
            set
            {
                this.lastExecution = value;
                OnPropertyChanged("LastExecution");
            }
        }

        DateTime startDate = default(DateTime);
        [Category("Date")]
        [Description("The date the scheduled event should start.")]
        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        Nullable<DateTime> endDate = null;
        [Category("Date")]
        [Description("The date the scheduled event should end.")]
        public Nullable<DateTime> EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        RepeatOptions repeatOptions = default(RepeatOptions);
        [Category("Frequency")]
        [Description("Properties that describe how often the scheduled event should repeat.")]
        public RepeatOptions RepeatOptions
        {
            get
            {
                return repeatOptions;
            }
            set
            {
                repeatOptions = value;
                OnPropertyChanged("RepeatOptions");
            }
        }

        Timer timer = new Timer();
        [XmlIgnore]
        public Timer Timer
        {
            get
            {
                return timer;
            }
            set
            {
                timer = value;
                OnPropertyChanged("Timer");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether or not event should execute based on repeat options.
        /// </summary>
        protected virtual void OnQuery()
        {
            var Now = DateTime.UtcNow;
            var Temp = this.StartDate;
            switch (this.RepeatOptions.Recurrence)
            {
                case Recurrence.Daily:
                    while (Temp < Now)
                        Temp = Temp.AddDays(this.RepeatOptions.RecurrenceValue);
                    break;
                case Recurrence.Weekly:
                    while (Temp < Now)
                        Temp = Temp.AddDays(this.RepeatOptions.RecurrenceValue * 7);
                    break;
                case Recurrence.Monthly:
                    while (Temp < Now)
                        Temp = Temp.AddMonths(this.RepeatOptions.RecurrenceValue);
                    break;
                case Recurrence.Yearly:
                    while (Temp < Now)
                        Temp = Temp.AddYears(this.RepeatOptions.RecurrenceValue);
                    break;
            }

            if (Temp <= Now && Temp != this.LastExecution)
            {
                this.OnExecuted();
                this.LastExecution = Temp;
            }
        }

        protected virtual void OnExecuted()
        {
            if (this.Executed != null)
                this.Executed(this, new EventArgs());
        }

        protected virtual void OnInitialized()
        {
            //this.Timer.Elapsed += (s, e) => this.OnQuery();
            this.Timer.Interval = this.Interval;
            this.Timer.Start();
        }

        #endregion

        #region ScheduledEvent

        protected ScheduledEvent() : base()
        {
            this.OnInitialized();
        }

        public ScheduledEvent(string Name) : base(Name)
        {
            this.OnInitialized();

            this.StartDate = DateTime.UtcNow;
        }

        public ScheduledEvent(string Name, string Description, DateTime StartDate, Nullable<DateTime> EndDate = null, RepeatOptions RepeatOptions = null) : base(Name)
        {
            this.OnInitialized();

            this.Description = Description;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.RepeatOptions = RepeatOptions ?? new RepeatOptions(Recurrence.None);
        }

        #endregion
    }
}
