using ImageMagick;
using Imagin.Apps.Paint.Effects;
using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using Imagin.Common.Numbers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    #region ImageDocument

    [DisplayName("Image")]
    [Icon(App.ImagePath + "DocumentImage.png")]
    [Serializable]
    public class ImageDocument : Document
    {
        enum Category { Document }

        #region Properties

        [Category("Format")]
        [DisplayName("Bit depth")]
        [ReadOnly]
        [Status]
        public int BitDepth => default;

        ObservableCollection<Count> counts = new();
        [Hidden]
        public ObservableCollection<Count> Counts
        {
            get => counts;
            set => this.Change(ref counts, value);
        }

        ImageMode mode = ImageMode.RGB;
        [Category("Mode")]
        [DisplayName("Mode")]
        public ImageMode Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        ImageModeDepth modeDepth = ImageModeDepth.Bit16;
        [Category("Mode")]
        [DisplayName("Mode depth")]
        public ImageModeDepth ModeDepth
        {
            get => modeDepth;
            set => this.Change(ref modeDepth, value);
        }

        ObservableCollection<Note> notes = new();
        [Hidden]
        public ObservableCollection<Note> Notes
        {
            get => notes;
            set => this.Change(ref notes, value);
        }

        ObservableCollection<PathGroup> paths = new();
        [Hidden]
        public ObservableCollection<PathGroup> Paths
        {
            get => paths;
            set => this.Change(ref paths, value);
        }

        PixelFormat pixelFormat = PixelFormat.Rgba128Float;
        /// <summary>
        /// RGB
        ///     -> Rgba128Float (32 bits / channel)
        ///     -> Rgba64       (16 bits / channel)
        ///     -> Bgr32        (8  bits / channel)
        /// CMYK
        ///     -> Cmyk32       (8  bits / channel)
        /// Grayscale
        ///     -> Gray32Float  (8  bits / channel)
        ///     -> Gray16       (4  bits / channel)
        ///     -> Gray8        (2  bits / channel)
        /// Black/White
        ///     -> BlackWhite   (1  bit  / pixel)
        /// </summary>
        [Category("Format")]
        [DisplayName("Pixel format")]
        [ReadOnly]
        public PixelFormat PixelFormat
        {
            get => pixelFormat;
            set => this.Change(ref pixelFormat, value);
        }

        #endregion

        #region ImageDocument

        public ImageDocument() : base() => Status = new ImageDocumentStatus(this);

        public ImageDocument(string name, Vector2<int> size, float resolution, PixelFormat format = PixelFormat.Rgba128Float) : this()
        {
            Height = size.Y;
            Name = name;
            PixelFormat = format;
            Resolution = resolution;
            Width = size.X;
        }

        public ImageDocument(DocumentPreset preset) : this()
        {
            Height = preset.Height;
            Name = preset.Name;
            PixelFormat = GetFormat(preset.Mode, preset.ModeDepth) ?? PixelFormat.Rgba64;
            Resolution = preset.Resolution;
            Width = preset.Width;

            var layer = new PixelLayer("Layer 0", new Size(preset.Width, preset.Height));
            layer.Pixels.Clear(preset.Background);

            Layers.Add(layer);
        }

        public ImageDocument(ImageDocument document) : this(document.Height, document.Width) 
        {
            //Copy over stuff
        }

        public ImageDocument(int height, int width) : this()
        {
            Height = height;
            Width = width;
        }

        public ImageDocument(WriteableBitmap bitmap, string filePath, params ImageEffect[] effects) : this()
        {
            Height 
                = bitmap.PixelHeight; 
            Width 
                = bitmap.PixelWidth;
            Path 
                = filePath;
            PixelFormat
                = bitmap.Format.Convert();

            var layer = new PixelLayer("Layer 0", bitmap) { IsSelected = true };
            effects?.ForEach(i => layer.Style.Effects.Add(i));
            Layers.Add(layer);
        }

        #endregion

        #region Methods

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            Selections.ForEach(i => i.Preserve());
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            Selections.ForEach(i => i.Restore());
            Status = new ImageDocumentStatus(this);
        }

        //...

        WriteableBitmap CopyFrom(PixelLayer layer, Shape selection)
        {
            selection ??= new Shape(Common.Media.Shape.GetRectangle(new(0, 0, layer.Pixels.PixelWidth, layer.Pixels.PixelHeight)));

            var bounds = Common.Media.Shape.GetBounds(selection.Points);

            var result = BitmapFactory.New((int)bounds.Width, (int)bounds.Height);
            result.ForEach((x0, y0, color) =>
            {
                int x1 = x0 + bounds.X.Int32();
                int y1 = y0 + bounds.Y.Int32();

                x1 = x1.Coerce(layer.Pixels.PixelWidth);
                y1 = y1.Coerce(layer.Pixels.PixelHeight);

                if (Common.Media.Shape.Contains(selection.Points, new(x1, y1)))
                    return layer.Pixels.GetPixel(x1, y1);

                return System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
            });
            return result;
        }

        public override Document Clone() => new ImageDocument(this);

        public override void Copy()
        {
            if (Get.Current<MainViewModel>().SelectedLayer is PixelLayer i)
                System.Windows.Clipboard.SetImage(CopyFrom(i, Selections[0]));
        }

        public override void CopyMerged()
        {
        }

        public override void Cut()
        {
            if (Get.Current<MainViewModel>().SelectedLayer is PixelLayer i)
            {
                Copy();
                if (Selections.Count == 0)
                    i.Pixels.Clear(System.Windows.Media.Colors.Transparent);

                else i.Pixels.FillPolygon(Common.Media.Shape.From(Selections[0].Points), System.Windows.Media.Colors.Transparent);
            }
        }

        public override void CutMerged()
        {
        }

        public override void Paste()
        {
            if (Get.Current<MainViewModel>().SelectedLayer is PixelLayer i)
                i.Pixels.Blend(BlendModes.Normal, System.Windows.Clipboard.GetImage().Bitmap().WriteableBitmap());
        }

        public override void PasteNewLayer()
        {
            if (Get.Current<MainViewModel>().SelectedLayer is PixelLayer i)
            {
                var newLayer = new PixelLayer("Untitled", System.Windows.Clipboard.GetImage().Bitmap().WriteableBitmap());
                Panel.Find<LayersPanel>().Layers.InsertAbove(i, newLayer);
            }
        }

        //...

        public void Crop(DoubleRegion selection) => Crop(selection.X.Int32(), selection.Y.Int32(), selection.Height.Int32(), selection.Width.Int32());

        public void Crop(int x, int y, int height, int width)
        {
            Height = height;
            Width = width;

            Layers.ForAll<VisualLayer>(i => i.Crop(x, y, height, width));
        }

        //...

        public void Flatten()
        {
            var result = MergeAll();
            Layers.Remove<Layer>();
            Layers.Add(result);
        }

        public static PixelFormat? GetFormat(ImageMode mode, ImageModeDepth depth)
        {
            switch (mode)
            {
                case ImageMode.Bitmap:
                    switch (depth)
                    {
                        case ImageModeDepth.Bit1:
                            return PixelFormat.BlackWhite;

                        case ImageModeDepth.Bit8:
                        case ImageModeDepth.Bit16:
                        case ImageModeDepth.Bit32:
                        default: return null;
                    }

                case ImageMode.Grayscale:
                    switch (depth)
                    {
                        case ImageModeDepth.Bit1:
                            return null;

                        case ImageModeDepth.Bit8:
                            return PixelFormat.Gray8;
                        
                        case ImageModeDepth.Bit16:
                            return PixelFormat.Gray16;

                        case ImageModeDepth.Bit32:
                            return PixelFormat.Gray32Float;
                    }
                    break;
                case ImageMode.Indexed:
                    switch (depth)
                    {
                        case ImageModeDepth.Bit1:
                        case ImageModeDepth.Bit16:
                        case ImageModeDepth.Bit32:
                        default: return null;

                        case ImageModeDepth.Bit8:
                            return PixelFormat.Indexed8;
                    }

                case ImageMode.RGB:
                    switch (depth)
                    {
                        case ImageModeDepth.Bit1:
                            return null;

                        case ImageModeDepth.Bit8:
                            return PixelFormat.Bgr32;

                        case ImageModeDepth.Bit16:
                            return PixelFormat.Rgba64;

                        case ImageModeDepth.Bit32:
                            return PixelFormat.Rgba128Float;
                    }
                    break;
            }
            return null;
        }

        public PixelLayer MergeAll(Predicate<Layer> where = null, LayerCollection layers = null)
        {
            layers ??= Layers;
            var size = new Size(Width, Height);

            StackableLayer a = new PixelLayer(null, size);
            layers.ForEach<StackableLayer>(i =>
            {
                a.Name ??= i.Name;
                if (where?.Invoke(i) != false)
                {
                    if (i.IsVisible)
                    {
                        StackableLayer b = i;
                        if (i is GroupLayer groupLayer)
                        {
                            b = MergeAll(where, groupLayer.Layers);
                            b.As<StyleLayer>().Style.Paste(groupLayer.Style);
                        }

                        a = a.Merge(b, size);
                    }
                }
            });

            return a as PixelLayer;
        }

        public void MergeVisible()
        {
            var result = MergeAll(i => i.IsVisible);
            Layers.Remove<Layer>(i => i.IsVisible);
            Layers.Add(result);
        }

        public void Resize(int height, int width, CardinalDirection direction, bool stretch, Interpolations interpolation)
        {
            Int32Size oldSize = new(Height, Width), newSize = new(height, width);

            Height = newSize.Height;
            Width = newSize.Width;

            if (stretch)
            {
                Layers.ForAll<PixelLayer>(i => i.Pixels = new(i.Pixels.Resize((i.Pixels.PixelWidth.Double() / (oldSize.Width.Double() / newSize.Width.Double())).Int32(), (i.Pixels.PixelHeight.Double() / (oldSize.Height.Double() / oldSize.Width.Double())).Int32(), interpolation)));
                return;
            }

            Layers.ForAll<VisualLayer>(i => i.Crop(oldSize, newSize, direction));
        }

        public virtual void Resize(ImageResizeOptions options)
        {
            var nh = options.NewHeight.Round().Int32();
            var nw = options.NewWidth.Round().Int32();
            Resize(nh, nw, options.Anchor, options.Mode == ImageResizeMode.Stretch, options.Interpolation);
        }

        public virtual void Rotate(ImageRotateOptions options)
        {
            Layers.ForEach<PixelLayer>(i => i.Pixels = i.Pixels.RotateFree(options.Angle.Int32()));
        }

        public virtual void Trim(ImageTrimOptions options)
        {
            ColorMatrix combined = null;

            var top = 0; var left = 0; var right = 0; var bottom = 0;
            if (options.Source == ImageTrimSource.TransparentPixels)
            {
                //Top
                for (uint y = 0; y < combined.Rows; y++)
                {
                    bool stop = false;
                    for (uint x = 0; x < combined.Columns; x++)
                    {
                        if (combined[y, x].A > 0)
                        {
                            stop = true;
                            break;
                        }
                    }
                    if (stop)
                        break;

                    top++;
                }
                //Bottom
                for (uint y = combined.Rows - 1; y >= 0; y--)
                {
                    bool stop = false;
                    for (uint x = 0; x < combined.Columns; x++)
                    {
                        if (combined[y, x].A > 0)
                        {
                            stop = true;
                            break;
                        }
                    }
                    if (stop)
                        break;

                    bottom++;
                }
                //Left
                for (uint x = 0; x < combined.Columns; x++)
                {
                    bool stop = false;
                    for (uint y = 0; y < combined.Rows; y++)
                    {
                        if (combined[y, x].A > 0)
                        {
                            stop = true;
                            break;
                        }
                    }
                    if (stop)
                        break;

                    left++;
                }
                //Right
                for (uint x = combined.Columns - 1; x >= 0; x--)
                {
                    bool stop = false;
                    for (uint y = 0; y < combined.Rows; y++)
                    {
                        if (combined[y, x].A > 0)
                        {
                            stop = true;
                            break;
                        }
                    }
                    if (stop)
                        break;

                    right++;
                }
            }

            //Fail (1): Empty document!
            if (top == Height || bottom == Height || left == Width || right == Width)
            {
                return;
            }

            //Fail (2): Nothing to trim!
            if (top == 0 && bottom == 0 && left == 0 && right == 0)
            {
                return;
            }

            if (bottom > 0)
                Resize(Height - bottom, Width, CardinalDirection.N, false, Interpolations.NearestNeighbor);

            if (left > 0)
                Resize(Height, Width - left, CardinalDirection.E, false, Interpolations.NearestNeighbor);

            if (right > 0)
                Resize(Height, Width - right, CardinalDirection.W, false, Interpolations.NearestNeighbor);

            if (top > 0)
                Resize(Height - top, Width, CardinalDirection.S, false, Interpolations.NearestNeighbor);
        }

        //...

        public override IEnumerable<Layer> NewLayers() 
            { yield return new PixelLayer("Untitled", new Size(Width, Height)); }

        public override Bitmap Render() 
            => MergeAll().Pixels.Bitmap<PngBitmapEncoder>();

        //...

        public static Bitmap Open(string path)
        {
            Bitmap result = null;
            Try.Invoke(() => result = new MagickImage(path).ToBitmap(), e => Dispatch.Invoke(() => Log.Write<Document>(e)));
            return result;
        }

        public static async Task<WriteableBitmap> OpenAsync(string path)
        {
            Bitmap result = null;
            await Task.Run(() => result = Open(path));
            return result.WriteableBitmap();
        }

        #endregion
    }

    #endregion

    #region ImageDocumentStatus

    public class ImageDocumentStatus : DocumentStatus
    {
        enum Category { Format }

        [Category(Category.Format)]
        [DisplayName("Bit depth")]
        [ReadOnly]
        [Status]
        public int BitDepth => default;

        public ImageDocumentStatus(ImageDocument document) : base(document) { }
    }

    #endregion
}