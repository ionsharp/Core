using System;

namespace Imagin.Controls.Extended
{
    public sealed class DecimalPropertyModel : NumericPropertyModel<decimal>
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null) 
                this.Info.SetValue(SelectedObject, NewValue == null ? 0m : (decimal)NewValue, null);
        }

        public DecimalPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
