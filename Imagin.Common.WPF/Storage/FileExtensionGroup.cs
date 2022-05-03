using Imagin.Common.Collections.ObjectModel;
using System.Collections.ObjectModel;

namespace Imagin.Common.Storage
{
    public class FileExtensionGroup : Base
    {
        public const string Wild = "*";

        public bool IsWild => fileExtensions.Contains("*");

        StringCollection fileExtensions = new();
        public StringCollection FileExtensions
        {
            get => fileExtensions;
            private set => this.Change(ref fileExtensions, value);
        }

        public FileExtensionGroup(params string[] fileExtensions)
        {
            if (fileExtensions?.Length > 0)
            {
                foreach (var i in fileExtensions)
                    FileExtensions.Add(i);
            }
        }
    }

    public class FileExtensionGroups : ObservableCollection<FileExtensionGroup>
    {
        public void Add(params string[] fileExtensions) => Add(new FileExtensionGroup(fileExtensions));
    }
}