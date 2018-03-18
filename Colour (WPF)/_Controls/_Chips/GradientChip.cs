using Imagin.Common.Linq;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// A chip for displaying and selecting a <see cref="LinearGradientBrush"/> or <see cref="RadialGradientBrush"/>.
    /// </summary>
    public class GradientChip : Chip<Brush>
    {
        /// <summary>
        /// 
        /// </summary>
        public GradientChip() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool? ShowDialog()
        {
            var dialog = new GradientDialog(Title, Value.As<Brush>().Duplicate(), this);
            var result = dialog.ShowDialog();

            if (result.Value || dialog.IsCancel)
            {
                Value = dialog.InitialValue;
            }
            else if (!IsSynchronized)
                Value = dialog.Value;

            return result;
        }
    }
}