using Imagin.Common.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Imagin.Controls.Common
{
    public abstract class UpDown : AdvancedTextBox, INotifyPropertyChanged
    {
        #region Properties

        #region Dependency 

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
        static void OnMajorChangeChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpDown UpDown = (UpDown)Object;
            UpDown.ResetTimer();
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
        static void OnStringFormatChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpDown UpDown = (UpDown)Object;
            UpDown.OnStringFormatChanged();
        }

        #endregion

        #region Protected

        protected UpDownTimer Timer
        {
            get; set;
        }
        
        protected bool OnTextChangedHandled = false;

        protected bool OnValueChangedHandled = false;

        #endregion

        #region References

        protected Button PART_Up
        {
            get; set;
        }

        protected Button PART_Down
        {
            get; set;
        }

        #endregion

        #endregion

        #region UpDown

        public UpDown() : base()
        {
            this.DefaultStyleKey = typeof(UpDown);

            this.CommandBindings.Add(new CommandBinding(Up, this.Up_Executed, this.CanIncrease));
            this.CommandBindings.Add(new CommandBinding(Down, this.Down_Executed, this.CanDecrease));

            this.ResetTimer();
        }

        #endregion

        #region Methods

        #region Abstract

        protected abstract bool CanDecrease();

        protected abstract bool CanIncrease();

        public abstract void Decrease();

        public abstract void Increase();

        #endregion

        #region Commands

        public static readonly RoutedUICommand Down = new RoutedUICommand("Down", "Down", typeof(UpDown));
        protected void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.IsUpDownEnabled)
                this.Decrease();
        }
        void CanDecrease(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CanDecrease();
        }

        public static readonly RoutedUICommand Up = new RoutedUICommand("Up", "Up", typeof(UpDown));
        protected void Up_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.IsUpDownEnabled)
                this.Increase();
        }
        void CanIncrease(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CanIncrease();
        }

        #endregion

        #region Events

        void OnMajorChange(UpDownDirection Direction)
        {
            if (Direction == UpDownDirection.Up)
                this.Increase();
            else if (Direction == UpDownDirection.Down)
                this.Decrease();
        }

        protected virtual void OnMajorChange(object sender, EventArgs e)
        {
            this.Timer.Milliseconds += this.Timer.Interval.TotalMilliseconds;
            if (this.Timer.Milliseconds < this.MajorChangeDelay) return;
            this.OnMajorChange(this.Timer.Direction);
        }

        void OnUpDownButtonPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Timer.Direction = sender.As<MaskedButton>().Tag.ToString().ParseEnum<UpDownDirection>();
            this.Timer.Start();
        }

        void OnUpDownButtonPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Timer.Stop();
            this.Timer.Milliseconds = 0.0;
            this.Timer.Direction = UpDownDirection.None;
        }

        #endregion

        #region Protected

        /// <summary>
        /// Set text; string format should be applied prior to calling.
        /// </summary>
        protected void SetText(string NewText)
        {
            int CaretIndex = this.CaretIndex;
            this.Text = NewText;
            this.CaretIndex = CaretIndex;
        }

        void ResetTimer()
        {
            this.Timer = new UpDownTimer();
            this.Timer.Interval = TimeSpan.FromMilliseconds(this.MajorChange);
            this.Timer.Tick += OnMajorChange;
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_Up = this.Template.FindName("PART_Up", this).As<Button>();
            this.PART_Up.PreviewMouseDown += OnUpDownButtonPreviewMouseDown;
            this.PART_Up.PreviewMouseUp += OnUpDownButtonPreviewMouseUp;
            this.PART_Up.MouseDoubleClick += (s, e) => e.Handled = true;
            this.PART_Up.MouseDown += (s, e) => e.Handled = true;
            this.PART_Up.MouseUp += (s, e) => e.Handled = true;

            this.PART_Down = this.Template.FindName("PART_Down", this).As<Button>();
            this.PART_Down.PreviewMouseDown += OnUpDownButtonPreviewMouseDown;
            this.PART_Down.PreviewMouseUp += OnUpDownButtonPreviewMouseUp;
            this.PART_Down.MouseDoubleClick += (s, e) => e.Handled = true;
            this.PART_Down.MouseDown += (s, e) => e.Handled = true;
            this.PART_Down.MouseUp += (s, e) => e.Handled = true;
        }

        /// <remarks>
        /// Focus is obtained in the base version of this method.
        /// Because UpDown contains nonfocusable buttons, we don't 
        /// wish to capture focus when parent (relative to actual 
        /// clicked visual) is a button.
        /// </remarks>
        protected override bool OnPreviewMouseLeftButtonDownHandled(MouseButtonEventArgs e)
        {
            var Parent = e.OriginalSource.As<DependencyObject>();
            
            while (!Parent.Is<UpDown>())
            {
                Parent = Parent.GetParent();
                if (Parent.Is<Button>())
                    break;
            }
            return Parent.Is<Button>();
        }

        #endregion

        #region INotifyPropertyChanged

        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Virtual

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
                this.Milliseconds = 0.0;
            }
        }

        #endregion
    }
}
