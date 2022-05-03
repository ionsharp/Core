using System.Windows;

namespace Imagin.Common.Controls
{
    public sealed class DockDocumentControl : DockContentControl
    {
        public static readonly DependencyProperty PersistProperty = DependencyProperty.Register(nameof(Persist), typeof(bool), typeof(DockDocumentControl), new FrameworkPropertyMetadata(false));
        public bool Persist
        {
            get => (bool)GetValue(PersistProperty);
            set => SetValue(PersistProperty, value);
        }

        public DockDocumentControl(DockRootControl root) : base(root) { }
    }
}