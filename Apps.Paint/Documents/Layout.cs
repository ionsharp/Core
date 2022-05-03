using Imagin.Common;
using Imagin.Common.Media;
using System;
using System.Drawing;
using System.Windows;

namespace Imagin.Apps.Paint
{
    public enum LayoutTypes
    {
        [Icon(App.ImagePath + "LayoutCircle.png")]
        Circle,
        [Icon(App.ImagePath + "LayoutDiagonalLeft.png")]
        DiagonalLeft,
        [Icon(App.ImagePath + "LayoutDiagonalRight.png")]
        DiagonalRight,
        [Icon(App.ImagePath + "LayoutHorizontal.png")]
        StripesHorizontal,
        [Icon(App.ImagePath + "LayoutVertical.png")]
        StripesVertical,
        [Icon(App.ImagePath + "LayoutTable.png")]
        Table,
        [Icon(App.ImagePath + "LayoutZigZag.png")]
        ZigZag
    }

    [DisplayName("Layout")]
    [Icon(App.ImagePath + "DocumentLayout.png")]
    [Serializable]
    public class LayoutDocument : ArrangeDocument
    {
        enum Category { Size }

        StringColor background = new(System.Windows.Media.Colors.White);
        [Tool]
        public StringColor Background
        {
            get => background;
            set => this.Change(ref background, value);
        }

        LayoutTypes layoutType = LayoutTypes.StripesHorizontal;
        [DisplayName("Layout")]
        [Featured, Tool]
        public LayoutTypes LayoutType
        {
            get => layoutType;
            set => this.Change(ref layoutType, value);
        }

        int columns = 2;
        [Tool]
        public int Columns
        {
            get => columns;
            set => this.Change(ref columns, value);
        }

        [Category(Category.Size)]
        [Format(RangeFormat.UpDown)]
        [Tool]
        [Range(1, int.MaxValue, 1)]
        public override int Height
        {
            get => base.Height;
            set => base.Height = value;
        }

        int rows = 2;
        [Tool]
        public int Rows
        {
            get => rows;
            set => this.Change(ref rows, value);
        }

        Thickness spacing = new(5);
        [Tool]
        public Thickness Spacing
        {
            get => spacing;
            set => this.Change(ref spacing, value);
        }

        [Category(Category.Size)]
        [Format(RangeFormat.UpDown)]
        [Tool]
        [Range(1, int.MaxValue, 1)]
        public override int Width
        {
            get => base.Width;
            set => base.Width = value;
        }

        public LayoutDocument() : base()
        {
            Height = 256;
            Width = 256;
        }

        public override Document Clone() => new LayoutDocument();

        public override Bitmap Render() => default;
    }
}