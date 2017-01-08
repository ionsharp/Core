using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public abstract class SolidColorBrushDialog : BrushDialogBase<SolidColorBrush>
    {
        public SolidColorBrushDialog() : base()
        {
        }

        public SolidColorBrushDialog(string title, SolidColorBrush initialValue, Chip<SolidColorBrush> chip = null) : base(title, initialValue, chip)
        {
        }
    }
}
