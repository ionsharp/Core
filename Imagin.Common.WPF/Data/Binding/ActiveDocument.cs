using Imagin.Common.Models;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class ActiveDocumentBinding : Binding
    {
        new public string Path
        {
            set => base.Path = new PropertyPath($"ActiveDocument{(value?.Length > 0 ? $".{value}" : "")}");
        }

        public ActiveDocumentBinding() : base() { }

        public ActiveDocumentBinding(string path) : base()
        {
            Path = path;
            Source = Get.Where<IDockViewModel>();
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }
    }
}