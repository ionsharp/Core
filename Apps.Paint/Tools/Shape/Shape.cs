using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    #region (abstract) ShapeTool

    [Serializable]
    public abstract class ShapeTool : Tool
    {
        protected virtual PointCollection CurrentPoints { get; set; }

        [NonSerialized]
        protected ShapeLayer hiddenLayer;

        [NonSerialized]
        protected Shape targetPath;

        [NonSerialized]
        protected PathGroup targetPathGroup;

        BlendModes blendMode = BlendModes.Normal;
        public BlendModes BlendMode
        {
            get => blendMode;
            set => this.Change(ref blendMode, value);
        }

        [Hidden]
        public override Cursor Cursor => Cursors.Cross;

        ShapeToolModes mode = ShapeToolModes.Shape;
        [Index(-999)]
        public ShapeToolModes Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        double opacity = 1;
        public double Opacity
        {
            get => opacity;
            set => this.Change(ref opacity, value);
        }

        protected abstract ShapeLayer NewLayer();

        protected override bool AssertLayer() => true;

        public abstract IEnumerable<Point> GetPoints();

        public override bool OnMouseDown(Point point)
        {
            if (!base.OnMouseDown(point))
                return false;

            switch (Mode)
            {
                case ShapeToolModes.Path:
                    targetPath = new Shape();
                    break;

                case ShapeToolModes.Pixels:
                case ShapeToolModes.Shape:
                    hiddenLayer = NewLayer();
                    break;
            }
            return true;
        }

        public override void OnMouseUp(Point point)
        {
            switch (Mode)
            {
                case ShapeToolModes.Path:
                    //targetPathGroup
                        //= TargetPathGroup is PathGroup a
                        //? a : new PathGroup("Untitled");

                    //targetPathGroup.Paths.Add(targetPath);

                    /*
                    if (!ReferenceEquals(targetPathGroup, TargetPathGroup))
                    {
                        Document.If<ImageDocument>(i => i.Paths.ForEach(j => j.IsSelected = false));
                        targetPathGroup.IsSelected = true;

                        Document.If<ImageDocument>(i => i.Paths.Insert(0, targetPathGroup));
                    }
                    */

                    targetPath.Points = new(GetPoints());
                    hiddenLayer = null; targetPath = null; targetPathGroup = null;
                    break;

                case ShapeToolModes.Pixels:
                    hiddenLayer.Render();

                    TargetLayer.As<PixelLayer>().Pixels.Blend(BlendMode, hiddenLayer.Pixels);
                    hiddenLayer = null;
                    break;

                case ShapeToolModes.Shape:
                    hiddenLayer.Points = new(CurrentPoints);
                    hiddenLayer.Render();

                    Layers.InsertAbove(TargetLayer, Array<Layer>.New(hiddenLayer));

                    Layers.UnselectAll();
                    hiddenLayer.IsSelected = true;

                    hiddenLayer = null;
                    break;
            }
            base.OnMouseUp(point);
        }
    }

    #endregion

    #region (abstract) RegionShapeTool

    [Serializable]
    public abstract class RegionShapeTool : ShapeTool
    {
        [NonSerialized]
        protected Int32Region CurrentRegion;

        string fill = $"255,0,0,0";
        public SolidColorBrush Fill
        {
            get
            {
                var result = fill.Split(',');
                return new SolidColorBrush(Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte()));
            }
            set => this.Change(ref fill, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        string stroke = $"255,0,0,0";
        public SolidColorBrush Stroke
        {
            get
            {
                var result = stroke.Split(',');
                return new SolidColorBrush(Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte()));
            }
            set => this.Change(ref stroke, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        double strokeThickness = 1.0;
        public double StrokeThickness
        {
            get => strokeThickness;
            set => this.Change(ref strokeThickness, value);
        }

        protected override ShapeLayer NewLayer() => new RegionShapeLayer("Untitled", Fill, Stroke, StrokeThickness, new(Document.Width, Document.Height));

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (hiddenLayer != null)
            {
                if (MouseDown != null && MouseMove != null)
                {
                    CurrentRegion = GetRegion(MouseDown.Value, MouseMove.Value);

                    CurrentPoints = new(GetPoints());
                    CurrentPoints.Add(CurrentPoints.First());

                    if (CurrentPoints.Count() > 1)
                    {
                        var geometry = new Shape(CurrentPoints).Geometry(true);
                        input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Black, 1 / zoom), geometry);
                        input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.White, 0.5 / zoom), geometry);
                    }
                }
            }
        }
    }

    #endregion
}