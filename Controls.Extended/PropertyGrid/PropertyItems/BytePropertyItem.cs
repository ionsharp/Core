namespace Imagin.Controls.Extended
{
    public sealed class BytePropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null) 
                this.Info.SetValue(SelectedObject, NewValue == null ? (byte)0 : (byte)NewValue, null);
        }

        public BytePropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsFeatured = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsFeatured)
        {
            this.Type = PropertyType.Byte;
        }
    }
}
