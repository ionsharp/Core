using Imagin.Common.Linq;
using Imagin.Common.Time;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_Calendar), Type = typeof(System.Windows.Controls.Calendar))]
    [TemplatePart(Name = nameof(PART_TimeSpanUpDown), Type = typeof(TimeSpanUpDown))]
    public class DateTimeUpDown : MultiUpDown<DateTime, DateTimePart>
    {
        #region Properties

        System.Windows.Controls.Calendar PART_Calendar = null;

        TimeSpanUpDown PART_TimeSpanUpDown = null;

        //...

        public override DateTime AbsoluteMaximum => Convert(DateTime.MaxValue, Kind);

        public override DateTime AbsoluteMinimum => Convert(DateTime.MinValue, Kind);

        public override DateTime DefaultValue
        {
            get
            {
                var now = DateTime.Now;
                return Kind switch
                {
                    DateTimeKind.Utc => DateTime.UtcNow,
                    _ => DateTime.Now,
                };
            }
        }

        //...

        /// <summary>
        /// The value to increment by (e.g., if <see cref="SelectedPart" /> = <see cref="DateTimePart.Month" /> and <see cref="Increment" /> = 3, increment by 3 months).
        /// </summary>
        protected double Increment { get; set; } = 1d;
        
        public static readonly DependencyProperty CalendarModeProperty = DependencyProperty.Register(nameof(CalendarMode), typeof(CalendarMode), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(CalendarMode.Month));
        public CalendarMode CalendarMode
        {
            get => (CalendarMode)GetValue(CalendarModeProperty);
            set => SetValue(CalendarModeProperty, value);
        }

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(DateTimeKind), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(DateTimeKind.Local, OnKindChanged));
        public DateTimeKind Kind
        {
            get => (DateTimeKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }
        static void OnKindChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DateTimeUpDown>().OnKindChanged(new Value<DateTimeKind>(e));

        public static readonly DependencyProperty SelectedPartProperty = DependencyProperty.Register(nameof(SelectedPart), typeof(DateTimePart), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(DateTimePart.Day));
        public DateTimePart SelectedPart
        {
            get => (DateTimePart)GetValue(SelectedPartProperty);
            set => SetValue(SelectedPartProperty, value);
        }

        public static readonly DependencyProperty TimeOfDayProperty = DependencyProperty.Register(nameof(TimeOfDay), typeof(TimeSpan), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(default(TimeSpan)));
        public TimeSpan TimeOfDay
        {
            get => (TimeSpan)GetValue(TimeOfDayProperty);
            private set => SetValue(TimeOfDayProperty, value);
        }

        public static readonly DependencyProperty TimeMenuIncrementProperty = DependencyProperty.Register(nameof(TimeMenuIncrement), typeof(TimeSpan), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(TimeSpan.FromHours(1)));
        public TimeSpan TimeMenuIncrement
        {
            get => (TimeSpan)GetValue(TimeMenuIncrementProperty);
            set => SetValue(TimeMenuIncrementProperty, value);
        }

        public static readonly DependencyProperty TimeMenuMaximumProperty = DependencyProperty.Register(nameof(TimeMenuMaximum), typeof(TimeSpan), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(TimeSpan.FromDays(1)));
        public TimeSpan TimeMenuMaximum
        {
            get => (TimeSpan)GetValue(TimeMenuMaximumProperty);
            set => SetValue(TimeMenuMaximumProperty, value);
        }

        public static readonly DependencyProperty TimeMenuMinimumProperty = DependencyProperty.Register(nameof(TimeMenuMinimum), typeof(TimeSpan), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(TimeSpan.Zero));
        public TimeSpan TimeMenuMinimum
        {
            get => (TimeSpan)GetValue(TimeMenuMinimumProperty);
            set => SetValue(TimeMenuMinimumProperty, value);
        }

        #endregion

        #region DateTimeUpDown

        public DateTimeUpDown() : base() 
        {
            XTextBoxBase.SetSuggestions(this, new List<object>());
        }

        #endregion

        #region Methods

        DateTime Convert(DateTime dateTime, DateTimeKind kind)
        {
            if (dateTime.Kind == kind)
                return dateTime;

            if (dateTime.Kind == DateTimeKind.Unspecified || kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(dateTime, kind);

            if (kind == DateTimeKind.Local)
            {
                return dateTime.ToLocalTime();
            }
            else return dateTime.ToUniversalTime();
        }

        void Increase(double increment)
        {
            var result = default(DateTime);
            if (Value != default)
            {
                try
                {
                    switch (SelectedPart)
                    {
                        case DateTimePart.Day:
                            result = Value.AddDays(increment);
                            break;
                        case DateTimePart.Hour:
                            result = Value.AddHours(increment);
                            break;
                        case DateTimePart.Meridian:
                            result = Value.Meridiem() switch
                            {
                                Meridiem.AM => Value.AddHours(12d),
                                Meridiem.PM => Value.AddHours(-12d),
                                _ => Value,
                            };
                            break;
                        case DateTimePart.Millisecond:
                            result = Value.AddMilliseconds(increment);
                            break;
                        case DateTimePart.Minute:
                            result = Value.AddMinutes(increment);
                            break;
                        case DateTimePart.Month:
                            result = Value.AddMonths(increment.Round().Int32());
                            break;
                        case DateTimePart.Second:
                            result = Value.AddSeconds(increment);
                            break;
                        case DateTimePart.Year:
                            result = Value.AddYears(increment.Round().Int32());
                            break;
                    }
                }
                catch
                {
                    //An error may occur if the result is "unrepresentable"
                    result = Value;
                }
            }
            else result = Minimum;

            SetCurrentValue(ValueProperty.Property, result);
        }

        void OnTimeChanged(object sender, TextChangedEventArgs e)
        {
            var i = PART_TimeSpanUpDown.Value;
            if (i != null)
                Value = Value.Date.AddHours(i.Hours).AddMinutes(i.Minutes).AddSeconds(i.Seconds).AddMilliseconds(i.Milliseconds);
        }

        //...

        protected override bool CanDecrease() => Value > Minimum;

        protected override bool CanIncrease() => Value < Maximum;

        //...

        protected override DateTime GetValue(string input) => input.DateTime();

        //...

        protected override object OnMaximumCoerced(object input)
        {
            var result = input.As<DateTime>().Coerce(AbsoluteMaximum, Value);
            result = Convert(result, Kind);
            return result;
        }

        protected override object OnMinimumCoerced(object input)
        {
            var result = input.As<DateTime>().Coerce(Value, AbsoluteMinimum);
            result = Convert(result, Kind);
            return result;
        }

        //...

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            if (e.Text?.Length > 0)
            {
                var i = e.Text[0];
                if (char.IsDigit(i) || i == ':' || i == '.' || i == '-' || i == ',' || char.IsLetter(i) || i == '/')
                {
                    e.Handled = false;
                    return;
                }
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            SetCurrentValue(TimeOfDayProperty, Value.TimeOfDay);
        }

        protected override void OnValueChanged(Value<DateTime> input)
        {
            base.OnValueChanged(input);
            handle.SafeInvoke(() => SetCurrentValue(KindProperty, input.New.Kind));
            SetCurrentValue(TimeOfDayProperty, input.New.TimeOfDay);
        }

        protected override object OnValueCoerced(object input)
        {
            var result = input.As<DateTime>().Coerce(Maximum, Minimum);

            if (Kind != DateTimeKind.Unspecified)
                result = Convert(result, Kind);

            return result;
        }

        //...

        protected override string ToString(DateTime input) => input.ToString(CultureInfo.CurrentCulture);

        //...

        public override void Decrease() => Increase(-Increment);

        public override void Increase() => Increase(Increment);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_TimeSpanUpDown = Template.FindName(nameof(PART_TimeSpanUpDown), this) as TimeSpanUpDown;
            if (PART_TimeSpanUpDown != null)
                PART_TimeSpanUpDown.TextChanged += OnTimeChanged;

            PART_Calendar = Template.FindName(nameof(PART_Calendar), this) as System.Windows.Controls.Calendar;
            if (PART_Calendar != null)
                PART_Calendar.SelectedDatesChanged += OnSelectedDatesChanged;
        }

        //...

        protected virtual void OnKindChanged(Value<DateTimeKind> input)
        {
            handle.SafeInvoke(() => SetCurrentValue(ValueProperty.Property, Convert(Value, input.New)));
            SetCurrentValue(MaximumProperty.Property, Convert(Maximum, input.New));
            SetCurrentValue(MinimumProperty.Property, Convert(Minimum, input.New));
        }

        protected virtual void OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems.FirstOrDefault()?.As<DateTime>();

            if (item != null)
                Value = item.Value.Date.AddHours(Value.TimeOfDay.Hours).AddMinutes(Value.TimeOfDay.Minutes).AddSeconds(Value.TimeOfDay.Seconds).AddMilliseconds(Value.TimeOfDay.Milliseconds);
        }

        #endregion
    }
}