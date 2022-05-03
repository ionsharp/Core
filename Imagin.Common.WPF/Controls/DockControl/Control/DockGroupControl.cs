using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public sealed class DockGroupControl : Grid, IDockControl
    {
        static readonly DependencyPropertyKey OrientationKey = DependencyProperty.RegisterReadOnly(nameof(Orientation), typeof(Orientation), typeof(DockDocumentControl), new FrameworkPropertyMetadata(Orientation.Horizontal));
        public static readonly DependencyProperty OrientationProperty = OrientationKey.DependencyProperty;
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            private set => SetValue(OrientationKey, value);
        }

        //...

        public DockControl DockControl => Root.DockControl;

        public DockRootControl Root { get; private set; }

        //...

        public DockGroupControl(DockRootControl root, Orientation orientation) : base()
        {
            Root = root; Orientation = orientation;
        }
    }
}