using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.ComponentModel;
using System.Windows;

namespace Imagin.Controls.Common
{

    public partial class CheckableDays : CheckableDatePicker
    {
        public static DependencyProperty IsFocusableProperty = DependencyProperty.Register("IsFocusable", typeof(bool), typeof(CheckableDays), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsFocusable
        {
            get
            {
                return (bool)GetValue(IsFocusableProperty);
            }
            set
            {
                SetValue(IsFocusableProperty, value);
            }
        }

        public static DependencyProperty DaysProperty = DependencyProperty.Register("Days", typeof(TrackableCollection<CheckableObject<int>>), typeof(CheckableDays), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TrackableCollection<CheckableObject<int>> Days
        {
            get
            {
                return (TrackableCollection<CheckableObject<int>>)GetValue(DaysProperty);
            }
            set
            {
                SetValue(DaysProperty, value);
            }
        }

        public static DependencyProperty SelectedDaysProperty = DependencyProperty.Register("SelectedDays", typeof(TrackableCollection<CheckableObject<int>>), typeof(CheckableDays), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TrackableCollection<CheckableObject<int>> SelectedDays
        {
            get
            {
                return (TrackableCollection<CheckableObject<int>>)GetValue(SelectedDaysProperty);
            }
            set
            {
                SetValue(SelectedDaysProperty, value);
            }
        }

        public CheckableDays() : base()
        {
            InitializeComponent();

            this.Days = new TrackableCollection<CheckableObject<int>>();
            this.Days.ItemAdded += (s, e) => 
            {
                e.Value.Checked += (t, f) =>
                {
                    if (f.Value)
                        SelectedDays.Add(e.Value);
                    else SelectedDays.Remove(e.Value);
                };
            };

            for (int i = 1; i <= 31; i++)
                this.Days.Add(new CheckableObject<int>(i));

            this.ItemsSource = this.Days;
        }
    }
}
