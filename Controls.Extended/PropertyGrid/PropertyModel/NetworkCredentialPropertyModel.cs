namespace Imagin.Controls.Extended
{
    public sealed class NetworkCredentialPropertyModel : PropertyModel
    {
        protected override void OnValueChanged(object NewValue)
        {
        }

        public NetworkCredentialPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
