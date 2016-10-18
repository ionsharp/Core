using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Imagin.Common.Scheduling
{
    [Serializable]
    public class RepeatOptions : AbstractObject
    {
        #region Properties

        [field: NonSerialized]
        public event EventHandler<EventArgs> OptionsChanged;

        string data;
        public string Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
                OnPropertyChanged("Data");
            }
        }

        ObservableCollection<int> days;
        public ObservableCollection<int> Days
        {
            get
            {
                return this.days;
            }
            set
            {
                this.days = value;
                OnPropertyChanged("Days");
            }
        }

        ObservableCollection<string> months;
        public ObservableCollection<string> Months
        {
            get
            {
                return this.months;
            }
            set
            {
                this.months = value;
                OnPropertyChanged("Months");
            }
        }

        MonthlyRecurrence monthlyRecurrence = MonthlyRecurrence.Each;
        /// <summary>
        /// Specification for how something should occur monthly.
        /// </summary>
        public MonthlyRecurrence MonthlyRecurrence
        {
            get
            {
                return this.monthlyRecurrence;
            }
            set
            {
                this.monthlyRecurrence = value;
                OnPropertyChanged("MonthlyRecurrence");
                this.OnOptionsChanged();
            }
        }

        Recurrence recurrence;
        /// <summary>
        /// How often something should repeat.
        /// </summary>
        public Recurrence Recurrence
        {
            get
            {
                return this.recurrence;
            }
            set
            {
                this.recurrence = value;
                OnPropertyChanged("Recurrence");
                this.OnOptionsChanged();
            }
        }

        int recurrenceValue = 1;
        /// <summary>
        /// Number of units to observe based on recurrence type (e.g., if daily, every {4} days).
        /// </summary>
        public int RecurrenceValue
        {
            get
            {
                return this.recurrenceValue;
            }
            set
            {
                this.recurrenceValue = value;
                OnPropertyChanged("RecurrenceValue");
                this.OnOptionsChanged();
            }
        }

        bool repeats = false;
        /// <summary>
        /// Indicates whether or not something should repeat.
        /// </summary>
        public bool Repeats
        {
            get
            {
                return this.repeats;
            }
            set
            {
                this.repeats = value;
                OnPropertyChanged("Repeats");
            }
        }

        ObservableCollection<DayOfWeek> weekDays;
        public ObservableCollection<DayOfWeek> WeekDays
        {
            get
            {
                return this.weekDays;
            }
            set
            {
                this.weekDays = value;
                OnPropertyChanged("WeekDays");
            }
        }

        WeeklyRecurrence weeklyRecurrence = WeeklyRecurrence.First;
        /// <summary>
        /// Specification for how something should occur weekly.
        /// </summary>
        public WeeklyRecurrence WeeklyRecurrence
        {
            get
            {
                return this.weeklyRecurrence;
            }
            set
            {
                this.weeklyRecurrence = value;
                OnPropertyChanged("WeeklyRecurrence");
                this.OnOptionsChanged();
            }
        }

        #endregion

        #region Methods

        async Task<string> BeginGetData(Recurrence Recurrence, int RecurrenceValue)
        {
            if (!this.Repeats)
                return "No Repeat";

            var Result = new StringBuilder();

            var Days = this.Days;
            var Months = this.Months;
            var WeekDays = this.WeekDays;

            var MonthlyRecurrence = this.MonthlyRecurrence;
            var WeeklyRecurrence = this.WeeklyRecurrence;

            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                Result.Append(string.Format("Every {0} {1}", RecurrenceValue, this.PrintLabel(Recurrence, RecurrenceValue)));
                switch (Recurrence)
                {
                    case Recurrence.Weekly:
                        if (WeekDays.Count > 0)
                        {
                            Result.Append(" on ");
                            Result.Append(this.PrintWeekDays(WeekDays));
                        }
                        break;
                    case Recurrence.Monthly:
                        if (MonthlyRecurrence == MonthlyRecurrence.Each)
                        {
                            Result.Append(" on the ");
                            if (Days.Count > 0)
                                Result.Append(this.PrintDays(Days));
                        }
                        else if (MonthlyRecurrence == MonthlyRecurrence.On)
                            Result.Append(string.Format(" on the {0} ", WeeklyRecurrence.ToString().ToLower()));
                        if (WeekDays.Count > 0)
                            Result.Append(this.PrintWeekDays(WeekDays));
                        break;
                    case Recurrence.Yearly:
                        if (WeekDays.Count > 0)
                        {
                            Result.Append(string.Format(" on the {0} ", WeeklyRecurrence.ToString().ToLower()));
                            Result.Append(this.PrintWeekDays(WeekDays));
                        }
                        if (Months.Count > 0)
                        {
                            Result.Append(this.WeekDays.Count > 0 ? " of " : " in ");
                            Result.Append(this.PrintMonths(Months));
                        }
                        break;
                }
            }));

            return Result.ToString();
        }

        string PrintDays(ObservableCollection<int> Days)
        {
            switch (Days.Count)
            {
                case 1:
                    return Days[0].ToString();
                case 2:
                    return string.Concat(Days[0], " and ", Days[1]);
                default:
                    var Result = new StringBuilder();
                    var SortedDays = Days.OrderBy(x => x).ToList<int>();
                    for (int i = 0; i < Days.Count; i++)
                    {
                        if (i == Days.Count - 1)
                            Result.Append("and ");
                        Result.Append(SortedDays[i].ToString());
                        if (i < Days.Count - 1)
                            Result.Append(", ");
                    }
                    return Result.ToString();
            }
        }

        string PrintLabel(Recurrence Recurrence, int RecurrenceValue)
        {
            var Append = RecurrenceValue != 1 ? "s" : "";
            switch (Recurrence)
            {
                case Recurrence.Daily:
                    return "day" + Append;
                case Recurrence.Weekly:
                    return "week" + Append;
                case Recurrence.Monthly:
                    return "month" + Append;
                case Recurrence.Yearly:
                    return "year" + Append;
            }
            return string.Empty;
        }

        string PrintMonths(ObservableCollection<string> Months)
        {
            switch (Months.Count)
            {
                case 1:
                    return this.Months[0].ToLower();
                case 2:
                    return string.Format("{0} and {1}", Months[0].ToLower(), Months[1].ToLower());
                default:
                    var Result = new StringBuilder();
                    var SortedMonths = Months.Cast<string>().OrderBy(x => x).ToList<string>();
                    for (int i = 0; i < SortedMonths.Count; i++)
                        Result.Append(i == (SortedMonths.Count - 1) ? "and " + SortedMonths[i].ToLower() : SortedMonths[i].ToLower() + ", ");
                    return Result.ToString();
            }
        }

        string PrintWeekDays(ObservableCollection<DayOfWeek> WeekDays)
        {
            switch (WeekDays.Count)
            {
                case 1:
                    return WeekDays[0].ToString().ToLower();
                case 2:
                    return string.Format("{0} and {1}", WeekDays[0].ToString().ToLower(), WeekDays[1].ToString().ToLower());
                default:
                    var Result = new StringBuilder();
                    var SortedWeekDays = WeekDays.Cast<DayOfWeek>().OrderBy(x => (int)x).ToList<DayOfWeek>();
                    for (int i = 0; i < SortedWeekDays.Count; i++)
                    {
                        if (i == SortedWeekDays.Count - 1)
                            Result.Append("and ");
                        Result.Append(SortedWeekDays[i].ToString().ToLower().Substring(0, 3));
                        if (i < SortedWeekDays.Count - 1)
                            Result.Append(", ");
                    }
                    return Result.ToString();
            }
        }

        protected virtual async void OnOptionsChanged()
        {
            if (this.OptionsChanged != null)
                this.OptionsChanged(this, new EventArgs());

            this.Data = await this.BeginGetData(this.Recurrence, this.RecurrenceValue);
        }

        public override string ToString()
        {
            return this.Recurrence.ToString();
        }

        #endregion

        #region RepeatOptions

        protected RepeatOptions() : base()
        {
        }

        public RepeatOptions(Recurrence Recurrence) : base()
        {
            this.Days = new ObservableCollection<int>();
            this.Months = new ObservableCollection<string>();
            this.WeekDays = new ObservableCollection<DayOfWeek>();

            this.Recurrence = Recurrence;
        }

        #endregion
    }
}
