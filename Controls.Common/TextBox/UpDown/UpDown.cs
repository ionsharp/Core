using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public abstract class UpDown : AdvancedTextBox, INotifyPropertyChanged
    {
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
            UpDown.OnStringFormatChanged(e);
        }

        public UpDown() : base()
        {
            this.DefaultStyleKey = typeof(UpDown);
            this.CommandBindings.Add(new CommandBinding(Up, Up_Executed, Up_CanExecute));
            this.CommandBindings.Add(new CommandBinding(Down, Down_Executed, Down_CanExecute));
        }

        public static readonly RoutedUICommand Up = new RoutedUICommand("Up", "Up", typeof(UpDown));
        protected abstract void Up_Executed(object sender, ExecutedRoutedEventArgs e);
        protected abstract void Up_CanExecute(object sender, CanExecuteRoutedEventArgs e);
        
        public static readonly RoutedUICommand Down = new RoutedUICommand("Down", "Down", typeof(UpDown));
        protected abstract void Down_Executed(object sender, ExecutedRoutedEventArgs e);
        protected abstract void Down_CanExecute(object sender, CanExecuteRoutedEventArgs e);

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

        /// <summary>
        /// Applies control-specific string format.
        /// </summary>
        /// <param name="StringFormat">The current [StringFormat]</param>
        /// <returns>[Text] with control-specific [StringFormat] applied.</returns>
        protected abstract void FormatValue(string StringFormat);

        protected virtual void Trim(string NewText)
        {
            this.SetText(NewText, true);
        }

        protected virtual void OnStringFormatChanged(DependencyPropertyChangedEventArgs e)
        {
            this.FormatValue(this.StringFormat);
        }

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
            this.FormatValue(this.StringFormat);
            this.OnPropertyChanged("Value");
        }

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
    }
}
