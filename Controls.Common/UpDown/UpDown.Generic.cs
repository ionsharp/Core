using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    public abstract class UpDown<T> : UpDown, IUpDown<T>
    {
        #region Properties

        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(T), typeof(UpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, new CoerceValueCallback(OnMinimumCoerced)));
        public T Minimum
        {
            get
            {
                return (T)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }
        static object OnMinimumCoerced(DependencyObject Object, object Value)
        {
            return Object.As<UpDown<T>>().OnMinimumCoerced(Value);
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(T), typeof(UpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, new CoerceValueCallback(OnMaximumCoerced)));
        public T Maximum
        {
            get
            {
                return (T)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }
        static object OnMaximumCoerced(DependencyObject Object, object Value)
        {
            return Object.As<UpDown<T>>().OnMaximumCoerced(Value);
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(UpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, new CoerceValueCallback(OnValueCoerced)));
        public T Value
        {
            get
            {
                return (T)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        static void OnValueChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpDown<T> UpDown = (UpDown<T>)Object;
            UpDown.OnValueChanged();
        }
        static object OnValueCoerced(DependencyObject Object, object Value)
        {
            return Object.As<UpDown<T>>().OnValueCoerced(Value);
        }

        #endregion

        #region Methods

        #region Abstract

        protected abstract object OnMaximumCoerced(object NewValue);

        protected abstract object OnMinimumCoerced(object NewValue);

        protected abstract object OnValueCoerced(object NewValue);

        #endregion

        #region Overrides

        protected sealed override void OnStringFormatChanged()
        {
            this.OnValueChanged();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            this.OnTextChanged();
        }

        #endregion

        #region Protected

        /// <summary>
        /// Set text; string format is not applied to value.
        /// </summary>
        protected void SetText(T Value)
        {
            this.SetText(Value.ToString());
        }

        #endregion

        #region Virtual

        protected virtual bool OnTextChanged()
        {
            if (this.OnTextChangedHandled)
                return this.OnTextChangedHandled = false;
            this.OnValueChangedHandled = true;
            return true;
        }

        protected virtual bool OnValueChanged()
        {
            if (this.OnValueChangedHandled)
                return this.OnValueChangedHandled = false;
            this.OnTextChangedHandled = true;
            return true;
        }

        #endregion

        #endregion

        #region UpDown<T>

        public UpDown() : base()
        {
        }

        #endregion
    }
}
