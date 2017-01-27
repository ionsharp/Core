using Imagin.Common.Extensions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UpDown<T> : UpDown, IUpDown<T>
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        protected bool TextChangeHandled { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        protected bool ValueChangeHandled { get; set; } = false;

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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(T), typeof(UpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMaximumChanged, new CoerceValueCallback(OnMaximumCoerced)));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(T), typeof(UpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMinimumChanged, new CoerceValueCallback(OnMinimumCoerced)));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(UpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, new CoerceValueCallback(OnValueCoerced)));
        /// <summary>
        /// 
        /// </summary>
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
            d.As<UpDown<T>>().OnValueChanged((T)e.NewValue);
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
            SetCurrentValue(MaximumProperty, AbsoluteMaximum);
            SetCurrentValue(MinimumProperty, AbsoluteMinimum);
            SetCurrentValue(ValueProperty, DefaultValue);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected abstract T GetValue(string Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected abstract object OnMaximumCoerced(object Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected abstract object OnMinimumCoerced(object Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected abstract object OnValueCoerced(object Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected abstract string ToString(T Value);

        /// <summary>
        /// Occurs when the string format changes.
        /// </summary>
        protected override void OnStringFormatChanged()
        {
            OnValueChanged(Value);
        }

        /// <summary>
        /// Occurs when the text changes.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!TextChangeHandled)
            {
                ValueChangeHandled = true;
                Value = GetValue(Text);

                //If current value does not match up with text, make it so
                var i = ToString(Value);
                if (Text != i)
                {
                    TextChangeHandled = true;
                    Text = i;
                    TextChangeHandled = false;
                }

                ValueChangeHandled = false;
            }
        }

        /// <summary>
        /// Occurs when the maximum value changes.
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnMaximumChanged(T Value)
        {
        }

        /// <summary>
        /// Occurs when the minimum value changes.
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnMinimumChanged(T Value)
        {
        }

        /// <summary>
        /// Occurs when the value changes.
        /// </summary>
        protected virtual void OnValueChanged(T Value)
        {
            if (!ValueChangeHandled)
            {
                TextChangeHandled = true;
                SetText(ToString(Value));
                TextChangeHandled = false;
            }
        }

        #endregion
    }
}
