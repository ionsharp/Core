using System;

namespace Imagin.Controls.Extended
{
    public sealed class LongPropertyModel : PropertyModel
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? 0L : Convert.ToInt64(NewValue), null);
        }

        public LongPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
