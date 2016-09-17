using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A StackPanel that applies spacing to each of it's items.
    /// </summary>
    public class Spacer : StackPanel
    {
        public static DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(Spacer), new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHorizontalContentAlignmentChanged));
        public HorizontalAlignment HorizontalContentAlignment
        {
            get
            {
                return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
            }
            set
            {
                SetValue(HorizontalContentAlignmentProperty, value);
            }
        }
        static void OnHorizontalContentAlignmentChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Spacer Spacer = Object.As<Spacer>();
            Spacer.RegisterSetHorizontalContentAlignment();
        }

        public static DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(Spacer), new FrameworkPropertyMetadata(VerticalAlignment.Top, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnVerticalContentAlignmentChanged));
        public VerticalAlignment VerticalContentAlignment
        {
            get
            {
                return (VerticalAlignment)GetValue(VerticalContentAlignmentProperty);
            }
            set
            {
                SetValue(VerticalContentAlignmentProperty, value);
            }
        }
        static void OnVerticalContentAlignmentChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Spacer Spacer = Object.As<Spacer>();
            Spacer.RegisterSetVerticalContentAlignment();
        }

        public static DependencyProperty SpacingProperty = DependencyProperty.Register("Spacing", typeof(Thickness), typeof(Spacer), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSpacingChanged));
        public Thickness Spacing
        {
            get
            {
                return (Thickness)GetValue(SpacingProperty);
            }
            set
            {
                SetValue(SpacingProperty, value);
            }
        }
        static void OnSpacingChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Spacer Spacer = Object.As<Spacer>();
            Spacer.RegisterSetPadding();
        }

        public static DependencyProperty TrimFirstProperty = DependencyProperty.Register("TrimFirst", typeof(bool), typeof(Spacer), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTrimFirstChanged));
        public bool TrimFirst
        {
            get
            {
                return (bool)GetValue(TrimFirstProperty);
            }
            set
            {
                SetValue(TrimFirstProperty, value);
            }
        }
        static void OnTrimFirstChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Spacer Spacer = Object.As<Spacer>();
            Spacer.RegisterSetPadding();
        }

        public static DependencyProperty TrimLastProperty = DependencyProperty.Register("TrimLast", typeof(bool), typeof(Spacer), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTrimLastChanged));
        public bool TrimLast
        {
            get
            {
                return (bool)GetValue(TrimLastProperty);
            }
            set
            {
                SetValue(TrimLastProperty, value);
            }
        }
        static void OnTrimLastChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Spacer Spacer = Object.As<Spacer>();
            Spacer.RegisterSetPadding();
        }

        void RegisterSetPadding()
        {
            this.LayoutUpdated -= SetPadding;
            this.LayoutUpdated += SetPadding;
        }

        void RegisterSetHorizontalContentAlignment()
        {
            this.LayoutUpdated -= SetHorizontalContentAlignment;
            this.LayoutUpdated += SetHorizontalContentAlignment;
        }

        void RegisterSetVerticalContentAlignment()
        {
            this.LayoutUpdated -= SetVerticalContentAlignment;
            this.LayoutUpdated += SetVerticalContentAlignment;
        }

        void SetPadding()
        {
            for (int i = 0, Count = this.Children.Count; i < Count; i++)
            {
                FrameworkElement Element = this.Children[i] as FrameworkElement;
                if ((i == 0 && TrimFirst) || (i == (Count - 1) && TrimLast))
                {
                    Element.Margin = new Thickness(0);
                    continue;
                }
                Element.Margin = this.Spacing;
            }
        }

        void SetPadding(object sender, System.EventArgs e)
        {
            this.SetPadding();
        }

        void SetHorizontalContentAlignment()
        {
            for (int i = 0, Count = this.Children.Count; i < Count; i++)
            {
                FrameworkElement Element = this.Children[i] as FrameworkElement;
                Element.HorizontalAlignment = this.HorizontalContentAlignment;
            }
        }

        void SetHorizontalContentAlignment(object sender, System.EventArgs e)
        {
            this.SetHorizontalContentAlignment();
        }

        void SetVerticalContentAlignment()
        {
            for (int i = 0, Count = this.Children.Count; i < Count; i++)
            {
                FrameworkElement Element = this.Children[i] as FrameworkElement;
                Element.VerticalAlignment = this.VerticalContentAlignment;
            }
        }

        void SetVerticalContentAlignment(object sender, System.EventArgs e)
        {
            this.SetVerticalContentAlignment();
        }

        public Spacer()
        {
        }
    }
}
