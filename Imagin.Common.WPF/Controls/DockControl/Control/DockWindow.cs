using System.Windows;

namespace Imagin.Common.Controls
{
    public sealed class DockWindow : Window
    {
        public DockRootControl Root => Content as DockRootControl;

        public DockWindow() : base() { }
    }
}