using Imagin.Common.Linq;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A chip for displaying and selecting a LinearGradientBrush.
    /// </summary>
    public class GradientChip : Chip<Brush>
    {
        /// <summary>
        /// 
        /// </summary>
        public GradientChip() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool? ShowDialog()
        {
            var Dialog = new GradientDialog(Title, Value.As<Brush>().Duplicate(), this);
            var Result = Dialog.ShowDialog();

            if (Result.Value || Dialog.IsCancel)
                Value = Dialog.InitialValue;
            else if (!IsSynchronized)
                Value = Dialog.Value;

            return Result;
        }
    }
}
