using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public sealed class SolidColorPropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? default(SolidColorBrush) : (SolidColorBrush)NewValue, null);
        }

        public SolidColorPropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsPrimary = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsPrimary)
        {
            this.Type = PropertyType.SolidColorBrush;
        }
    }
}
