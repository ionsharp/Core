using System;

namespace Imagin.Controls.Extended
{
    public sealed class DoublePropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? 0d : Convert.ToDouble(NewValue), null);
        }

        public DoublePropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsPrimary = false) : base(SelectedObject, Name, Value, Category, IsReadOnly)
        {
            this.Type = PropertyType.Double;
        }
    }
}
