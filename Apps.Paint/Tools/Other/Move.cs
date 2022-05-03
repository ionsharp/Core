using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Move")]
    [Icon(App.ImagePath + "Move.png")]
    [Serializable]
    public class MoveTool : Tool
    {
        enum Category { Commands }

        #region Properties

        PointCollection oldPoints;

        [field: NonSerialized]
        Vector2<int> startPoint;

        //...

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Move.png");

        [Hidden]
        new protected VisualLayer TargetLayer => base.TargetLayer as VisualLayer;

        int? x = null;
        [ReadOnly]
        public int? X
        {
            get => x;
            set => this.Change(ref x, value);
        }

        int? y = null;
        [ReadOnly]
        public int? Y
        {
            get => y;
            set => this.Change(ref y, value);
        }

        #endregion

        #region Methods

        void MovePixels(Point point)
        {
            var x = MouseDown.Value.X - point.X;
            var y = MouseDown.Value.Y - point.Y;

            x = x < 0 ? startPoint.X + x.Absolute() : startPoint.X - x.Absolute();
            y = y < 0 ? startPoint.Y + y.Absolute() : startPoint.Y - y.Absolute();

            TargetLayer.X = x.Round().Int32();
            TargetLayer.Y = y.Round().Int32();
        }

        void Resize(PixelLayer layer)
        {
            //1.Check if layer position and size covers entire document
            if (layer.X > 0 || layer.Y > 0 || layer.X + layer.Pixels.PixelWidth < Document.Width || layer.Y + layer.Pixels.PixelHeight < Document.Height)
            {
                WriteableBitmap oldPixels = layer.Pixels, newPixels = null;

                //2. Get a height and width that allows the layer to cover the entire document based on it's current position
                var newHeight
                    = layer.Y > 0
                    ? layer.Y + layer.Pixels.PixelHeight
                    : layer.Pixels.PixelHeight + (Document.Height - (layer.Y + layer.Pixels.PixelHeight)).Minimum(0);
                var newWidth
                    = layer.X > 0
                    ? layer.X + layer.Pixels.PixelWidth
                    : layer.Pixels.PixelWidth + (Document.Width - (layer.X + layer.Pixels.PixelWidth)).Minimum(0);

                //3. Create a new image
                newPixels = BitmapFactory.New(newWidth, newHeight);

                //4. Write old pixels onto new pixels based on the layer's position relative to the document!
                newPixels.ForEach((x, y, color) =>
                {
                    if (layer.X > 0 && layer.Y > 0)
                    {
                        if (x >= layer.X && y >= layer.Y)
                            return oldPixels.GetPixel(x - layer.X, y - layer.Y);
                    }
                    else if (layer.X > 0 && layer.Y < 0)
                    {
                        if (x >= layer.X && y <= layer.Pixels.PixelHeight)
                            return oldPixels.GetPixel(x - layer.X, y);
                    }
                    else if (layer.X < 0 && layer.Y > 0)
                    {
                        if (x <= layer.Pixels.PixelWidth && y >= layer.Y)
                            return oldPixels.GetPixel(x, y - layer.Y);
                    }
                    else if (layer.X < 0 && layer.Y < 0)
                    {
                        if (x <= layer.Pixels.PixelWidth && y <= layer.Pixels.PixelHeight)
                            return oldPixels.GetPixel(x, y);
                    }
                    return Colors.Transparent;
                });

                layer.Pixels = newPixels;
                if (layer.X > 0)
                    layer.X = 0;

                if (layer.Y > 0)
                    layer.Y = 0;
            }
        }

        //...

        public override bool OnMouseDown(Point point)
        {
            if (TargetLayer != null)
            {
                base.OnMouseDown(point);

                int xStart = 0, yStart = 0;
                switch (TargetLayer.Type)
                {
                    case LayerType.Pixel:
                    case LayerType.Text:
                        xStart = TargetLayer.X;
                        yStart = TargetLayer.Y;
                        break;

                    case LayerType.Shape:
                        oldPoints = TargetLayer.As<ShapeLayer>().Points;
                        break;
                }

                startPoint = new Vector2<int>(xStart, yStart);
                X = 0; Y = 0;
            }
            return false;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                X = (MouseMove.Value.X - MouseDown.Value.X).Round().Int32();
                Y = (MouseMove.Value.Y - MouseDown.Value.Y).Round().Int32();

                switch (TargetLayer.Type)
                {
                    case LayerType.Pixel:
                        MovePixels(point);
                        break;

                    case LayerType.Shape:
                        TargetLayer.If<ShapeLayer>(i =>
                        {
                            i.Points = new(Enumerable.Select<Point, Point>(oldPoints, i => new(i.X + X.Value, i.Y + Y.Value)));
                            i.Render();
                        });
                        break;

                    case LayerType.Text:
                        TargetLayer.If<TextLayer>(i => i.Region = new(startPoint.X + X.Value, startPoint.Y + Y.Value, i.Width, i.Height));
                        break;
                }
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            startPoint = null;

            if (TargetLayer is PixelLayer)
                Resize((PixelLayer)TargetLayer);

            X = null; Y = null;
            oldPoints = null;
        }

        #endregion

        #region Commands

        ICommand alignTopEdgesCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "AlignTopEdges.png")]
        public ICommand AlignTopEdgesCommand => alignTopEdgesCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand alignVerticalCentersCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "AlignVerticalCenters.png")]
        public ICommand AlignVerticalCentersCommand => alignVerticalCentersCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand alignBottomEdgesCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "AlignBottomEdges.png")]
        public ICommand AlignBottomEdgesCommand => alignBottomEdgesCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand alignLeftEdgesCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "AlignLeftEdges.png")]
        public ICommand AlignLeftEdgesCommand => alignLeftEdgesCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand alignHorizontalCentersCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "AlignHorizontalCenters.png")]
        public ICommand AlignHorizontalCentersCommand => alignHorizontalCentersCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand alignRightEdgesCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "AlignRightEdges.png")]
        public ICommand AlignRightEdgesCommand => alignRightEdgesCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand distributeTopEdgesCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "DistributeTopEdges.png")]
        public ICommand DistributeTopEdgesCommand => distributeTopEdgesCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand distributeVerticalCentersCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "DistributeVerticalCenters.png")]
        public ICommand DistributeVerticalCentersCommand => distributeVerticalCentersCommand = distributeVerticalCentersCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand distributeBottomEdgesCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "DistributeBottomEdges.png")]
        public ICommand DistributeBottomEdgesCommand => distributeBottomEdgesCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand distributeLeftEdgesCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "DistributeLeftEdges.png")]
        public ICommand DistributeLeftEdgesCommand => distributeLeftEdgesCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand distributeHorizontalCentersCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "DistributeHorizontalCenters.png")]
        public ICommand DistributeHorizontalCentersCommand => distributeHorizontalCentersCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand distributeRightEdgesCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "DistributeRightEdges.png")]
        public ICommand DistributeRightEdgesCommand => distributeRightEdgesCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        ICommand autoAlignLayersCommand;
        [Category(Category.Commands)]
        [Icon(App.ImagePath + "AutoAlignLayers.png")]
        public ICommand AutoAlignLayersCommand => autoAlignLayersCommand ??= new RelayCommand(() =>
        {
        }, () => true);

        #endregion
    }
}