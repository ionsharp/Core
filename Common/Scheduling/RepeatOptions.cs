using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.ComponentModel;
using Imagin.Common.Extensions;
using Imagin.Common.Globalization;
using System;
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

        public static string DefaultData
        {
            get
            {
                return LocalizationProvider.GetLocalizedValue<string>("Never");
            }
        }

        string data = DefaultData;
        public string Data
        {
            get
            {
                return this.data.IsNullOrEmpty() ? DefaultData : this.data;
            }
            set
            {
                this.data = value;
                OnPropertyChanged("Data");
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

        #endregion

        #region Methods

        async Task<string> BeginGetData(Recurrence Recurrence, int RecurrenceValue)
        {
            if (!this.Repeats || this.Recurrence == Recurrence.None)
                return DefaultData;

            var Result = new StringBuilder();
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                Result.Append(string.Format("Every {0}{1}", RecurrenceValue == 1 ? string.Empty : string.Format("{0} ", RecurrenceValue), PrintLabel(Recurrence, RecurrenceValue)));
            }));

            return Result.ToString();
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

        protected virtual void OnInitialized()
        {
            OnOptionsChanged();
        }

        protected virtual async void OnOptionsChanged()
        {
            if (this.OptionsChanged != null)
                this.OptionsChanged(this, new EventArgs());

            this.Data = await this.BeginGetData(this.Recurrence, this.RecurrenceValue);
        }

        public DateTime? GetNext(DateTime Start)
        {
            if (Repeats)
            {
                var Now = DateTime.Now;
                switch (Recurrence)
                {
                    case Recurrence.Daily:
                        while (Start < Now)
                            Start = Start.AddDays(RecurrenceValue);
                        return (DateTime?)Start;
                    case Recurrence.Weekly:
                        while (Start < Now)
                            Start = Start.AddDays(RecurrenceValue * 7);
                        return (DateTime?)Start;
                    case Recurrence.Monthly:
                        while (Start < Now)
                            Start = Start.AddMonths(RecurrenceValue);
                        return (DateTime?)Start;
                    case Recurrence.Yearly:
                        while (Start < Now)
                            Start = Start.AddYears(RecurrenceValue);
                        return (DateTime?)Start;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return this.Recurrence.ToString();
        }

        #endregion

        #region RepeatOptions

        public RepeatOptions() : base()
        {
            OnInitialized();
        }

        public RepeatOptions(Recurrence Recurrence) : base()
        {
            OnInitialized();
            this.Recurrence = Recurrence;
        }

        #endregion
    }
}
