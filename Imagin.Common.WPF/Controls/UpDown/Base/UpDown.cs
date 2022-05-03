using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_Down), Type = typeof(Button))]
    [TemplatePart(Name = nameof(PART_Up), Type = typeof(Button))]
    public abstract class UpDown : TextBox
    {
        #region UpDownDirection

        protected enum UpDownDirection
        {
            None, Up, Down
        }

        #endregion

        #region UpDownTimer

        protected class UpDownTimer : DispatcherTimer
        {
            public double Milliseconds { get; set; } = 0;

            public UpDownDirection Direction { get; set; } = default;

            internal UpDownTimer() : base() { }
        }

        #endregion

        #region Properties

        protected ContentControl PART_Down { get; private set; } = null;

        protected ContentControl PART_Up { get; private set; } = null;

        //...

        protected UpDownTimer Timer { get; private set; } = null;

        //...

        public static readonly DependencyProperty CanUpDownProperty = DependencyProperty.Register(nameof(CanUpDown), typeof(bool), typeof(UpDown), new FrameworkPropertyMetadata(true));
        public bool CanUpDown
        {
            get => (bool)GetValue(CanUpDownProperty);
            set => SetValue(CanUpDownProperty, value);
        }
        
        public static readonly DependencyProperty DirectionalChangeProperty = DependencyProperty.Register(nameof(DirectionalChange), typeof(DirectionalNavigation), typeof(UpDown), new FrameworkPropertyMetadata(DirectionalNavigation.Circular));
        public DirectionalNavigation DirectionalChange
        {
            get => (DirectionalNavigation)GetValue(DirectionalChangeProperty);
            set => SetValue(DirectionalChangeProperty, value);
        }

        public static readonly DependencyProperty DownButtonTemplateProperty = DependencyProperty.Register(nameof(DownButtonTemplate), typeof(DataTemplate), typeof(UpDown), new FrameworkPropertyMetadata(default(DataTemplate)));
        public DataTemplate DownButtonTemplate
        {
            get => (DataTemplate)GetValue(DownButtonTemplateProperty);
            set => SetValue(DownButtonTemplateProperty, value);
        }

        public static readonly DependencyProperty MajorChangeProperty = DependencyProperty.Register(nameof(MajorChange), typeof(double), typeof(UpDown), new FrameworkPropertyMetadata(100.0, OnMajorChangeChanged));
        public double MajorChange
        {
            get => (double)GetValue(MajorChangeProperty);
            set => SetValue(MajorChangeProperty, value);
        }
        static void OnMajorChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.As<UpDown>().OnMajorChangeChanged(new Value<double>(e));

        public static readonly DependencyProperty MajorChangeDelayProperty = DependencyProperty.Register(nameof(MajorChangeDelay), typeof(double), typeof(UpDown), new FrameworkPropertyMetadata(500.0));
        public double MajorChangeDelay
        {
            get => (double)GetValue(MajorChangeDelayProperty);
            set => SetValue(MajorChangeDelayProperty, value);
        }

        public static readonly DependencyProperty UpButtonTemplateProperty = DependencyProperty.Register(nameof(UpButtonTemplate), typeof(DataTemplate), typeof(UpDown), new FrameworkPropertyMetadata(default(DataTemplate)));
        public DataTemplate UpButtonTemplate
        {
            get => (DataTemplate)GetValue(UpButtonTemplateProperty);
            set => SetValue(UpButtonTemplateProperty, value);
        }

        #endregion

        #region UpDown

        public UpDown() : base() => this.RegisterHandler(i => ResetTimer(), i =>
        {
            if (Timer is not null)
            {
                Timer.Stop();
                Timer.Tick -= OnMajorChange;
            }
        });

        #endregion

        #region Methods

        #region Abstract

        protected abstract bool CanDecrease();

        protected abstract bool CanIncrease();

        public abstract void Decrease();

        public abstract void ValueToMaximum();

        public abstract void ValueToMinimum();

        public abstract void Increase();

        #endregion

        #region Commands

        ICommand downCommand;
        public ICommand DownCommand => downCommand ??= new RelayCommand(() => Change(UpDownDirection.Down), () => CanUpDown && (DirectionalChange == DirectionalNavigation.Circular || CanDecrease()));

        ICommand upCommand;
        public ICommand UpCommand => upCommand ??= new RelayCommand(() => Change(UpDownDirection.Up), () => CanUpDown && (DirectionalChange == DirectionalNavigation.Circular || CanIncrease()));

        #endregion

        #region Protected

        /// <summary>
        /// Increase or decrease based on given direction.
        /// </summary>
        /// <param name="Direction"></param>
        protected void Change(UpDownDirection Direction)
        {
            if (Direction == UpDownDirection.Up)
            {
                if (CanIncrease())
                {
                    Increase();
                }
                else if (DirectionalChange == DirectionalNavigation.Circular)
                    ValueToMinimum(); //Value = Minimum;
            }
            else if (Direction == UpDownDirection.Down)
            {
                if (CanDecrease())
                {
                    Decrease();
                }
                else if (DirectionalChange == DirectionalNavigation.Circular)
                    ValueToMaximum(); //Value = Maximum;
            }
        }

        protected void StopTimer()
        {
            Timer.Stop();
            Timer.Direction = UpDownDirection.None;
            Timer.Milliseconds = 0d;
        }

        protected void ResetTimer()
        {
            Timer = Timer ?? new();
            Timer.Stop();
            Timer.Interval = TimeSpan.FromMilliseconds(MajorChange);
            Timer.Tick -= OnMajorChange;
            Timer.Tick += OnMajorChange;
        }

        /// <summary>
        /// Set text; string format should be applied prior to calling.
        /// </summary>
        protected void SetText(string text)
        {
            var i = CaretIndex;
            i = i <= 0 ? text.Length - 1 : i;
            SetCurrentValue(TextProperty, text);
            CaretIndex = i;
        }

        #endregion

        #region Overrides

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            var name = e.OriginalSource.FindParent<ImageButton>()?.Name;
            Timer.Direction = name == nameof(PART_Up) ? UpDownDirection.Up : name == nameof(PART_Down) ? UpDownDirection.Down : UpDownDirection.None;
            Timer.Start();
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            StopTimer();
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if (ModifierKeys.Control.Pressed())
            {
                if (e.Delta > 0)
                {
                    Change(UpDownDirection.Up);
                }
                else Change(UpDownDirection.Down);
                e.Handled = true;
            }
        }

        #endregion

        #region Virtual

        protected virtual void OnMajorChange(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                StopTimer();
                return;
            }

            Timer.Milliseconds += Timer.Interval.TotalMilliseconds;
            if (Timer.Milliseconds < MajorChangeDelay)
                return;

            Change(Timer.Direction);
        }

        protected virtual void OnMajorChangeChanged(Value<double> input)
        {
            ResetTimer();
        }

        #endregion

        #endregion
    }
}