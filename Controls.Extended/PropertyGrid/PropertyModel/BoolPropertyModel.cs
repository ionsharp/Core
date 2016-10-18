namespace Imagin.Controls.Extended
{
    public sealed class BoolPropertyModel : PropertyModel
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null) 
                this.Info.SetValue(SelectedObject, NewValue == null ? false : (bool)NewValue, null);
        }

        public BoolPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
