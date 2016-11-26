using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.ComponentModel;
using System.Windows;

namespace Imagin.Controls.Common
{
    public partial class CheckableMonths : CheckableDatePicker
    {
        public static DependencyProperty MonthsProperty = DependencyProperty.Register("Months", typeof(TrackableCollection<CheckableObject<string>>), typeof(CheckableMonths), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TrackableCollection<CheckableObject<string>> Months
        {
            get
            {
                return (TrackableCollection<CheckableObject<string>>)GetValue(MonthsProperty);
            }
            set
            {
                SetValue(MonthsProperty, value);
            }
        }

        public static DependencyProperty SelectedMonthsProperty = DependencyProperty.Register("SelectedMonths", typeof(TrackableCollection<CheckableObject<string>>), typeof(CheckableDays), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TrackableCollection<CheckableObject<string>> SelectedMonths
        {
            get
            {
                return (TrackableCollection<CheckableObject<string>>)GetValue(SelectedMonthsProperty);
            }
            set
            {
                SetValue(SelectedMonthsProperty, value);
            }
        }

        public CheckableMonths() : base()
        {
            InitializeComponent();

            this.Months = new TrackableCollection<CheckableObject<string>>();
            this.Months.ItemAdded += (s, e) => e.Value.Checked += (t, f) => SelectedMonths.Add(e.Value);

            this.Months.Add(new CheckableObject<string>("Jan"));
            this.Months.Add(new CheckableObject<string>("Feb"));
            this.Months.Add(new CheckableObject<string>("Mar"));
            this.Months.Add(new CheckableObject<string>("Apr"));
            this.Months.Add(new CheckableObject<string>("May"));
            this.Months.Add(new CheckableObject<string>("Jun"));
            this.Months.Add(new CheckableObject<string>("Jul"));
            this.Months.Add(new CheckableObject<string>("Aug"));
            this.Months.Add(new CheckableObject<string>("Sep"));
            this.Months.Add(new CheckableObject<string>("Oct"));
            this.Months.Add(new CheckableObject<string>("Nov"));
            this.Months.Add(new CheckableObject<string>("Dec"));

            this.ItemsSource = this.Months;
        }
    }
}
