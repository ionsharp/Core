using System.Windows.Media;

namespace Imagin.Common.Mvvm
{
    public interface IPaneViewModel
    {
        string Title
        {
            get; set;
        }

        ImageSource Icon
        {
            get; set;
        }

        bool IsVisible
        {
            get; set;
        }
    }
}
