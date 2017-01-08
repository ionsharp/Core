using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Extended
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
            var Initial = Value.Duplicate();

            var Dialog = new GradientDialog(Title, Initial, this);
            var Result = Dialog.ShowDialog();

            if (Result.Value || Dialog.Cancel)
                Value = Initial;
            else if (!IsSynchronized)
                Value = Dialog.Gradient;

            return Result;
        }
    }
}
