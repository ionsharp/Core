using Imagin.Common.Configuration;
using Imagin.Common.Globalization;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalizedTextBlock : ContentControl
    {
        TextBlock textBlock;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty LocalizedTextProperty = DependencyProperty.Register("LocalizedText", typeof(string), typeof(LocalizedTextBlock), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string LocalizedText
        {
            get
            {
                return (string)GetValue(LocalizedTextProperty);
            }
            private set
            {
                SetValue(LocalizedTextProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(object), typeof(LocalizedTextBlock), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));
        /// <summary>
        /// 
        /// </summary>
        public object Text
        {
            get
            {
                return GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<LocalizedTextBlock>().OnTextChanged(e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextStyleProperty = DependencyProperty.Register("TextStyle", typeof(Style), typeof(LocalizedTextBlock), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextStyleChanged));
        /// <summary>
        /// 
        /// </summary>
        public Style TextStyle
        {
            get
            {
                return (Style)GetValue(TextStyleProperty);
            }
            set
            {
                SetValue(TextStyleProperty, value);
            }
        }
        static void OnTextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<LocalizedTextBlock>().OnTextStyleChanged((Style)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(LocalizedTextBlock), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public TextTrimming TextTrimming
        {
            get
            {
                return (TextTrimming)GetValue(TextTrimmingProperty);
            }
            set
            {
                SetValue(TextTrimmingProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(LocalizedTextBlock), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public TextWrapping TextWrapping
        {
            get
            {
                return (TextWrapping)GetValue(TextWrappingProperty);
            }
            set
            {
                SetValue(TextWrappingProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LocalizedTextBlock() : base()
        {
            textBlock = new TextBlock();

            if (TextStyle != null)
                textBlock.Style = TextStyle;

            textBlock.SetBinding(TextBlock.TextProperty, new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("LocalizedText"),
                Source = this
            });
            textBlock.SetBinding(TextBlock.TextTrimmingProperty, new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("TextTrimming"),
                Source = this
            });
            textBlock.SetBinding(TextBlock.TextWrappingProperty, new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("TextWrapping"),
                Source = this
            });

            SetCurrentValue(ContentProperty, textBlock);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current is IApp)
                Application.Current.As<IApp>().Languages.Set += OnLanguageSet;

            OnTextChanged(Text);
        }

        void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current is IApp)
                Application.Current.As<IApp>().Languages.Set -= OnLanguageSet;
        }

        void OnLanguageSet(object sender, EventArgs<CultureInfo> e)
        {
            OnTextChanged(Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnTextChanged(object Value)
        {
            var text = Value?.ToString();
            var result = text;

            if (text?.Length > 0)
            {
                result = Localizer.GetValue<string>(text);

                if (result.IsNullOrEmpty())
                    result = text;
            }

            SetCurrentValue(LocalizedTextProperty, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnTextStyleChanged(Style Value)
        {
            if (Value != null && textBlock != null)
                textBlock.Style = Value;
        }
    }
}
