namespace Imagin.Controls.Extended
{
    public sealed class FileSystemObjectPropertyItem : StringPropertyItem
    {
        public FileSystemObjectPropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsPrimary = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsPrimary)
        {
            this.Type = PropertyType.FileSystemObject;
        }
    }
}

