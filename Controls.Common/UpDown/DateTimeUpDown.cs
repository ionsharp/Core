using Imagin.Common.Extensions;
using Imagin.Common.Primitives;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Calendar", Type = typeof(Calendar))]
    [TemplatePart(Name = "PART_DropDown", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_TimeBox", Type = typeof(ComboBox))]
    public class DateTimeUpDown : UpDown<DateTime>
    {
        #region Properties
        
        Popup PART_DropDown { get; set; } = null;

        Calendar PART_Calendar { get; set; } = null;

        ComboBox PART_TimeBox { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public override DateTime AbsoluteMaximum
        {
            get
            {
                return DateTime.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override DateTime AbsoluteMinimum
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override DateTime DefaultValue
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// The value to increment by (e.g., if <see cref="SelectedPart" /> = <see cref="DateTimePart.Month" /> and <see cref="Increment" /> = 3, increment by 3 months).
        /// </summary>
        protected double Increment { get; set; } = 1d;

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
        public static DependencyProperty DropDownStretchProperty = DependencyProperty.Register("DropDownStretch", typeof(Stretch), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(Stretch.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #endregion

        #region DateTimeUpDown

        public DateTimeUpDown() : base()
        {
            DefaultStyleKey = typeof(DateTimeUpDown);
        }

        #endregion

        #region Methods

        protected override DateTime GetValue(string Value)
        {
            return Value.ToDateTime();
        }

        protected override string ToString(DateTime Value)
        {
            return Value.ToString(StringFormat);
        }

        protected override object OnMaximumCoerced(object Value)
        {
            return Value.As<DateTime>().Coerce(AbsoluteMaximum, this.Value);
        }

        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<DateTime>().Coerce(this.Value, AbsoluteMinimum);
        }

        protected override object OnValueCoerced(object Value)
        {
            return Value.As<DateTime>().Coerce(Maximum, Minimum);
        }

        protected override bool CanDecrease()
        {
            try
            {
                Change(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override bool CanIncrease()
        {
            try
            {
                Change(true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override bool OnPreviewMouseLeftButtonDownHandled(MouseButtonEventArgs e, Type[] HandledTypes = null)
        {
            return base.OnPreviewMouseLeftButtonDownHandled(e, new Type[] 
            {
                typeof(Button),
                typeof(ToggleButton),
            });
        }

        protected virtual void OnDropDownClosed(object sender, EventArgs e)
        {
            //Just in case...
            if (IsDropDownOpen)
                IsDropDownOpen = false;
        }

        protected virtual void OnIsDropDownOpenChanged(bool Value)
        {
            if (PART_DropDown != null)
                PART_DropDown.IsOpen = Value;
        }

        protected virtual void OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!StaysOpenOnSelection)
                IsDropDownOpen = false;
        }

        protected DateTime Change(bool IncreaseOrDecrease)
        {
            if (Value == default(DateTime))
                Value = DateTime.MinValue;

            var i = IncreaseOrDecrease ? Increment : -Increment;

            switch (SelectedPart)
            {
                case DateTimePart.Day:
                    return Value.AddDays(i);
                case DateTimePart.Hour:
                    return Value.AddHours(i);
                case DateTimePart.Meridian:
                    switch (Value.GetMeridiem())
                    {
                        case Imagin.Common.Meridiem.Ante:
                            return Value.AddHours(12d);
                        case Imagin.Common.Meridiem.Post:
                            return Value.AddHours(-12d);
                    }
                    return Value;
                case DateTimePart.Millisecond:
                    return Value.AddMilliseconds(i);
                case DateTimePart.Minute:
                    return Value.AddMinutes(i);
                case DateTimePart.Month:
                    return Value.AddMonths(i.Round().ToInt32());
                case DateTimePart.Second:
                    return Value.AddSeconds(i);
                case DateTimePart.Year:
                    return Value.AddYears(i.Round().ToInt32());
            }

            return default(DateTime);
        }

        public override void Decrease()
        {
            Value = Change(false);
        }

        public override void Increase()
        {
            Value = Change(true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_TimeBox = Template.FindName("PART_TimeBox", this) as ComboBox;
            if (PART_TimeBox != null)
            {
                var k = new DateTime(DateTime.Now.Year, 1, 1);
                for (var i = 0; i < 24; i++)
                {
                    var j = k.AddHours(i);
                    PART_TimeBox.Items.Add(j);
                }

                PART_TimeBox.SelectionChanged += (s, e) =>
                {
                    var t = e.AddedItems[0].As<DateTime>();
                    Value = new DateTime(Value.Year, Value.Month, Value.Day, t.Hour, t.Minute, t.Second);
                };
            }

            PART_Calendar = Template.FindName("PART_Calendar", this) as Calendar;
            if (PART_Calendar != null)
                PART_Calendar.SelectedDatesChanged += OnSelectedDatesChanged;

            PART_DropDown = Template.FindName("PART_DropDown", this) as Popup;
            if (PART_DropDown != null)
                PART_DropDown.Closed += OnDropDownClosed;
        }

        #endregion
    }
}
