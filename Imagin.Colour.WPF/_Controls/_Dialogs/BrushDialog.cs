using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BrushDialog : BrushDialogBase<Brush>
    {
        /// <summary>
        /// 
        /// </summary>
        public BrushDialog() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="initialValue"></param>
        /// <param name="chip"></param>
        public BrushDialog(string title, Brush initialValue, Chip<Brush> chip = null) : base(title, initialValue, chip) { }
    }
}
