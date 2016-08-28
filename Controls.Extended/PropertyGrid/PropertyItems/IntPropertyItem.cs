using System;

namespace Imagin.Controls.Extended
{
    public sealed class IntPropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? 0 : Convert.ToInt32(NewValue), null);
        }

        public IntPropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsPrimary = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsPrimary)
        {
            this.Type = PropertyType.Int;
        }
    }
}
