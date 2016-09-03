namespace Imagin.Controls.Extended
{
    public class StringPropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? string.Empty : NewValue.ToString(), null);
        }

        public StringPropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsFeatured = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsFeatured)
        {
            this.Type = PropertyType.String;
        }
    }
}
