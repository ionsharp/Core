using System;
using System.Windows;

namespace Imagin.Common
{
    [Serializable]
    public class Alignment : Base
    {
        public static Alignment Center => new(HorizontalAlignment.Center, VerticalAlignment.Center);

        HorizontalAlignment horizontal = HorizontalAlignment.Left;
        public HorizontalAlignment Horizontal
        {
            get => horizontal;
            set => this.Change(ref horizontal, value);
        }

        VerticalAlignment vertical = VerticalAlignment.Top;
        public VerticalAlignment Vertical
        {
            get => vertical;
            set => this.Change(ref vertical, value);
        }

        public Alignment() : base() { }

        public Alignment(HorizontalAlignment horizontal, VerticalAlignment vertical) : this()
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}