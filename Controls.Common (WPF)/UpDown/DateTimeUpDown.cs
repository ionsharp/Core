using Imagin.Common.Linq;
using Imagin.Common.Primitives;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Calendar", Type = typeof(System.Windows.Controls.Calendar))]
    [TemplatePart(Name = "PART_DropDown", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_TimeBox", Type = typeof(ComboBox))]
    public class DateTimeUpDown : MultiUpDown<DateTime, DateTimePart>
    {
        #region Properties

        bool KindChangeHandled = false;

        bool _ValueChangeHandled = false;

        Popup PART_DropDown { get; set; } = null;

        System.Windows.Controls.Calendar PART_Calendar { get; set; } = null;

        TimeUpDown PART_Time { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public override DateTime AbsoluteMaximum
        {
            get
            {
                return Convert(DateTime.MaxValue, Kind);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override DateTime AbsoluteMinimum
        {
            get
            {
                return Convert(DateTime.MinValue, Kind);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override DateTime DefaultValue
        {
            get
            {
                var now = DateTime.Now;
                switch (Kind)
                {
                    case DateTimeKind.Utc:
                        return DateTime.UtcNow;
                    default:
                        return DateTime.Now;
                }
            }
        }

        /// <summary>
        /// The value to increment by (e.g., if <see cref="SelectedPart" /> = <see cref="DateTimePart.Month" /> and <see cref="Increment" /> = 3, increment by 3 months).
        /// </summary>
        protected double Increment { get; set; } = 1d;
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CalendarModeProperty = DependencyProperty.Register("CalendarMode", typeof(CalendarMode), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(CalendarMode.Month, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public CalendarMode CalendarMode
        {
            get
            {
                return (CalendarMode)GetValue(CalendarModeProperty);
            }
            set
            {
                SetValue(CalendarModeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty KindProperty = DependencyProperty.Register("Kind", typeof(DateTimeKind), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(DateTimeKind.Local, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnKindChanged));
        /// <summary>
        /// 
        /// </summary>
        public DateTimeKind Kind
        {
            get
            {
                return (DateTimeKind)GetValue(KindProperty);
            }
            set
            {
                SetValue(KindProperty, value);
            }
        }
        static void OnKindChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<DateTimeUpDown>().OnKindChanged((DateTimeKind)e.NewValue);
        }

        /// <summary>
        /// Identifies the <see cref="IsDropDownOpen"/> property.
        /// </summary>
        public static DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));
        /// <summary>
        /// Gets or sets whether or not the drop down is open.
        /// </summary>
        public bool IsDropDownOpen
        {
            get
            {
                return (bool)GetValue(IsDropDownOpenProperty);
            }
            set
            {
                SetValue(IsDropDownOpenProperty, value);
            }
        }
        static void OnIsDropDownOpenChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<DateTimeUpDown>().OnIsDropDownOpenChanged((bool)e.NewValue);
        }

        /// <summary>
        /// Identifies the <see cref="DropDownAnimation"/> property.
        /// </summary>
        public static DependencyProperty DropDownAnimationProperty = DependencyProperty.Register("DropDownAnimation", typeof(PopupAnimation), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(PopupAnimation.Fade, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the drop down animation.
        /// </summary>
        public PopupAnimation DropDownAnimation
        {
            get
            {
                return (PopupAnimation)GetValue(DropDownAnimationProperty);
            }
            set
            {
                SetValue(DropDownAnimationProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="DropDownPlacement"/> property.
        /// </summary>
        public static DependencyProperty DropDownPlacementProperty = DependencyProperty.Register("DropDownPlacement", typeof(PlacementMode), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(PlacementMode.Bottom, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the drop down placement.
        /// </summary>
        public PlacementMode DropDownPlacement
        {
            get
            {
                return (PlacementMode)GetValue(DropDownPlacementProperty);
            }
            set
            {
                SetValue(DropDownPlacementProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="DropDownStretch"/> property.
        /// </summary>
        public static DependencyProperty DropDownStretchProperty = DependencyProperty.Register("DropDownStretch", typeof(Stretch), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(Stretch.Fill, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the drop down stretch. If <see cref="Stretch.None"/>, drop down width assumes width of it's content; if anything else, drop down width assumes width of parent, <see cref="DateTimeUpDown"/>.
        /// </summary>
        public Stretch DropDownStretch
        {
            get
            {
                return (Stretch)GetValue(DropDownStretchProperty);
            }
            set
            {
                SetValue(DropDownStretchProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="DropDownStyle"/> property.
        /// </summary>
        public static DependencyProperty DropDownStyleProperty = DependencyProperty.Register("DropDownStyle", typeof(Style), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets style of drop down; style must target <see cref="Border"/> control.
        /// </summary>
        public Style DropDownStyle
        {
            get
            {
                return (Style)GetValue(DropDownStyleProperty);
            }
            set
            {
                SetValue(DropDownStyleProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedPart"/> property.
        /// </summary>
        public static DependencyProperty SelectedPartProperty = DependencyProperty.Register("SelectedPart", typeof(DateTimePart), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(DateTimePart.Day, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the selected <see cref="DateTimePart"/> of the <see cref="DateTime"/> value.
        /// </summary>
        public DateTimePart SelectedPart
        {
            get
            {
                return (DateTimePart)GetValue(SelectedPartProperty);
            }
            set
            {
                SetValue(SelectedPartProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="StaysOpen"/> property.
        /// </summary>
        public static DependencyProperty StaysOpenProperty = DependencyProperty.Register("StaysOpen", typeof(bool), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not the drop down stays open when clicking neutral area outside of it.
        /// </summary>
        public bool StaysOpen
        {
            get
            {
                return (bool)GetValue(StaysOpenProperty);
            }
            set
            {
                SetValue(StaysOpenProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="StaysOpenOnSelection"/> property.
        /// </summary>
        public static DependencyProperty StaysOpenOnSelectionProperty = DependencyProperty.Register("StaysOpenOnSelection", typeof(bool), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not the drop down stays open after making a selection.
        /// </summary>
        public bool StaysOpenOnSelection
        {
            get
            {
                return (bool)GetValue(StaysOpenOnSelectionProperty);
            }
            set
            {
                SetValue(StaysOpenOnSelectionProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TimeOfDayProperty = DependencyProperty.Register("TimeOfDay", typeof(TimeSpan), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(default(TimeSpan), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan TimeOfDay
        {
            get
            {
                return (TimeSpan)GetValue(TimeOfDayProperty);
            }
            private set
            {
                SetValue(TimeOfDayProperty, value);
            }
        }
        
        #endregion

        #region DateTimeUpDown

        /// <summary>
        /// 
        /// </summary>
        public DateTimeUpDown() : base()
        {
            DefaultStyleKey = typeof(DateTimeUpDown);
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

        /// <summary>
        /// 
        /// </summary>
        void Increase(double increment)
        {
            var result = default(DateTime);
            if (Value != default(DateTime))
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
                            switch (Value.Meridiem())
                            {
                                case Imagin.Common.Meridiem.Ante:
                                    result = Value.AddHours(12d);
                                    break;
                                case Imagin.Common.Meridiem.Post:
                                    result = Value.AddHours(-12d);
                                    break;
                                default:
                                    result = Value;
                                    break;
                            }
                            break;
                        case DateTimePart.Millisecond:
                            result = Value.AddMilliseconds(increment);
                            break;
                        case DateTimePart.Minute:
                            result = Value.AddMinutes(increment);
                            break;
                        case DateTimePart.Month:
                            result = Value.AddMonths(increment.Round().ToInt32());
                            break;
                        case DateTimePart.Second:
                            result = Value.AddSeconds(increment);
                            break;
                        case DateTimePart.Year:
                            result = Value.AddYears(increment.Round().ToInt32());
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
            var value = PART_Time.Value;

            if (value != null)
                Value = Value.Date.AddHours(value.Hours).AddMinutes(value.Minutes).AddSeconds(value.Seconds).AddMilliseconds(value.Milliseconds);
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CanDecrease()
        {
            return Value > Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CanIncrease()
        {
            return Value < Maximum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override DateTime GetValue(string value)
        {
            return value.ToDateTime();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnMaximumCoerced(object value)
        {
            var result = value.As<DateTime>().Coerce(AbsoluteMaximum, Value);
            result = Convert(result, Kind);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object value)
        {
            var result = value.As<DateTime>().Coerce(Value, AbsoluteMinimum);
            result = Convert(result, Kind);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            SetCurrentValue(TimeOfDayProperty, Value.TimeOfDay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected override void OnValueChanged(DateTime Value)
        {
            base.OnValueChanged(Value);

            if (!_ValueChangeHandled)
            {
                KindChangeHandled = true;
                SetCurrentValue(KindProperty, Value.Kind);
                KindChangeHandled = false;
            }

            SetCurrentValue(TimeOfDayProperty, Value.TimeOfDay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object value)
        {
            var result = value.As<DateTime>().Coerce(Maximum, Minimum);

            if (Kind != DateTimeKind.Unspecified)
                result = Convert(result, Kind);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(DateTime Value)
        {
            return Value.ToString(CultureInfo.CurrentCulture);
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public override void Decrease()
        {
            Increase(-Increment);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Increase()
        {
            Increase(Increment);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Time = Template.FindName("PART_Time", this) as TimeUpDown;
            if (PART_Time != null)
                PART_Time.TextChanged += OnTimeChanged;

            PART_Calendar = Template.FindName("PART_Calendar", this) as System.Windows.Controls.Calendar;
            if (PART_Calendar != null)
                PART_Calendar.SelectedDatesChanged += OnSelectedDatesChanged;

            PART_DropDown = Template.FindName("PART_DropDown", this) as Popup;
            if (PART_DropDown != null)
                PART_DropDown.Closed += OnDropDownClosed;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDropDownClosed(object sender, EventArgs e)
        {
            if (IsDropDownOpen)
                IsDropDownOpen = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnIsDropDownOpenChanged(bool value)
        {
            if (PART_DropDown != null)
                PART_DropDown.IsOpen = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnKindChanged(DateTimeKind value)
        {
            if (!KindChangeHandled)
            {
                _ValueChangeHandled = true;
                SetCurrentValue(ValueProperty.Property, Convert(Value, value));
                _ValueChangeHandled = false;
            }

            SetCurrentValue(MaximumProperty.Property, Convert(Maximum, value));
            SetCurrentValue(MinimumProperty.Property, Convert(Minimum, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems.FirstOrDefault()?.As<DateTime>();

            if (item != null)
                Value = item.Value.Date.AddHours(Value.TimeOfDay.Hours).AddMinutes(Value.TimeOfDay.Minutes).AddSeconds(Value.TimeOfDay.Seconds).AddMilliseconds(Value.TimeOfDay.Milliseconds);

            if (!StaysOpenOnSelection)
                SetCurrentValue(IsDropDownOpenProperty, false);
        }

        #endregion
    }
}
