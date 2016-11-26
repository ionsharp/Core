namespace Imagin.Controls.Extended
{
    public sealed class BytePropertyModel : NumericPropertyModel<byte>
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null) 
                this.Info.SetValue(SelectedObject, NewValue == null ? (byte)0 : (byte)NewValue, null);
        }

        public BytePropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
