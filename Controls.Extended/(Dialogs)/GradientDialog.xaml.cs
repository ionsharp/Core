using Imagin.Common.Input;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class GradientDialog : BrushDialog
    {
        protected override string DefaultTitle
        {
            get
            {
                return "Select Gradient";
            }
        }

        public GradientDialog() : this(string.Empty, Brushes.Transparent, null)
        {
        }

        public GradientDialog(string title, Brush initialValue, Chip<Brush> chip = null) : base(title, initialValue, chip)
        {
            InitializeComponent();
            PART_GradientPicker.Gradient = initialValue;
        }

        protected override void OnRevert(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnGradientChanged(object sender, EventArgs<Brush> e)
        {
            Value = e.Value;

            if (Chip != null && Chip.IsSynchronized)
                Chip.Value = e.Value;
        }
    }
}
