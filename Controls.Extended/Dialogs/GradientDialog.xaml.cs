using Imagin.Controls.Common;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class GradientDialog : BasicWindow
    {
        public Brush Gradient
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

        public GradientDialog() : base()
        {
            InitializeComponent();
        }

        public GradientDialog(string title, Brush initialGradient, GradientChip gradientChip = null)
        {
            InitializeComponent();

            Title = title;
            Gradient = initialGradient;
            GradientChip = gradientChip;
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
