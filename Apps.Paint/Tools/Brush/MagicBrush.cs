using Imagin.Apps.Paint.Effects;
using Imagin.Common;
using Imagin.Common.Colors;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    #region (abstract) MagicBrushTool

    [Serializable]
    public abstract class MagicBrushTool : BrushTool
    {
        WriteableBitmap cacheBitmap = null;

        [Hidden]
        public override BlendModes Mode
        {
            get => base.Mode;
            set => base.Mode = value;
        }

        //...

        protected abstract Color Apply(int x, int y, Color color, double factor);

        protected override void Draw(Vector2<int> point, Color color)
        {
            if (cacheBitmap != null && cacheBytes != null)
            {
                uint x2 = 0;
                for (var x = point.X; x < point.X + cacheBytes.Columns; x++, x2++)
                {
                    uint y2 = 0;
                    for (var y = point.Y; y < point.Y + cacheBytes.Rows; y++, y2++)
                    {
                        if (x >= 0 && x < Pixels.PixelWidth && y >= 0 && y < Pixels.PixelHeight)
                        {
                            if (x2 < cacheBytes.Columns && y2 < cacheBytes.Rows)
                            {
                                var factor = cacheBytes.GetValue(y2, x2).Divide(255.0);
                                if (factor > 0)
                                {
                                    var oldColor = cacheBitmap.GetPixel(x, y);
                                    var newColor = Apply(x, y, oldColor, factor * Opacity);
                                    Pixels.SetPixel(x, y, newColor);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnStarted()
            => cacheBitmap = Pixels.Clone();
    }

    #endregion

    #region CloneStampTool

    [DisplayName("Clone stamp")]
    [Icon(App.ImagePath + "CloneStamp.png")]
    [Serializable]
    public class CloneStampTool : MagicBrushTool
    {
        [Hidden]
        public override Uri CursorIcon => Resources.ProjectImage("CloneStampCursor.png");

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("CloneStamp.png");

        System.Drawing.Point2D offset = new(-10, 0);
        [Hidden]
        public System.Drawing.Point2D Offset
        {
            get => offset;
            set => this.Change(ref offset, value);
        }

        Line<int> offsetPreview = new(0, 0, 0, 0);
        [Hidden]
        public Line<int> OffsetPreview
        {
            get => offsetPreview;
            set => this.Change(ref offsetPreview, value);
        }

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            var x2 = x + Offset.X;
            var y2 = y + Offset.Y;

            if (x2 >= 0 && x2 < Pixels.PixelWidth && y2 >= 0 && y2 < Pixels.PixelHeight)
                return color.Blend(Pixels.GetPixel(x2, y2).A(factor.Multiply(255)), Mode);

            return color;
        }

        bool drawOffset = false;

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (drawOffset)
            {
                input.DrawLine(new Pen(System.Windows.Media.Brushes.Black, 1 / zoom), new Point(OffsetPreview.X1, OffsetPreview.Y1), new Point(OffsetPreview.X2, OffsetPreview.Y2));
                input.DrawLine(new Pen(System.Windows.Media.Brushes.White, 0.5 / zoom), new Point(OffsetPreview.X1, OffsetPreview.Y1), new Point(OffsetPreview.X2, OffsetPreview.Y2));
            }
        }

        public override bool OnMouseDown(Point point)
        {
            if (ModifierKeys.Alt.Pressed())
            {
                MouseDown = point;
                drawOffset = true;
                return true;
            }
            return base.OnMouseDown(point);
        }

        public override void OnMouseMove(Point point)
        {
            if (drawOffset)
            {
                MouseMove = point;

                OffsetPreview = new Line<int>(MouseDown.Value.X.Round().Int32(), MouseDown.Value.Y.Round().Int32(), MouseMove.Value.X.Round().Int32(), MouseMove.Value.Y.Round().Int32());
                Offset.X = (MouseDown.Value.X - MouseMove.Value.X).Round().Int32();
                Offset.Y = (MouseDown.Value.Y - MouseMove.Value.Y).Int32();
                return;
            }
            base.OnMouseMove(point);
        }

        public override void OnMouseUp(Point point)
        {
            if (drawOffset)
            {
                drawOffset = false;
                OffsetPreview = new Line<int>(0, 0, 0, 0);
            }

            base.OnMouseUp(point);
        }
    }

    #endregion

    #region EffectBrushTool

    [DisplayName("Effect brush")]
    [Icon(App.ImagePath + "BrushFx.png")]
    [Serializable]
    public class EffectBrushTool : MagicBrushTool
    {
        [Hidden]
        public override Uri CursorIcon => Resources.ProjectImage("EffectBrushCursor.png");

        [Hidden]
        public ListCollectionView Effects 
            => Get.Current<MainViewModel>().Effects;

        int effectIndex = -1;
        [DisplayName("Effect")]
        [Source(nameof(Effects))]
        [Style(Int32Style.Index)]
        public int EffectIndex
        {
            get => effectIndex;
            set
            {
                this.Change(ref effectIndex, value);
                Effect = (Effects != null && EffectIndex >= 0 && EffectIndex < Effects.Count ? (ImageEffect)Effects.SourceCollection.To<IList>()[EffectIndex] : null)?.Copy();
            }
        }

        ImageEffect effect = null;
        [DisplayName("Effect")]
        [Label(false)]
        public ImageEffect Effect
        {
            get => effect;
            set => this.Change(ref effect, value);
        }

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("BrushFx.png");

        protected override Color Apply(int x, int y, Color color, double factor)
            => effect?.Apply(color, factor * Opacity) ?? color;
    }

    #endregion

    #region SpongeTool

    [DisplayName("Sponge")]
    [Icon(App.ImagePath + "Sponge.png")]
    [Serializable]
    public class SpongeTool : MagicBrushTool
    {
        [Hidden]
        public override Uri CursorIcon => Resources.ProjectImage("SpongeCursor.png");

        double flow = 1;
        public double Flow
        {
            get => flow;
            set => this.Change(ref flow, value);
        }

        double intensity = 0.1;
        public double Intensity
        {
            get => intensity;
            set => this.Change(ref intensity, value);
        }

        SpongeToolModes spongeMode = SpongeToolModes.Desaturate;
        public SpongeToolModes SpongeMode
        {
            get => spongeMode;
            set => this.Change(ref spongeMode, value);
        }

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Sponge.png");

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            var hsl = HSL.From(new RGB(color));

            var increment = Intensity * Flow * factor;
            increment *= SpongeMode == SpongeToolModes.Desaturate ? -1 : 1;

            var rgb = new HSL(hsl[0], (hsl[1] + increment).Coerce(1), hsl[2]).Convert();
            return Color.FromArgb(color.A, rgb[0].Multiply(255), rgb[1].Multiply(255), rgb[2].Multiply(255));
        }
    }

    #endregion

    //...

    #region (abstract) AdjacentBrushTool : MagicBrushTool

    #region (abstract) AdjacentBrushTool

    [Serializable]
    public abstract class AdjacentBrushTool : MagicBrushTool
    {
        double strength = 0.5;
        public double Strength
        {
            get => strength;
            set => this.Change(ref strength, value);
        }
    }

    #endregion

    #region BlurTool

    [DisplayName("Blur")]
    [Icon(App.ImagePath + "Blur.png")]
    [Serializable]
    public class BlurTool : AdjacentBrushTool
    {
        [Hidden]
        public override Uri CursorIcon => Resources.ProjectImage("BlurCursor.png");

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Blur.png");

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            return color;
        }
    }

    #endregion

    #region SharpenTool

    [DisplayName("Sharpen")]
    [Icon(App.ImagePath + "Sharpen.png")]
    [Serializable]
    public class SharpenTool : AdjacentBrushTool
    {
        [Hidden]
        public override Uri CursorIcon => Resources.ProjectImage("SharpenCursor.png");

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Sharpen.png");

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            var m0 = color - Pixels.GetPixel(x - 1, y);
            var m1 = color - Pixels.GetPixel(x + 1, y);
            var m2 = color - Pixels.GetPixel(x, y - 1);
            var m3 = color - Pixels.GetPixel(x, y + 1);

            double M = Math.Max(m0.Encode(), Math.Max(m1.Encode(), Math.Max(m2.Encode(), m3.Encode()))),
                   N = M * Strength;

            var r = (color.R + N).Byte();
            var g = (color.G + N).Byte();
            var b = (color.B + N).Byte();

            return Color.FromArgb(color.A, r, g, b);
        }
    }

    #endregion

    #region SmudgeTool

    [DisplayName("Smudge")]
    [Icon(App.ImagePath + "Smudge.png")]
    [Serializable]
    public class SmudgeTool : AdjacentBrushTool
    {
        [Hidden]
        public override Uri CursorIcon => Resources.ProjectImage("SmudgeCursor.png");

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Smudge.png");

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            return color;
        }
    }

    #endregion

    #endregion

    #region (abstract) ExposureBrushTool : MagicBrushTool

    #region (abstract) ExposureBrushTool

    [DisplayName("Exposure")]
    [Serializable]
    public abstract class ExposureBrushTool : MagicBrushTool
    {
        double exposure = 0.5;
        [Range(0.0, 1.0, 0.01)]
        public double Exposure
        {
            get => exposure;
            set => this.Change(ref exposure, value);
        }

        ColorRanges range = ColorRanges.Midtones;
        [Index(0)]
        public ColorRanges Range
        {
            get => range;
            set => this.Change(ref range, value);
        }
    }

    #endregion

    #region BurnTool

    [DisplayName("Burn")]
    [Icon(App.ImagePath + "Burn.png")]
    [Serializable]
    public class BurnTool : ExposureBrushTool
    {
        [Hidden]
        public override Uri CursorIcon => Resources.ProjectImage("BurnCursor.png");

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Burn.png");

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            var newExposure = Exposure * factor;
            double r = color.R.Divide(255.0), g = color.G.Divide(255.0), b = color.B.Divide(255.0);
            switch (Range)
            {
                case ColorRanges.Highlights:
                    r *= 1 + (-newExposure / 3);
                    g *= 1 + (-newExposure / 3);
                    b *= 1 + (-newExposure / 3);
                    break;
                case ColorRanges.Midtones:
                    r = Math.Pow(r, 1 + (newExposure / 3));
                    g = Math.Pow(g, 1 + (newExposure / 3));
                    b = Math.Pow(b, 1 + (newExposure / 3));
                    break;
                case ColorRanges.Shadows:
                    r = (r - (newExposure / 3)) / (1 - (newExposure / 3));
                    g = (g - (newExposure / 3)) / (1 - (newExposure / 3));
                    b = (b - (newExposure / 3)) / (1 - (newExposure / 3));
                    break;
            }
            return Color.FromArgb(color.A, r.Multiply(255), g.Multiply(255), b.Multiply(255));
        }
    }

    #endregion

    #region DodgeTool

    [DisplayName("Dodge")]
    [Icon(App.ImagePath + "Dodge.png")]
    [Serializable]
    public class DodgeTool : ExposureBrushTool
    {
        [Hidden]
        public override Uri CursorIcon => Resources.ProjectImage("DodgeCursor.png");

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Dodge.png");

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            var newExposure = Exposure * factor;
            double r = color.R.Divide(255.0), g = color.G.Divide(255.0), b = color.B.Divide(255.0);
            switch (Range)
            {
                case ColorRanges.Highlights:
                    r *= 1 + (newExposure / 3);
                    g *= 1 + (newExposure / 3);
                    b *= 1 + (newExposure / 3);
                    break;
                case ColorRanges.Midtones:
                    r = Math.Pow(r, 1 / (1 + newExposure));
                    g = Math.Pow(g, 1 / (1 + newExposure));
                    b = Math.Pow(b, 1 / (1 + newExposure));
                    break;
                case ColorRanges.Shadows:
                    r = (newExposure / 3) + r - (newExposure / 3) * r;
                    g = (newExposure / 3) + g - (newExposure / 3) * g;
                    b = (newExposure / 3) + b - (newExposure / 3) * b;
                    break;
            }
            return Color.FromArgb(color.A, r.Multiply(255), g.Multiply(255), b.Multiply(255));
        }
    }

    #endregion

    #endregion
}