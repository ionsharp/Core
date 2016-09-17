using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class GradientDialog : Window
    {
        public LinearGradientBrush Gradient
        {
            get
            {
                return PART_GradientEditor.Gradient;
            }
            set
            {
                PART_GradientEditor.Gradient = value;
            }
        }

        public GradientChip GradientChip
        {
            get; set;
        }

        public bool Save = false;

        public bool Cancel = false;

        public GradientDialog()
        {
            InitializeComponent();
        }

        public GradientDialog(string Title, LinearGradientBrush InitialGradient, GradientChip GradientChip = null)
        {
            InitializeComponent();
            this.Title = Title;
            this.Gradient = InitialGradient;
            this.GradientChip = GradientChip;
        }

        void OnSave(object sender, RoutedEventArgs e)
        {
            this.Save = true;
            this.Close();
        }

        void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Cancel = true;
            this.Close();
        }
    }
}
