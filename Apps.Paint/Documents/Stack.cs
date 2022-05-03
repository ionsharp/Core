using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Stack")]
    [Icon(App.ImagePath + "DocumentStack.png")]
    [Serializable]
    public class StackDocument : ArrangeDocument
    {
        enum Category { Alignment }

        #region Properties

        Adaptation adapt;
        [Featured, Index(1), Tool]
        public Adaptation Adapt
        {
            get => adapt;
            set => this.Change(ref adapt, value);
        }

        StringColor background = new(System.Windows.Media.Colors.White);
        [Format(ColorFormat.Both)]
        [Tool]
        public StringColor Background
        {
            get => background;
            set => this.Change(ref background, value);
        }

        int columnsOrRows = 1;
        [Tool]
        [Range(1, int.MaxValue, 1)]
        public int ColumnsOrRows
        {
            get => columnsOrRows;
            set => this.Change(ref columnsOrRows, value);
        }

        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
        [Category(Category.Alignment)]
        [Tool]
        public HorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set => this.Change(ref horizontalAlignment, value);
        }

        Interpolations interpolation = Interpolations.Bilinear;
        [Hidden, Tool]
        public Interpolations Interpolation
        {
            get => interpolation;
            set => this.Change(ref interpolation, value);
        }

        Thickness margin;
        [Tool]
        public Thickness Margin
        {
            get => margin;
            set => this.Change(ref margin, value);
        }

        Orientation orientation;
        [Featured, Index(0), Tool]
        public Orientation Orientation
        {
            get => orientation;
            set => this.Change(ref orientation, value);
        }

        int spacing;
        [Tool]
        public int Spacing
        {
            get => spacing;
            set => this.Change(ref spacing, value);
        }

        VerticalAlignment verticalAlignment = VerticalAlignment.Center;
        [Category(Category.Alignment)]
        [Tool]
        public VerticalAlignment VerticalAlignment
        {
            get => verticalAlignment;
            set => this.Change(ref verticalAlignment, value);
        }

        bool wrap = false;
        [Tool]
        public bool Wrap
        {
            get => wrap;
            set => this.Change(ref wrap, value);
        }

        #endregion

        #region StackDocument

        public StackDocument() : base() { }

        #endregion

        #region Methods

        void OnLayersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var size = GetSize();
            Height = size.Height; Width = size.Width;
        }

        void Apply(Adaptation type)
        {
            int minimum = 0, maximum = 0;
            switch (type)
            {
                case Adaptation.Grow:
                case Adaptation.Shrink:
                    Limit(out minimum, out maximum);
                    break;
            }

            Layers.ForEach(i =>
            {
                var layer = i as StaticLayer;

                var length = default(int);
                switch (type)
                {
                    case Adaptation.Grow:
                        length = maximum;
                        break;
                    case Adaptation.Shrink:
                        length = minimum;
                        break;
                    case Adaptation.None:
                        layer.DisplayHeight = double.NaN;
                        layer.DisplayWidth = double.NaN;
                        return;
                }

                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        layer.DisplayHeight = length;
                        layer.DisplayWidth = double.NaN;
                        break;
                    case Orientation.Vertical:
                        layer.DisplayHeight = double.NaN;
                        layer.DisplayWidth = length;
                        break;
                }
            });
        }

        Bitmap Draw()
        {
            var size = GetSize();
            var height = size.Height; var width = size.Width;

            var result = new Bitmap(width, height);
            using (var g = Graphics.FromImage(result))
            {
                g.FillRectangle(new SolidBrush(Background.Value.Int32()), new Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(result.Width, result.Height)));

                var column = 0; var row = 0; var mh = 0; var mw = 0;
                int x = 0 + Margin.Left.Int32(), y = 0 + Margin.Top.Int32();
                for (int i = 0, Count = Layers.Count; i < Count; i++)
                {
                    var layer = Layers[i] as StaticLayer;

                    var h
                        = double.IsNaN(layer.DisplayHeight)
                        ? layer.Source.PixelHeight
                        : layer.DisplayHeight.Int32();

                    var w
                        = double.IsNaN(layer.DisplayWidth)
                        ? layer.Source.PixelWidth
                        : layer.DisplayWidth.Int32();

                    var image = layer.Source.Resize(w, h, interpolation);
                    image.ForEach((_, _, j) => System.Windows.Media.Color.FromArgb((layer.Opacity * 255).Byte(), j.R, j.G, j.B));

                    var bitmap = image.Bitmap<PngBitmapEncoder>();

                    if (!Wrap)
                    {
                        switch (Orientation)
                        {
                            case Orientation.Horizontal:
                                var vo = 0;
                                switch (VerticalAlignment)
                                {
                                    case VerticalAlignment.Bottom:
                                        vo = (-y + height - h - margin.Bottom).Int32();
                                        break;

                                    case VerticalAlignment.Center:
                                        vo = -y + (height / 2) - (h / 2);
                                        break;

                                    case VerticalAlignment.Top:
                                        vo = 0;
                                        break;
                                }

                                g.DrawImage(bitmap, x, y + vo);
                                x += w + Spacing;
                                break;

                            case Orientation.Vertical:
                                var ho = w / 2;
                                switch (HorizontalAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        ho = 0;
                                        break;

                                    case HorizontalAlignment.Center:
                                        ho = -x + (width / 2) - (w / 2);
                                        break;

                                    case HorizontalAlignment.Right:
                                        ho = (-x + width - w - margin.Left).Int32();
                                        break;
                                }

                                g.DrawImage(bitmap, x + ho, y);
                                y += h + Spacing;
                                break;
                        }
                    }
                    else
                    {
                        g.DrawImage(bitmap, x, y);
                        switch (Orientation)
                        {
                            case Orientation.Horizontal:
                                x += w + spacing;
                                mh = h > mh ? h : mh;

                                column++;
                                if (column == ColumnsOrRows)
                                {
                                    x = 0;
                                    y += mh + spacing;

                                    mh = 0;
                                }
                                break;

                            case Orientation.Vertical:
                                y += h + spacing;
                                mw = w > mw ? w : mw;

                                row++;
                                if (row == ColumnsOrRows)
                                {
                                    x += mw + spacing;
                                    y = 0;

                                    mw = 0;
                                }
                                break;
                        }
                    }
                }
            }

            return result;
        }

        System.Drawing.Size GetSize()
        {
            int height = 0, width = 0;
            var column = 0; var row = 0; var mh = 0; var mw = 0;

            if (ColumnsOrRows == 1 || !Wrap)
            {
                var orientation = Wrap ? Orientation.Invert() : Orientation;
                foreach (var i in Layers)
                {
                    var layer = i as StaticLayer;

                    var h
                        = double.IsNaN(layer.DisplayHeight)
                        ? layer.Source.PixelHeight
                        : layer.DisplayHeight.Int32();

                    var w
                        = double.IsNaN(layer.DisplayWidth)
                        ? layer.Source.PixelWidth
                        : layer.DisplayWidth.Int32();

                    switch (orientation)
                    {
                        case Orientation.Horizontal:
                            height = h > height ? h : height;
                            width += w;
                            break;

                        case Orientation.Vertical:
                            width = w > width ? w : width;
                            height += h;
                            break;
                    }
                }
            }
            else
            {
                foreach (var i in Layers)
                {
                    var layer = i as StaticLayer;

                    var h
                        = double.IsNaN(layer.DisplayHeight)
                        ? layer.Source.PixelHeight
                        : layer.DisplayHeight.Int32();

                    var w
                        = double.IsNaN(layer.DisplayWidth)
                        ? layer.Source.PixelWidth
                        : layer.DisplayWidth.Int32();

                    switch (Orientation)
                    {
                        case Orientation.Horizontal:
                            mh = h > mh ? h : mh;
                            mw += w;
                            
                            width += w;
                            column++;

                            if (column == ColumnsOrRows)
                            {
                                height += mh;
                                column = 0;

                                width = mw > width ? mw : width;
                                mh = 0; mw = 0;

                                row++;
                            }
                            break;

                        case Orientation.Vertical:
                            mw = w > mw ? w : mw;
                            mh += h;

                            height += h;
                            row++;

                            if (row == ColumnsOrRows)
                            {
                                width += mw;
                                row = 0;

                                height = mh > height ? mh : height;
                                mw = 0; mh = 0;

                                column++;
                            }
                            break;
                    }
                }
            }

            height
                += margin.Bottom.Int32()
                + margin.Top.Int32();
            width
                += margin.Left.Int32()
                + margin.Right.Int32();

            switch (orientation)
            {
                case Orientation.Horizontal:
                    width += spacing * (Layers.Count - 1);
                    if (Wrap)
                        height += spacing * row;

                    break;

                case Orientation.Vertical:
                    height += spacing * (Layers.Count - 1);
                    if (Wrap)
                        width += spacing * column;

                    break;
            }

            return new(width, height);
        }

        void Limit(out int minimum, out int maximum)
        {
            minimum = 0;
            maximum = 0;
            
            var mi = int.MaxValue;
            var ma = int.MinValue;
            if (Adapt == Adaptation.Grow || Adapt == Adaptation.Shrink)
            {
                Layers.ForEach(i =>
                {
                    var layer = i as StaticLayer;
                    switch (Orientation)
                    {
                        case Orientation.Horizontal:
                            switch (Adapt)
                            {
                                case Adaptation.Grow:
                                    ma = layer.Source.PixelHeight > ma ? (int)layer.Source.PixelHeight : ma;
                                    break;
                                case Adaptation.Shrink:
                                    mi = layer.Source.PixelHeight < mi ? (int)layer.Source.PixelHeight : mi;
                                    break;
                            }
                            break;
                        case Orientation.Vertical:
                            switch (Adapt)
                            {
                                case Adaptation.Grow:
                                    ma = (int)layer.Source.PixelWidth > ma ? (int)layer.Source.PixelWidth : ma;
                                    break;
                                case Adaptation.Shrink:
                                    mi = (int)layer.Source.PixelWidth < mi ? (int)layer.Source.PixelWidth : mi;
                                    break;
                            }
                            break;
                    }
                });
            }
            minimum = mi;
            maximum = ma;
        }

        //...

        public override Document Clone() => new StackDocument();

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Adapt):
                    Apply(Adapt);
                    goto case nameof(Margin);

                case nameof(Orientation):
                    Apply(Adapt);
                    goto case nameof(Margin);

                case nameof(Margin):
                case nameof(Spacing):
                    var size = GetSize();
                    Height = size.Height; Width = size.Width;
                    break;

                case nameof(Wrap):
                    goto case nameof(Margin);
            }
        }

        public override Bitmap Render()
        {
            try
            {
                var result = Draw();

                if (result == null)
                    throw new NullReferenceException();

                return result;
            }
            catch (Exception e)
            {
                Log.Write<StackDocument>(e);
                return null;
            }
        }

        //...

        public override void Subscribe() 
        {
            base.Subscribe();
            Layers.CollectionChanged += OnLayersChanged;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            Layers.CollectionChanged -= OnLayersChanged;
        }

        #endregion
    }
}