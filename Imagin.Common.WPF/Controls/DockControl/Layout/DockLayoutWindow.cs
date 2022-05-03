using Imagin.Common.Numbers;
using System;
using System.Windows;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class DockLayoutWindow : DockLayoutRoot
    {
        Point2D position;
        public Point2D Position
        {
            get => position;
            set => this.Change(ref position, value);
        }

        DoubleSize size;
        public DoubleSize Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        string state = $"{WindowState.Normal}";
        public virtual WindowState State
        {
            get => (WindowState)Enum.Parse(typeof(WindowState), state);
            set => this.Change(ref state, $"{value}");
        }

        public DockLayoutWindow() : base() { }
    }
}