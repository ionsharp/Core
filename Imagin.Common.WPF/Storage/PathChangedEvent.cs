using Imagin.Common.Linq;
using System.Windows;

namespace Imagin.Common.Storage
{
    public delegate void PathChangedEventHandler(object sender, PathChangedEventArgs e);

    public class PathChangedEventArgs : RoutedEventArgs
    {
        public readonly string Path;

        public PathChangedEventArgs(RoutedEvent input, string path) : base(input) => Path = path;
    }
}