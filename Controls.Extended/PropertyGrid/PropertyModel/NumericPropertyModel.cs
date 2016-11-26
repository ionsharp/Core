namespace Imagin.Controls.Extended
{
    public abstract class NumericPropertyModel : PropertyModel
    {
        internal abstract void SetConstraint(object Minimum, object Maximum);

        public NumericPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
