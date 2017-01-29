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
        public static readonly DependencyProperty<T, UpDown<T>> MaximumProperty = new DependencyProperty<T, UpDown<T>>("Maximum", new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMaximumChanged, OnMaximumCoerced));
        /// <summary>
        /// 
        /// </summary>
        public T Maximum
        {
            get
            {
                return MaximumProperty.Get(this);
            }
            set
            {
                MaximumProperty.Set(this, value);
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
        public static readonly DependencyProperty<T, UpDown<T>> MinimumProperty = new DependencyProperty<T, UpDown<T>>("Minimum", new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMinimumChanged, OnMinimumCoerced));
        /// <summary>
        /// 
        /// </summary>
        public T Minimum
        {
            get
            {
                return MinimumProperty.Get(this);
            }
            set
            {
                MinimumProperty.Set(this, value);
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
        public static readonly DependencyProperty<T, UpDown<T>> ValueProperty = new DependencyProperty<T, UpDown<T>>("Value", new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnValueCoerced));
        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get
            {
                return ValueProperty.Get(this);
            }
            set
            {
                ValueProperty.Set(this, value);
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
            SetCurrentValue(MaximumProperty.Property, AbsoluteMaximum);
            SetCurrentValue(MinimumProperty.Property, AbsoluteMinimum);
            SetCurrentValue(ValueProperty.Property, DefaultValue);
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
