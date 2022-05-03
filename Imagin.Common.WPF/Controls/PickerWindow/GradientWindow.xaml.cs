using Imagin.Common.Media;

namespace Imagin.Common.Controls
{
    public partial class GradientWindow : PickerWindow
    {
        public GradientWindow() : base()
        {
            SetCurrentValue(ValueProperty, Gradient.Default);
            InitializeComponent();
        }

        public GradientWindow(string title, Gradient gradient) : this()
        {
            SetCurrentValue(TitleProperty, 
                title);
            SetCurrentValue(ValueProperty, 
                gradient);
        }
    }
}