using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Down", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Up", Type = typeof(Button))]
    public abstract class UpDown : TextBox, IPropertyChanged
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
            public double Milliseconds { get; set; } = 0d;

            /// <summary>
            /// 
            /// </summary>
            public UpDownDirection Direction { get; set; } = default(UpDownDirection);

            /// <summary>
            /// 
            /// </summary>
            internal UpDownTimer() : base()
            {
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
        protected ContentControl PART_Down { get; private set; } = null;

        /// <summary>
        /// 
        /// </summary>
        protected ContentControl PART_Up { get; private set; } = null;

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
        public static DependencyProperty DirectionalChangeProperty = DependencyProperty.Register("DirectionalChange", typeof(DirectionalNavigation), typeof(UpDown), new FrameworkPropertyMetadata(DirectionalNavigation.Circular, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DirectionalNavigation DirectionalChange
        {
            get
            {
                return (DirectionalNavigation)GetValue(DirectionalChangeProperty);
            }
            set
            {
                SetValue(DirectionalChangeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DownButtonTemplateProperty = DependencyProperty.Register("DownButtonTemplate", typeof(DataTemplate), typeof(UpDown), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate DownButtonTemplate
        {
            get
            {
                return (DataTemplate)GetValue(DownButtonTemplateProperty);
            }
            set
            {
                SetValue(DownButtonTemplateProperty, value);
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
        public static DependencyProperty UpButtonTemplateProperty = DependencyProperty.Register("UpButtonTemplate", typeof(DataTemplate), typeof(UpDown), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate UpButtonTemplate
        {
            get
            {
                return (DataTemplate)GetValue(UpButtonTemplateProperty);
            }
            set
            {
                SetValue(UpButtonTemplateProperty, value);
            }
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
        /// 
        /// </summary>
        public abstract void RiseValue();

        /// <summary>
        /// 
        /// </summary>
        public abstract void SinkValue();

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
                downCommand = downCommand ?? new RelayCommand(() => Change(UpDownDirection.Down), () => CanUpDown && (DirectionalChange == DirectionalNavigation.Circular || CanDecrease()));
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
                upCommand = upCommand ?? new RelayCommand(() => Change(UpDownDirection.Up), () => CanUpDown && (DirectionalChange == DirectionalNavigation.Circular || CanIncrease()));
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
            if (Direction == UpDownDirection.Up)
            {
                if (CanIncrease())
                {
                    Increase();
                }
                else if (DirectionalChange == DirectionalNavigation.Circular)
                    SinkValue(); //Value = Minimum;
            }
            else if (Direction == UpDownDirection.Down)
            {
                if (CanDecrease())
                {
                    Decrease();
                }
                else if (DirectionalChange == DirectionalNavigation.Circular)
                    RiseValue(); //Value = Maximum;
            }
        }

        /// <summary>
        /// Reset the timer used for making major changes.
        /// </summary>
        protected void ResetTimer()
        {
            Timer?.Stop();
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
            SetCurrentValue(TextProperty, text);
            CaretIndex = i;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                if (e.Delta > 0)
                {
                    Change(UpDownDirection.Up);
                }
                else Change(UpDownDirection.Down);

                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Down = Template.FindName("PART_Down", this) as ContentControl;

            PART_Down.PreviewMouseDown += OnButtonMouseDown;
            PART_Down.PreviewMouseUp += OnButtonMouseUp;

            PART_Down.MouseDoubleClick += Handle;
            PART_Down.MouseDown += Handle;
            PART_Down.MouseUp += Handle;

            PART_Up = Template.FindName("PART_Up", this) as ContentControl;

            PART_Up.PreviewMouseDown += OnButtonMouseDown;
            PART_Up.PreviewMouseUp += OnButtonMouseUp;

            PART_Up.MouseDoubleClick += Handle;
            PART_Up.MouseDown += Handle;
            PART_Up.MouseUp += Handle;
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
            var name = sender.As<ContentControl>().Name;

            Timer.Direction = name == "PART_Up" ? UpDownDirection.Up : name == "PART_Down" ? UpDownDirection.Down : UpDownDirection.None;
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
            Timer.Direction = UpDownDirection.None;
            Timer.Milliseconds = 0d;
        }

        /// <summary>
        /// Occurs during a major change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMajorChange(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                OnButtonMouseUp(this, null);
                return;
            }

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
