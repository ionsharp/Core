using System;

namespace Imagin.Controls.Extended
{
    public sealed class DateTimePropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? default(DateTime) : (DateTime)NewValue, null);
        }

        public DateTimePropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsPrimary = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsPrimary)
        {
            this.Type = PropertyType.DateTime;
        }
    }
}
