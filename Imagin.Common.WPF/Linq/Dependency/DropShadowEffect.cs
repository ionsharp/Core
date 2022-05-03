using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Imagin.Common.Linq
{
    public static class XDropShadowEffect
    {
        #region Brush

        public static readonly DependencyProperty BrushProperty = DependencyProperty.RegisterAttached("Brush", typeof(SolidColorBrush), typeof(XDropShadowEffect), new FrameworkPropertyMetadata(null, OnBrushChanged));
        public static SolidColorBrush GetBrush(DropShadowEffect i) => (SolidColorBrush)i.GetValue(BrushProperty);
        public static void SetBrush(DropShadowEffect i, SolidColorBrush input) => i.SetValue(BrushProperty, input);
        static void OnBrushChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DropShadowEffect effect)
                effect.Color = e.NewValue.As<SolidColorBrush>()?.Color ?? System.Windows.Media.Colors.Transparent;
        }

        #endregion
    }
}