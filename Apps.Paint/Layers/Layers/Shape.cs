using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    #region ShapeLayer

    [DisplayName("Shape layer")]
    [Icon(App.ImagePath + "LayerShape.png")]
    [Serializable]
    public class ShapeLayer : RasterizableLayer
    {
        enum Category { Stroke }

        #region Properties

        [Hidden]
        public override PointCollection Bounds => default;

        PointCollection points = null;
        [Hidden]
        public PointCollection Points
        {
            get => points;
            set => this.Change(ref points, value);
        }

        string stroke = $"255,0,0,0";
        [Category(Category.Stroke)]
        public virtual SolidColorBrush Stroke
        {
            get
            {
                var result = stroke.Split(',');
                return new SolidColorBrush(Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte()));
            }
            set => this.Change(ref stroke, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        double strokeThickness = 1.0;
        [Category(Category.Stroke)]
        [DisplayName("Stroke thickness")]
        [Range(0.0, 1000.0, 1.0)]
        [Format(Common.RangeFormat.UpDown)]
        public virtual double StrokeThickness
        {
            get => strokeThickness;
            set => this.Change(ref strokeThickness, value);
        }

        #endregion

        #region ShapeLayer

        public ShapeLayer(string name, SolidColorBrush stroke, double strokeThickness) : base(LayerType.Shape, name)
        {
            this.stroke = $"{stroke.Color.A},{stroke.Color.R},{stroke.Color.G},{stroke.Color.B}";
            this.strokeThickness = strokeThickness;
        }

        #endregion

        #region Methods

        protected virtual System.Windows.Media.Geometry GetGeometry()
        {
            if (Points == null)
                return null;

            var streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(Points[0], true, true);
                Points.RemoveAt(0);

                geometryContext.PolyLineTo(Points, true, true);
            }
            streamGeometry.Freeze();
            return streamGeometry;
        }

        public override Layer Clone() => new ShapeLayer(Name, Stroke, StrokeThickness)
        {
            IsLocked = IsLocked,
            IsVisible = IsVisible,
            Pixels = new(XWriteableBitmap.Clone(Pixels)),
            Points = new(Points),
            //Style = Style.Clone(),
            X = X,
            Y = Y
        };

        public virtual ShapeLayer Merge(ShapeLayer layer) => default;

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Stroke):
                case nameof(StrokeThickness):
                    Render();
                    break;

                case nameof(Points):

                    int? maxX = null; int? maxY = null;
                    int? minX = null; int? minY = null;
                    foreach (var i in Points)
                    {
                        if (minX == null || i.X < minX)
                            minX = i.X.Int32();

                        if (minY == null || i.Y < minY)
                            minY = i.Y.Int32();

                        if (maxX == null || i.X > maxX)
                            maxX = i.X.Int32();

                        if (maxY == null || i.Y > maxY)
                            maxY = i.Y.Int32();
                    }

                    X = minX.Value;
                    Y = minY.Value;

                    if (this is RegionShapeLayer layer)
                    {
                        layer.Height = maxY.Value - minY.Value;
                        layer.Width = maxX.Value - minX.Value;
                    }
                    break;
            }
        }

        public override void Render(WriteableBitmap input)
        {
            if (Points != null)
                input?.DrawPolyline(Shape.From(Points), Stroke.Color, StrokeThickness.Int32());
        }

        #endregion
    }

    #endregion

    #region RegionShapeLayer

    [DisplayName("Shape layer")]
    [Serializable]
    public class RegionShapeLayer : ShapeLayer
    {
        enum Category { Fill, Size }

        #region Properties

        [Hidden]
        public override PointCollection Bounds => new PointCollection
        {
            new System.Windows.Point(X, Y),
            new System.Windows.Point(X + width, Y),
            new System.Windows.Point(X + width, Y + height),
            new System.Windows.Point(X, Y + height)
        };

        string fill = $"255,0,0,0";
        [Category(Category.Fill)]
        public virtual SolidColorBrush Fill
        {
            get
            {
                var result = fill.Split(',');
                var actualResult = default(SolidColorBrush);
                Try.Invoke(() => actualResult = new SolidColorBrush(Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte())));
                return actualResult ?? Brushes.Transparent;
            }
            set => this.Change(ref fill, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        protected int height = 0;
        [Category(Category.Size)]
        [Range(1, 5000, 1)]
        [Format(Common.RangeFormat.Both)]
        public int Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        protected int width = 0;
        [Category(Category.Size)]
        [Range(1, 5000, 1)]
        [Format(Common.RangeFormat.Both)]
        public int Width
        {
            get => width;
            set => this.Change(ref width, value);
        }

        #endregion

        #region RegionShapeLayer

        public RegionShapeLayer(string name, SolidColorBrush fill, SolidColorBrush stroke, double strokeThickness, System.Drawing.Size size) : base(name, stroke, strokeThickness)
        {
            this.fill = $"{fill.Color.A},{fill.Color.R},{fill.Color.G},{fill.Color.B}";
            Pixels = new(BitmapFactory.New(size));
        }

        #endregion

        #region Methods

        public override Layer Clone()
        {
            var result = new RegionShapeLayer(Name, Fill, Stroke, StrokeThickness, new(Size.Width, Size.Height))
            {
                Height = Height,
                IsLocked = IsLocked,
                IsVisible = IsVisible,
                Pixels = new(XWriteableBitmap.Clone(Pixels)),
                Points = new(Points),
                Width = Width,
                X = X,
                Y = Y
            };
            result.Style.Paste(Style);
            return result;
        }

        public override void Crop(int x, int y, int height, int width)
        {
            var dx = x - X;
            var dy = y - Y;
            X -= x;
            Y -= y;
        }

        public override void Crop(Int32Size oldSize, Int32Size newSize, CardinalDirection direction)
        {
            /*
            if (height < Height)
            {
                //We don't have to worry about resizing layers vertically
            }
            else if (height > Height)
            {
                //Resize each layer vertically...
                foreach (var i in Layers)
                {
                    if (i is VisualLayer)
                    {
                        //...if the height of the layer is less than the new canvas height.
                        if ((i as VisualLayer).Height < height)
                        {
                            //Set i.Height to height
                        }
                    }
                }
            }

            if (width < Width)
            {
                //We don't have to worry about resizing layers horizontally
            }
            else if (width > Width)
            {
                //Resize all layers horizontally...
                foreach (var i in Layers)
                {
                    if (i is VisualLayer)
                    {
                        //...if the width of the layer is less than the new canvas width.
                        if ((i as VisualLayer).Width < width)
                        {
                            //Set i.Width to width
                        }
                    }
                }
            }

            //Now we have to apply the anchor
            foreach (var i in Layers)
            {
                switch (direction)
                {
                    case CardinalDirection.E:
                        break;
                    case CardinalDirection.N:
                        break;
                    case CardinalDirection.NE:
                        break;
                    case CardinalDirection.NW:
                        break;
                    case CardinalDirection.Origin:
                        break;
                    case CardinalDirection.S:
                        break;
                    case CardinalDirection.SE:
                        break;
                    case CardinalDirection.SW:
                        break;
                    case CardinalDirection.W:
                        break;
                }
            }
            */
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Fill):
                case nameof(Height):
                case nameof(Width):
                    Render();
                    break;
            }
        }

        public override void Render(WriteableBitmap input)
        {
            if (input == null || Points == null) 
                return;

            var points = Shape.From(Points);
            input.FillPolygon(points, Fill.Color);
            input.DrawPolyline(points, Stroke.Color, StrokeThickness.Int32());
        }

        #endregion
    }

    #endregion
}