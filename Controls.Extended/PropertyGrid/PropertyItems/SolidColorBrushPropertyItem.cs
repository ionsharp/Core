using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public sealed class SolidColorBrushPropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? default(SolidColorBrush) : (SolidColorBrush)(new BrushConverter().ConvertFrom(NewValue)), null);
        }

        public SolidColorBrushPropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsFeatured = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsFeatured)
        {
            this.Type = PropertyType.SolidColorBrush;
        }
    }
}
