using Imagin.Common.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Imagin.Common.Input;
using System.Windows.Controls.Primitives;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Down", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Up", Type = typeof(Button))]
    public abstract class UpDown : TextBoxExt, INotifyPropertyChanged
    {
        #region Properties

        protected Button PART_Down { get; private set; } = null;

        protected Button PART_Up { get; private set; } = null;

        protected UpDownTimer Timer { get; private set; } = null;

        protected bool OnTextChangedHandled { get; set; } = false;

        protected bool OnValueChangedHandled { get; set; } = false;

        public static DependencyProperty IsUpDownEnabledProperty = DependencyProperty.Register("IsUpDownEnabled", typeof(bool), typeof(UpDown), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsUpDownEnabled
        {
            get
            {
                return (bool)GetValue(IsUpDownEnabledProperty);
            }
            set
            {
                SetValue(IsUpDownEnabledProperty, value);
            }
        }

        public static DependencyProperty MajorChangeProperty = DependencyProperty.Register("MajorChange", typeof(double), typeof(UpDown), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMajorChangeChanged));
        public double MajorChange
        {
            get
            {
                return (double)GetValue(MajorChangeProperty);
            }
            set
            {
                SetValue(MajorChangeProperty, value);
            }
        }
        static void OnMajorChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<UpDown>().OnMajorChangeChanged((double)e.NewValue);
        }

        public static DependencyProperty MajorChangeDelayProperty = DependencyProperty.Register("MajorChangeDelay", typeof(double), typeof(UpDown), new FrameworkPropertyMetadata(500.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double MajorChangeDelay
        {
            get
            {
                return (double)GetValue(MajorChangeDelayProperty);
            }
            set
            {
                SetValue(MajorChangeDelayProperty, value);
            }
        }

        public static DependencyProperty StringFormatProperty = DependencyProperty.Register("StringFormat", typeof(string), typeof(UpDown), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStringFormatChanged));
        public string StringFormat
        {
            get
            {
                return (string)GetValue(StringFormatProperty);
            }
            set
            {
                SetValue(StringFormatProperty, value);
            }
        }
        static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<UpDown>().OnStringFormatChanged();
        }

        #endregion

        #region UpDown

        /// <summary>
        /// Initializes a new instance of the <see cref="UpDown"/> class.
        /// </summary>
        public UpDown() : base()
        {
            DefaultStyleKey = typeof(UpDown);
            ResetTimer();
        }

        #endregion

        #region Methods

        #region Abstract

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool CanDecrease();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool CanIncrease();

        /// <summary>
        /// 
        /// </summary>
        public abstract void Decrease();

        /// <summary>
        /// 
        /// </summary>
        public abstract void Increase();

        #endregion

        #region Commands

        ICommand downCommand;
        public ICommand DownCommand
        {
            get
            {
                downCommand = downCommand ?? new RelayCommand(x => Decrease(), x => IsUpDownEnabled && CanDecrease());
                return downCommand;
            }
        }

        ICommand upCommand;
        public ICommand UpCommand
        {
            get
            {
                upCommand = upCommand ?? new RelayCommand(x => Increase(), x => IsUpDownEnabled && CanIncrease());
                return upCommand;
            }
        }

        #endregion

        #region Private

        void Handle(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region Protected

        protected void Change(UpDownDirection Direction)
        {
            if (Direction == UpDownDirection.Up)
                Increase();

            else if (Direction == UpDownDirection.Down)
                Decrease();
        }

        protected void ResetTimer()
        {
            Timer = new UpDownTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(MajorChange);
            Timer.Tick += OnMajorChange;
        }

        /// <summary>
        /// Set text; string format should be applied prior to calling.
        /// </summary>
        protected void SetText(string text)
        {
            var i = CaretIndex;
            Text = text;
            CaretIndex = i;
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Up = Template.FindName("PART_Up", this).As<Button>();
            PART_Up.PreviewMouseDown += OnButtonMouseDown;
            PART_Up.PreviewMouseUp += OnButtonMouseUp;
            PART_Up.MouseDoubleClick += Handle;
            PART_Up.MouseDown += Handle;
            PART_Up.MouseUp += Handle;

            PART_Down = Template.FindName("PART_Down", this).As<Button>();
            PART_Down.PreviewMouseDown += OnButtonMouseDown;
            PART_Down.PreviewMouseUp += OnButtonMouseUp;
            PART_Down.MouseDoubleClick += Handle;
            PART_Down.MouseDown += Handle;
            PART_Down.MouseUp += Handle;
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                if (e.Delta > 0)
                {
                    Increase();
                }
                else
                {
                    Decrease();
                }
                e.Handled = true;
            }
        }

        #endregion

        #region Virtual

        protected virtual void OnButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            Timer.Direction = sender.As<MaskedButton>().CommandParameter.ToString().ParseEnum<UpDownDirection>();
            Timer.Start();
        }

        protected virtual void OnButtonMouseUp(object sender, MouseButtonEventArgs e)
        {
            Timer.Stop();
            Timer.Milliseconds = 0.0;
            Timer.Direction = UpDownDirection.None;
        }

        /// <summary>
        /// Occurs during a major change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMajorChange(object sender, EventArgs e)
        {
            Timer.Milliseconds += Timer.Interval.TotalMilliseconds;
            if (Timer.Milliseconds < MajorChangeDelay) return;
            Change(Timer.Direction);
        }

        /// <summary>
        /// Occurs when <see cref="MajorChange"/> changes.
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnMajorChangeChanged(double Value)
        {
            ResetTimer();
        }

        /// <summary>
        /// Occurs when <see cref="StringFormat"/> changes.
        /// </summary>
        protected virtual void OnStringFormatChanged()
        {
        }

        #endregion

        #endregion

        #region Types

        protected enum UpDownDirection
        {
            None,
            Up,
            Down
        }

        protected class UpDownTimer : DispatcherTimer
        {
            public double Milliseconds
            {
                get; set;
            }

            public UpDownDirection Direction
            {
                get; set;
            }

            internal UpDownTimer() : base()
            {
                Milliseconds = 0.0;
            }
        }

        #endregion

        #region INotifyPropertyChanged

        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
