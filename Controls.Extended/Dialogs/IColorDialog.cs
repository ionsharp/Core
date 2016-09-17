using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public interface IColorDialog
    {
        Color SelectedColor
        {
            get; set;
        }

        Color InitialColor
        {
            get; set;
        }

        bool? ShowDialog();
    }
}
