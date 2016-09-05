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

        DispatcherTimer Timer
        {
            get; set;
        }

        double MillisecondsTicked = 0.0;

        protected bool IgnoreTextChange = false;

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

        public static DependencyProperty MajorChangeProperty = DependencyProperty.Register("MajorChange", typeof(int), typeof(UpDown), new FrameworkPropertyMetadata(100, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMajorChangeChanged));
        public int MajorChange
        {
            get
            {
                return (int)GetValue(MajorChangeProperty);
            }
            set
            {
                SetValue(MajorChangeProperty, value);
            }
        }
        static void OnMajorChangeChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpDown UpDown = (UpDown)Object;
            if (UpDown.Timer != null)
            {
                UpDown.Timer.Stop();
                UpDown.Timer.Interval = TimeSpan.FromMilliseconds(UpDown.MajorChange);
            }
        }

        #endregion

        #region UpDown

        public UpDown() : base()
        {
            this.DefaultStyleKey = typeof(UpDown);
            this.CommandBindings.Add(new CommandBinding(Up, this.Up_Executed, this.CanIncrease));
            this.CommandBindings.Add(new CommandBinding(Down, this.Down_Executed, this.CanDecrease));

            this.Timer = new DispatcherTimer();
            this.Timer.Interval = TimeSpan.FromMilliseconds(this.MajorChange);
            this.Timer.Tick += OnTick;
        }

        #endregion

        #region Methods

        #region Abstract

        /// <summary>
        /// Gets current value as object.
        /// </summary>
        /// <returns>Current value as object.</returns>
        public abstract object GetValue();

        /// <summary>
        /// Coerces value to constraints.
        /// </summary>
        /// <param name="NewValue">The new value to constrain</param>
        protected abstract void CoerceValue(object NewValue);

        public abstract void Increase();

        public abstract void Decrease();

        #endregion

        #region Commands

        public static readonly RoutedUICommand Up = new RoutedUICommand("Up", "Up", typeof(UpDown));
        protected void Up_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Increase();
        }
        protected abstract void CanIncrease(object sender, CanExecuteRoutedEventArgs e);
        
        public static readonly RoutedUICommand Down = new RoutedUICommand("Down", "Down", typeof(UpDown));
        protected void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Decrease();
        }
        protected abstract void CanDecrease(object sender, CanExecuteRoutedEventArgs e);

        #endregion

        #region Events

        void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Timer.Tag = sender.As<MaskedButton>().Tag.ToString() == "1" ? true : false;
            Timer.Start();
        }

        void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Timer.Stop();
            this.MillisecondsTicked = 0.0;
            this.Timer.Tag = null;
        }

        void OnTick(object sender, EventArgs e)
        {
            this.MillisecondsTicked += Convert.ToDouble(this.Timer.Interval.TotalMilliseconds);
            if (this.MillisecondsTicked < 500.0)
                return;
            if ((bool)this.Timer.Tag)
                this.Increase();
            else this.Decrease();
        }

        #endregion

        #region Protected

        /// <summary>
        /// Sets text while preserving caret index.
        /// </summary>
        protected void SetText(object Value, bool IgnoreTextChange = false)
        {
            this.SetText(Value.ToString(), IgnoreTextChange);
        }

        /// <summary>
        /// Sets text while preserving caret index.
        /// </summary>
        protected void SetText(string NewText, bool IgnoreTextChange = false)
        {
            int CaretIndex = this.CaretIndex;
            this.IgnoreTextChange = IgnoreTextChange;
            this.Text = NewText;
            this.CaretIndex = CaretIndex;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// We want to do two things anytime text changes:
        /// 
        /// 1) Clip value to [Minimum] and [Maximum].
        /// 2) Update [Value] one-way bindings.
        /// </summary>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (this.IgnoreTextChange)
            {
                this.IgnoreTextChange = false;
                return;
            }

            this.Trim(this.Text);

            if (this.IsUpDownEnabled)
                this.CoerceValue(this.GetValue());

            this.FormatValue();

            this.OnPropertyChanged("Value");
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            MaskedButton PART_Up = this.Template.FindName("PART_Up", this).As<MaskedButton>();
            PART_Up.PreviewMouseDown += OnPreviewMouseDown;
            PART_Up.PreviewMouseUp += OnPreviewMouseUp;

            MaskedButton PART_Down = this.Template.FindName("PART_Down", this).As<MaskedButton>();
            PART_Down.PreviewMouseDown += OnPreviewMouseDown;
            PART_Down.PreviewMouseUp += OnPreviewMouseUp;
        }

        #endregion

        #region Virtual

        /// <summary>
        /// Applies control-specific string format.
        /// </summary>
        /// <param name="StringFormat">The current [StringFormat]</param>
        /// <returns>[Text] with control-specific [StringFormat] applied.</returns>
        protected virtual void FormatValue()
        {
        }

        protected virtual void Trim(string NewText)
        {
            this.SetText(NewText, true);
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

        #endregion
    }
}
