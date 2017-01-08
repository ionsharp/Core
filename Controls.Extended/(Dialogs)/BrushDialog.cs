using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public abstract class BrushDialog : BrushDialogBase<Brush>
    {
        public BrushDialog() : base()
        {
        }

        public BrushDialog(string title, Brush initialValue, Chip<Brush> chip = null) : base(title, initialValue, chip)
        {
        }
    }
}
