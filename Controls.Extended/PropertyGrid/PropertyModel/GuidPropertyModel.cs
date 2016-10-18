using System;

namespace Imagin.Controls.Extended
{
    public sealed class GuidPropertyModel : PropertyModel
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null) 
                this.Info.SetValue(SelectedObject, NewValue == null ? default(Guid) : (Guid)NewValue, null);
        }

        public GuidPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
