using Imagin.Common.Models;
using System.Windows;

namespace Imagin.Common.Controls
{
    internal sealed class DockDragReference
    {
        public readonly Content[] Content;

        public readonly Point Start;

        public DockDragReference(Content[] content, Point start)
        {
            Content 
                = content;
            Start 
                = start;
        }
    }
}