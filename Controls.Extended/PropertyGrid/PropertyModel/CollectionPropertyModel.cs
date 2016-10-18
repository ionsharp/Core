namespace Imagin.Controls.Extended
{
    public sealed class CollectionPropertyModel : PropertyModel
    {
        protected override void OnValueChanged(object NewValue)
        {
        }

        public CollectionPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
