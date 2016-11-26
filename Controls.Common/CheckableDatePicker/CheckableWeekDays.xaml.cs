using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.ComponentModel;
using System;
using System.Windows;

namespace Imagin.Controls.Common
{
    public partial class CheckableWeekDays : CheckableDatePicker
    {
        public static DependencyProperty WeekDaysProperty = DependencyProperty.Register("WeekDays", typeof(TrackableCollection<CheckableObject<DayOfWeek>>), typeof(CheckableWeekDays), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TrackableCollection<CheckableObject<DayOfWeek>> WeekDays
        {
            get
            {
                return (TrackableCollection<CheckableObject<DayOfWeek>>)GetValue(WeekDaysProperty);
            }
            set
            {
                SetValue(WeekDaysProperty, value);
            }
        }

        public static DependencyProperty SelectedWeekDaysProperty = DependencyProperty.Register("SelectedWeekDays", typeof(TrackableCollection<CheckableObject<DayOfWeek>>), typeof(CheckableWeekDays), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TrackableCollection<CheckableObject<DayOfWeek>> SelectedWeekDays
        {
            get
            {
                return (TrackableCollection<CheckableObject<DayOfWeek>>)GetValue(SelectedWeekDaysProperty);
            }
            set
            {
                SetValue(SelectedWeekDaysProperty, value);
            }
        }

        public CheckableWeekDays() : base()
        {
            InitializeComponent();

            this.WeekDays = new TrackableCollection<CheckableObject<DayOfWeek>>();
            this.WeekDays.ItemAdded += (s, e) => e.Value.Checked += (t, f) => SelectedWeekDays.Add(e.Value);

            this.WeekDays.Add(new CheckableObject<DayOfWeek>(DayOfWeek.Sunday));
            this.WeekDays.Add(new CheckableObject<DayOfWeek>(DayOfWeek.Monday));
            this.WeekDays.Add(new CheckableObject<DayOfWeek>(DayOfWeek.Tuesday));
            this.WeekDays.Add(new CheckableObject<DayOfWeek>(DayOfWeek.Wednesday));
            this.WeekDays.Add(new CheckableObject<DayOfWeek>(DayOfWeek.Thursday));
            this.WeekDays.Add(new CheckableObject<DayOfWeek>(DayOfWeek.Friday));
            this.WeekDays.Add(new CheckableObject<DayOfWeek>(DayOfWeek.Saturday));

            this.ItemsSource = this.WeekDays;
        }
    }
}
