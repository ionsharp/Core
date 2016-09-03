namespace Imagin.Controls.Extended
{
    public sealed class BoolPropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null) 
                this.Info.SetValue(SelectedObject, NewValue == null ? false : (bool)NewValue, null);
        }

        public BoolPropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsFeatured = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsFeatured)
        {
            this.Type = PropertyType.Bool;
        }
    }
}
