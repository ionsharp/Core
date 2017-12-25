using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Imagin.Common.Linq;

namespace Imagin.Controls.Common
{
    public partial class DateTimePicker : UserControl
    {
        bool DateTimeChangeHandled = false;

        bool TimeChangeHandled = false;

        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register("DateTime", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(DateTime.UtcNow, OnDateTimeChanged));
        public DateTime DateTime
        {
            get
            {
                return (DateTime)GetValue(DateTimeProperty);
            }
            set
            {
                SetValue(DateTimeProperty, value);
            }
        }
        static void OnDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<DateTimePicker>().OnDateTimeChanged((DateTime)e.NewValue);
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(DateTimePicker), new PropertyMetadata(Orientation.Horizontal));
        public Orientation Orientation
        {
            get
            {
                return (Orientation)GetValue(OrientationProperty);
            }
            set
            {
                SetValue(OrientationProperty, value);
            }
        }

        public static readonly DependencyProperty TimeVisibilityProperty = DependencyProperty.Register("TimeVisibility", typeof(Visibility), typeof(DateTimePicker), new PropertyMetadata(Visibility.Visible));
        public Visibility TimeVisibility
        {
            get
            {
                return (Visibility)GetValue(TimeVisibilityProperty);
            }
            set
            {
                SetValue(TimeVisibilityProperty, value);
            }
        }

        public DateTimePicker()
        {
            InitializeComponent();
        }

        protected virtual void OnDateTimeChanged(DateTime Value)
        {
            if (!TimeChangeHandled)
            {
                DateTimeChangeHandled = true;
                PART_TimePicker.Time = Value.TimeOfDay;
                DateTimeChangeHandled = false;
            }
        }

        protected virtual void OnTimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            if (!DateTimeChangeHandled)
            {
                TimeChangeHandled = true;
                DateTime = DateTime.Date + new TimeSpan(e.NewTime.Hours, e.NewTime.Minutes, e.NewTime.Seconds);
                TimeChangeHandled = false;
            }
        }
    }
}
