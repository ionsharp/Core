using Imagin.Common.Models;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class DockDragEvent : Base
    {
        public object ActualContent => Content == null ? Root.DockControl.Convert(Window.Root.Child) : Content;

        public readonly Content[] Content;

        public readonly DockRootControl Root;

        public readonly DockContentControl Source;

        public readonly DockWindow Window;

        IDockControl mouseOver = null;
        public IDockControl MouseOver
        {
            get => mouseOver;
            internal set => this.Change(ref mouseOver, value);
        }

        public Point MousePosition { get; internal set; }

        internal DockDragEvent(DockContentControl source, DockRootControl root, Content[] content, DockWindow window)
        {
            Source = source;
            Root = root;
            Content = content;
            Window = window;
        }
    }
}