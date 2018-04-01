using Imagin.Common.Continuance;
using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeSpanUpDown : MultiUpDown<TimeSpan, TimePart>
    {
        #region Properties

        bool SelectionChangeHandled;

        /// <summary>
        /// 
        /// </summary>
        public override TimeSpan AbsoluteMaximum
        {
            get
            {
                return TimeSpan.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override TimeSpan AbsoluteMinimum
        {
            get
            {
                return TimeSpan.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override TimeSpan DefaultValue
        {
            get
            {
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(double), typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double Increment
        {
            get
            {
                return (double)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedPart"/> property.
        /// </summary>
        public static DependencyProperty SelectedPartProperty = DependencyProperty.Register("SelectedPart", typeof(TimePart), typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(TimePart.Hour, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the selected <see cref="TimePart"/> of the <see cref="TimeSpan"/> value.
        /// </summary>
        public TimePart SelectedPart
        {
            get
            {
                return (TimePart)GetValue(SelectedPartProperty);
            }
            set
            {
                SetValue(SelectedPartProperty, value);
            }
        }

        #endregion

        #region TimeSpanUpDown

        /// <summary>
        /// 
        /// </summary>
        public TimeSpanUpDown() : base()
        {
        }

        #endregion

        #region Methods

        void Increase(double increment)
        {
            var result = TimeSpan.Zero;

            if (Value != default(TimeSpan))
            {
                try
                {
                    switch (SelectedPart)
                    {
                        case TimePart.Hour:
                            result = Value + TimeSpan.FromHours(increment);
                            break;
                        case TimePart.Minute:
                            result = Value + TimeSpan.FromMinutes(increment);
                            break;
                        case TimePart.Second:
                            result = Value + TimeSpan.FromSeconds(increment);
                            break;
                    }
                }
                catch
                {
                    result = Value;
                }
            }
            else result = Minimum;

            SetCurrentValue(ValueProperty.Property, result);
        }

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
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override TimeSpan GetValue(string Value)
        {
            return Value.ToTimeSpan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (!SelectionChangeHandled)
            {
                var targetIndex = CaretIndex;

                var end = 0;
                var start = 0;
                var found = false;

                var part = TimePart.Hour;
                var length = Text.Length;

                var index = 0;
                foreach (var @char in Text)
                {
                    if (index == targetIndex)
                    {
                        found = true;
                        end = length;
                    }

                    if (@char == ':')
                    {
                        if (found)
                        {
                            end = index;
                            break;
                        }

                        switch (part)
                        {
                            case TimePart.Hour:
                                part = TimePart.Minute;
                                break;
                            case TimePart.Minute:
                                part = TimePart.Second;
                                break;
                        }

                        start = index + 1;
                    }

                    index++;
                }

                start = start.Coerce(length);
                length = (end - start).Coerce(length);

                if (SelectionStart!= start && SelectionLength != length)
                {
                    SelectionChangeHandled = true;
                    Select(start, length);
                    SelectionChangeHandled = false;
                }

                SetCurrentValue(SelectedPartProperty, part);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnMaximumCoerced(object value)
        {
            return value.As<TimeSpan>().Coerce(AbsoluteMaximum, Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object value)
        {
            return value.As<TimeSpan>().Coerce(Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object value)
        {
            return value.As<TimeSpan>().Coerce(Maximum, Minimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(TimeSpan Value)
        {
            return Value.ToString();
        }

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

        #endregion
    }
}
