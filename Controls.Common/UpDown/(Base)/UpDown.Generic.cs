using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    public abstract class UpDown<T> : UpDown, IUpDown<T>
    {
        #region Properties

        /// <summary>
        /// The absolute maximum value possible.
        /// </summary>
        public abstract T AbsoluteMaximum
        {
            get;
        }

        /// <summary>
        /// The absolute minimum value possible.
        /// </summary>
        public abstract T AbsoluteMinimum
        {
            get;
        }

        /// <summary>
        /// The default value.
        /// </summary>
        public abstract T DefaultValue
        {
            get;
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(T), typeof(UpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMaximumChanged, new CoerceValueCallback(OnMaximumCoerced)));
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
        static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<UpDown<T>>().OnMaximumChanged((T)e.NewValue);
        }
        static object OnMaximumCoerced(DependencyObject d, object Value)
        {
            return d.As<UpDown<T>>().OnMaximumCoerced(Value);
        }

        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(T), typeof(UpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMinimumChanged, new CoerceValueCallback(OnMinimumCoerced)));
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
        static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<UpDown<T>>().OnMinimumChanged((T)e.NewValue);
        }
        static object OnMinimumCoerced(DependencyObject Object, object Value)
        {
            return Object.As<UpDown<T>>().OnMinimumCoerced(Value);
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
        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<UpDown<T>>().OnValueChanged();
        }
        static object OnValueCoerced(DependencyObject d, object Value)
        {
            return d.As<UpDown<T>>().OnValueCoerced(Value);
        }

        #endregion

        #region UpDown

        /// <summary>
        /// Initializes a new instance of the <see cref="UpDown"/> class.
        /// </summary>
        public UpDown() : base()
        {
            Minimum = AbsoluteMinimum;
            Maximum = AbsoluteMaximum;
            Value = DefaultValue;
        }

        #endregion

        #region Methods

        protected abstract T GetValue(string Value);

        protected abstract object OnMaximumCoerced(object NewValue);

        protected abstract object OnMinimumCoerced(object NewValue);

        protected abstract object OnValueCoerced(object NewValue);

        protected abstract string ToString(T Value);

        protected override void OnStringFormatChanged()
        {
            OnValueChanged();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            OnTextChanged();
        }

        protected virtual void OnMaximumChanged(T Value)
        {
        }

        protected virtual void OnMinimumChanged(T Value)
        {
        }

        protected virtual bool OnTextChanged()
        {
            var Result = false;
            if (OnTextChangedHandled)
                OnTextChangedHandled = false;

            Result = OnValueChangedHandled = true;

            if (Result)
                Value = GetValue(Text);

            return Result;
        }

        protected virtual bool OnValueChanged()
        {
            var Result = false;
            if (OnValueChangedHandled)
                OnValueChangedHandled = false;

            Result = OnTextChangedHandled = true;

            if (Result)
                SetText(ToString(Value));

            return Result;
        }

        /// <summary>
        /// Set text; string format is not applied to value.
        /// </summary>
        protected void SetText(T Value)
        {
            SetText(Value.ToString());
        }

        #endregion
    }
}
