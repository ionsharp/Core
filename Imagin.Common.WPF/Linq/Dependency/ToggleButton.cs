using Imagin.Common.Converters;
using Imagin.Common.Numbers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Linq
{
    [Extends(typeof(ToggleButton))]
    public static class XToggleButton
    {
        public static readonly ResourceKey<ToggleButton> StyleKey = new();

        #region BulletSize

        public static readonly DependencyProperty BulletSizeProperty = DependencyProperty.RegisterAttached("BulletSize", typeof(DoubleSize), typeof(XToggleButton), new FrameworkPropertyMetadata(null));
        [TypeConverter(typeof(DoubleSizeTypeConverter))]
        public static DoubleSize GetBulletSize(ToggleButton i) => (DoubleSize)i.GetValue(BulletSizeProperty);
        public static void SetBulletSize(ToggleButton i, DoubleSize value) => i.SetValue(BulletSizeProperty, value);

        #endregion

        #region BulletTemplate

        public static readonly DependencyProperty BulletTemplateProperty = DependencyProperty.RegisterAttached("BulletTemplate", typeof(DataTemplate), typeof(XToggleButton), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetBulletTemplate(ToggleButton i) => (DataTemplate)i.GetValue(BulletTemplateProperty);
        public static void SetBulletTemplate(ToggleButton i, DataTemplate value) => i.SetValue(BulletTemplateProperty, value);

        #endregion
    }
}