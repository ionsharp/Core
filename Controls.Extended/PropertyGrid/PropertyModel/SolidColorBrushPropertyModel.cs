using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public sealed class SolidColorBrushPropertyModel : PropertyModel
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? default(SolidColorBrush) : (SolidColorBrush)(new BrushConverter().ConvertFrom(NewValue)), null);
        }

        public SolidColorBrushPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
