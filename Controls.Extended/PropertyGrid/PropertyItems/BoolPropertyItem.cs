namespace Imagin.Controls.Extended
{
    public sealed class BoolPropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null) 
                this.Info.SetValue(SelectedObject, NewValue == null ? false : (bool)NewValue, null);
        }

        public BoolPropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsPrimary = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsPrimary)
        {
            this.Type = PropertyType.Bool;
        }
    }
}
