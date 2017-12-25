using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public abstract class ParseBox<TType> : TextBox
    {
        bool textChangeHandled { get; set; } = false;

        bool valueChangeHandled { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty<TType, ParseBox<TType>> ValueProperty = new DependencyProperty<TType, ParseBox<TType>>("Value", new FrameworkPropertyMetadata(default(TType), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnValueCoerced));
        /// <summary>
        /// 
        /// </summary>
        public TType Value
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
            d.As<ParseBox<TType>>().OnValueChanged((TType)e.NewValue);
        }
        static object OnValueCoerced(DependencyObject d, object Value)
        {
            return d.As<ParseBox<TType>>().OnValueCoerced(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public ParseBox() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetText(string text)
        {
            var i = CaretIndex;
            SetCurrentValue(TextProperty, text);
            CaretIndex = i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract TType GetValue(string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected abstract string ToString(TType Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!textChangeHandled)
            {
                valueChangeHandled = true;
                SetCurrentValue(ValueProperty.Property, GetValue(Text));

                var i = ToString(Value);
                if (Text != i)
                {
                    textChangeHandled = true;
                    SetCurrentValue(TextProperty, i);
                    textChangeHandled = false;
                }

                valueChangeHandled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnValueChanged(TType value)
        {
            if (!valueChangeHandled)
            {
                textChangeHandled = true;
                SetText(ToString(Value));
                textChangeHandled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual object OnValueCoerced(object value)
        {
            return value;
        }
    }
}
