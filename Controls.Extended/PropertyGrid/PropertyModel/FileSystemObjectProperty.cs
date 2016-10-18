namespace Imagin.Controls.Extended
{
    public sealed class FileSystemObjectPropertyItem : StringPropertyItem
    {
        public FileSystemObjectPropertyItem(object SelectedObject, string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured = false) : base(SelectedObject, Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
            this.Type = PropertyType.FileSystemObject;
        }
    }
}

