namespace Imagin.Controls.Extended
{
    public sealed class ShortPropertyModel : NumericPropertyModel<short>
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null) 
                this.Info.SetValue(SelectedObject, NewValue == null ? (short)0 : (short)NewValue, null);
        }

        public ShortPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
