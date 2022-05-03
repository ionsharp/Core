using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System.Windows;

namespace Imagin.Common.Controls
{
    public sealed partial class DialogWindow : Window
    {
        public static DoubleSize DefaultImageSize = new(64, 64);

        public static readonly DependencyProperty ReferenceProperty = DependencyProperty.Register(nameof(Reference), typeof(DialogReference), typeof(DialogWindow), new FrameworkPropertyMetadata(null));
        public DialogReference Reference
        {
            get => (DialogReference)GetValue(ReferenceProperty);
            set => SetValue(ReferenceProperty, value);
        }

        internal DialogWindow() : base() => InitializeComponent();

        internal DialogWindow(DialogOpenedHandler onOpened) : base()
        {
            InitializeComponent();
            onOpened.If(i => OnOpened(i));
        }

        async void OnOpened(DialogOpenedHandler handler)
        {
            var result = await handler();
            XWindow.SetResult(this, result);
            Close();
        }
    }
}