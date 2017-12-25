using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SolidColorBrushDialog : BrushDialogBase<SolidColorBrush>
    {
        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrushDialog() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="initialValue"></param>
        /// <param name="chip"></param>
        public SolidColorBrushDialog(string title, SolidColorBrush initialValue, Chip<SolidColorBrush> chip = null) : base(title, initialValue, chip)
        {
        }
    }
}
