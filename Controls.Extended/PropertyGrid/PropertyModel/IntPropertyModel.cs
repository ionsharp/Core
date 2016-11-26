using System;

namespace Imagin.Controls.Extended
{
    public sealed class IntPropertyModel : NumericPropertyModel<int>
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? 0 : Convert.ToInt32(NewValue), null);
        }

        public IntPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
