using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A StackPanel that applies spacing to each of it's items.
    /// </summary>
    public class Spacer : StackPanel
    {
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
        private static void OnSpacingChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((Spacer)Object).SetPadding();
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
        private static void OnTrimFirstChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((Spacer)Object).SetPadding();
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
        private static void OnTrimLastChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((Spacer)Object).SetPadding();
        }

        private void SetPadding()
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

        public Spacer()
        {
            this.LayoutUpdated += OnLayoutUpdated;
        }

        private void OnLayoutUpdated(object sender, System.EventArgs e)
        {
            this.SetPadding();
        }
    }
}
