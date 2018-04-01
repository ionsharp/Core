using Imagin.Common.Input;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GradientDialog : BrushDialog
    {
        /// <summary>
        /// 
        /// </summary>
        protected override string DefaultTitle => "Select Gradient";

        /// <summary>
        /// 
        /// </summary>
        public GradientDialog() : this(string.Empty, Brushes.Transparent, null) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="initialValue"></param>
        /// <param name="chip"></param>
        public GradientDialog(string title, Brush initialValue, Chip<Brush> chip = null) : base(title, initialValue, chip)
        {
            InitializeComponent();
            PART_GradientPicker.Gradient = initialValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnRevert(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnGradientChanged(object sender, EventArgs<Brush> e)
        {
            Value = e.Value;

            if (Chip != null && Chip.IsSynchronized)
                Chip.Value = e.Value;
        }
    }
}
