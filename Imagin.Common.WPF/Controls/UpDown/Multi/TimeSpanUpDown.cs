using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using Imagin.Common.Time;
using System;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class TimeSpanUpDownSuggestionHandler : ISuggest
    {
        public string Convert(object input) => $"{input}";

        public bool Handle(object input, string text) => true;
    }

    public class TimeSpanUpDown : MultiUpDown<TimeSpan, TimePart>
    {
        bool SelectionChangeHandled;

        //...

        public ObservableCollection<TimeSpan> Items { get; private set; } = new();

        //...

        public override TimeSpan AbsoluteMaximum => TimeSpan.MaxValue;

        public override TimeSpan AbsoluteMinimum => TimeSpan.MinValue;

        public override TimeSpan DefaultValue => TimeSpan.Zero;

        //...

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(nameof(Increment), typeof(double), typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(1d));
        public double Increment
        {
            get => (double)GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }

        public static readonly DependencyProperty MenuIncrementProperty = DependencyProperty.Register(nameof(MenuIncrement), typeof(TimeSpan), typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(TimeSpan.FromHours(1), OnMenuItemsChanged));
        public TimeSpan MenuIncrement
        {
            get => (TimeSpan)GetValue(MenuIncrementProperty);
            set => SetValue(MenuIncrementProperty, value);
        }

        public static readonly DependencyProperty MenuMaximumProperty = DependencyProperty.Register(nameof(MenuMaximum), typeof(TimeSpan), typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(TimeSpan.FromDays(1), OnMenuItemsChanged));
        public TimeSpan MenuMaximum
        {
            get => (TimeSpan)GetValue(MenuMaximumProperty);
            set => SetValue(MenuMaximumProperty, value);
        }

        public static readonly DependencyProperty MenuMinimumProperty = DependencyProperty.Register(nameof(MenuMinimum), typeof(TimeSpan), typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(TimeSpan.Zero, OnMenuItemsChanged));
        public TimeSpan MenuMinimum
        {
            get => (TimeSpan)GetValue(MenuMinimumProperty);
            set => SetValue(MenuMinimumProperty, value);
        }

        static void OnMenuItemsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TimeSpanUpDown>().OnMenuItemsChanged(new Value<TimeSpan>(e));

        public static readonly DependencyProperty SelectedPartProperty = DependencyProperty.Register(nameof(SelectedPart), typeof(TimePart), typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(TimePart.Hour));
        public TimePart SelectedPart
        {
            get => (TimePart)GetValue(SelectedPartProperty);
            set => SetValue(SelectedPartProperty, value);
        }

        //...

        public TimeSpanUpDown() : base()
        {
            XTextBoxBase.SetSuggestions(this, Items);
            XTextBoxBase.SetSuggestionHandler(this, new TimeSpanUpDownSuggestionHandler());
            UpdateMenu();
        }

        //...

        void Increase(double increment)
        {
            var result = TimeSpan.Zero;
            if (Value != default)
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

        //...

        void UpdateMenu()
        {
            Items.Clear();

            var result = MenuMinimum;
            while (result.TotalSeconds < MenuMaximum.TotalSeconds)
            {
                Items.Add(result);
                result = result.Add(MenuIncrement);
            }
        }

        //...

        protected override bool CanDecrease() => Value > Minimum;

        protected override bool CanIncrease() => Value < Maximum;

        //...

        protected override TimeSpan GetValue(string input) => input.TimeSpan();

        //...

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

        //...

        protected override object OnMaximumCoerced(object input) => input.As<TimeSpan>().Coerce(AbsoluteMaximum, Value);

        protected override object OnMinimumCoerced(object input) => input.As<TimeSpan>().Coerce(Value, AbsoluteMinimum);

        //...

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            if (e.Text?.Length > 0)
            {
                var i = e.Text[0];
                if (char.IsDigit(i) || i == ':' || i == '.' || i == '-' || i == ',')
                {
                    e.Handled = false;
                    return;
                }
                e.Handled = true;
            }
        }

        //...

        protected override object OnValueCoerced(object input) => input.As<TimeSpan>().Coerce(Maximum, Minimum);

        //...

        protected override string ToString(TimeSpan input) => input.ToString();

        //...

        protected virtual void OnMenuItemsChanged(Value<TimeSpan> input) => UpdateMenu();

        //...

        public override void Decrease() => Increase(-Increment);

        public override void Increase() => Increase(Increment);
    }
}