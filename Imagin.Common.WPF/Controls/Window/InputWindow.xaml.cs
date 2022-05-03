using Imagin.Common.Linq;
using System.Windows;

namespace Imagin.Common.Controls
{
    public partial class InputWindow : Window
    {
        public static readonly DependencyProperty InputProperty = DependencyProperty.Register(nameof(Input), typeof(string), typeof(InputWindow), new FrameworkPropertyMetadata(string.Empty));
        public string Input
        {
            get => (string)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(InputWindow), new FrameworkPropertyMetadata(string.Empty));
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public InputWindow() : base()
        {
            XWindow.SetFooterButtons(this, new Buttons(this, Buttons.SaveCancel));
            InitializeComponent();
        }

        public InputWindow(string title, string input, string placeholder) : this()
        {
            SetCurrentValue(TitleProperty,
                title);
            SetCurrentValue(InputProperty,
                input);
            SetCurrentValue(PlaceholderProperty,
                placeholder);
        }
    }
}