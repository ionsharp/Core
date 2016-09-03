using System;

namespace Imagin.Controls.Extended
{
    public sealed class LongPropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? 0L : Convert.ToInt64(NewValue), null);
        }

        public LongPropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsFeatured = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsFeatured)
        {
            this.Type = PropertyType.Long;
        }
    }
}
