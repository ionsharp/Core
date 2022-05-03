namespace Imagin.Common.Controls
{
    public interface IDockControl
    {
        double ActualHeight { get; }

        double ActualWidth { get; }

        DockControl DockControl { get; }

        DockRootControl Root { get; }
    }
}