using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public sealed class LinearGradientPropertyModel : PropertyModel
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? default(LinearGradientBrush) : (LinearGradientBrush)NewValue, null);
        }

        public LinearGradientPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
