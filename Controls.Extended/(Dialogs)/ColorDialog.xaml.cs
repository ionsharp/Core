using Imagin.Common.Input;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class ColorDialog : SolidColorBrushDialog
    {
        protected override string DefaultTitle
        {
            get
            {
                return "Select Color";
            }
        }

        public ColorDialog() : this(string.Empty, Brushes.Transparent, null)
        {
        }

        public ColorDialog(string title, SolidColorBrush initialValue, Chip<SolidColorBrush> chip = null) : base(title, initialValue, chip)
        {
            InitializeComponent();
            PART_ColorPicker.InitialColor = InitialValue.Color;
        }

        protected override void OnRevert(object sender, RoutedEventArgs e)
        {
            PART_ColorPicker.SelectedColor = PART_ColorPicker.InitialColor;
        }

        protected virtual void OnSelectedColorChanged(object sender, EventArgs<Color> e)
        {
            var v = new SolidColorBrush(e.Value);

            Value = v;

            if (Chip != null && Chip.IsSynchronized)
                Chip.Value = v;
        }
    }
}
