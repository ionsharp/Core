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
        #region Classes

        /// <summary>
        /// 
        /// </summary>
        protected class UpDownTimer : DispatcherTimer
        {
            /// <summary>
            /// 
            /// </summary>
            public double Milliseconds
            {
                get; set;
            }

            /// <summary>
            /// 
            /// </summary>
            public UpDownDirection Direction
            {
                get; set;
            }

            /// <summary>
            /// 
            /// </summary>
            internal UpDownTimer() : base()
            {
                Milliseconds = 0.0;
            }
        }

        #endregion

        #region Enums

        /// <summary>
        /// 
        /// </summary>
        protected enum UpDownDirection
        {
            /// <summary>
            /// 
            /// </summary>
            None,
            /// <summary>
            /// 
            /// </summary>
            Up,
            /// <summary>
            /// 
            /// </summary>
            Down
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        protected Button PART_Down { get; private set; } = null;

        /// <summary>
        /// 
        /// </summary>
        protected Button PART_Up { get; private set; } = null;

        /// <summary>
        /// 
        /// </summary>
        protected UpDownTimer Timer { get; private set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CanUpDownProperty = DependencyProperty.Register("CanUpDown", typeof(bool), typeof(UpDown), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool CanUpDown
        {
            get
            {
                return (bool)GetValue(CanUpDownProperty);
            }
            set
            {
                SetValue(CanUpDownProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MajorChangeProperty = DependencyProperty.Register("MajorChange", typeof(double), typeof(UpDown), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMajorChangeChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MajorChangeDelayProperty = DependencyProperty.Register("MajorChangeDelay", typeof(double), typeof(UpDown), new FrameworkPropertyMetadata(500.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty StringFormatProperty = DependencyProperty.Register("StringFormat", typeof(string), typeof(UpDown), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStringFormatChanged));
        /// <summary>
        /// 
        /// </summary>
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
        /// Gets whether or not decreasing is enabled.
        /// </summary>
        /// <returns></returns>
        protected abstract bool CanDecrease();

        /// <summary>
        /// Gets whether or not increasing is enabled.
        /// </summary>
        /// <returns></returns>
        protected abstract bool CanIncrease();

        /// <summary>
        /// Decreases value by some value.
        /// </summary>
        public abstract void Decrease();

        /// <summary>
        /// Increases value by some value.
        /// </summary>
        public abstract void Increase();

        #endregion

        #region Commands

        ICommand downCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand DownCommand
        {
            get
            {
                downCommand = downCommand ?? new RelayCommand(x => Decrease(), x => CanUpDown && CanDecrease());
                return downCommand;
            }
        }

        ICommand upCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand UpCommand
        {
            get
            {
                upCommand = upCommand ?? new RelayCommand(x => Increase(), x => CanUpDown && CanIncrease());
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

        /// <summary>
        /// Increase or decrease based on given direction.
        /// </summary>
        /// <param name="Direction"></param>
        protected void Change(UpDownDirection Direction)
        {
            if (Direction == UpDownDirection.Up && CanIncrease())
                Increase();

            else if (Direction == UpDownDirection.Down && CanDecrease())
                Decrease();
        }

        /// <summary>
        /// Reset the timer used for making major changes.
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Down = Template.FindName("PART_Down", this).As<Button>();

            PART_Down.PreviewMouseDown += OnButtonMouseDown;
            PART_Down.PreviewMouseUp += OnButtonMouseUp;

            PART_Down.MouseDoubleClick += Handle;
            PART_Down.MouseDown += Handle;
            PART_Down.MouseUp += Handle;

            PART_Up = Template.FindName("PART_Up", this).As<Button>();

            PART_Up.PreviewMouseDown += OnButtonMouseDown;
            PART_Up.PreviewMouseUp += OnButtonMouseUp;

            PART_Up.MouseDoubleClick += Handle;
            PART_Up.MouseDown += Handle;
            PART_Up.MouseUp += Handle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                if (e.Delta > 0 && CanIncrease())
                {
                    Increase();
                }
                else if (CanDecrease())
                    Decrease();

                e.Handled = true;
            }
        }

        #endregion

        #region Virtual

        /// <summary>
        /// Occurs when the mouse presses the increase or decrease button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            Timer.Direction = sender.As<MaskedButton>().CommandParameter.ToString().ParseEnum<UpDownDirection>();
            Timer.Start();
        }

        /// <summary>
        /// Occurs when the mouse releases the increase or decrease button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (Timer.Milliseconds < MajorChangeDelay)
                return;

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

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        /// <param name="Name"></param>
        public virtual void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }

        #endregion

        #endregion
    }
}
