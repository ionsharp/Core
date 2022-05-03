using Imagin.Apps.Paint.Effects;
using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Pixel layer")]
    [Icon(App.ImagePath + "LayerPixel.png")]
    [Serializable]
    public class PixelLayer : VisualLayer
    {
        enum Category { Save, Size }

        [Hidden]
        public override PointCollection Bounds
        {
            get
            {
                var result = new PointCollection
                {
                    new System.Windows.Point(X, Y),
                    new System.Windows.Point(X + Pixels.PixelWidth, Y),
                    new System.Windows.Point(X + Pixels.PixelWidth, Y + Pixels.PixelHeight),
                    new System.Windows.Point(X, Y + Pixels.PixelHeight)
                };
                return result;
            }
        }

        [Category(Category.Size)]
        [ReadOnly]
        public virtual int Height => Pixels.PixelHeight;

        [Category(Category.Size)]
        [ReadOnly]
        public virtual int Width => Pixels.PixelWidth;

        //...

        public PixelLayer() : base(LayerType.Pixel, default) { }

        public PixelLayer(string name, Bitmap pixels, params ImageEffect[] effects) : this(name, pixels.WriteableBitmap(), effects)
        {
        }

        public PixelLayer(string name, WriteableBitmap pixels, params ImageEffect[] effects) : base(LayerType.Pixel, name)
        {
            Pixels = new(pixels);
            effects?.ForEach(i => Style.Effects.Add(i));
        }

        public PixelLayer(string name, Size size, params ImageEffect[] effects) : base(LayerType.Pixel, name)
        {
            Pixels = new(BitmapFactory.New(size));
            effects?.ForEach(i => Style.Effects.Add(i));
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Pixels):
                    this.Changed(() => Height);
                    this.Changed(() => Width);
                    break;
            }
        }

        //...

        public void RasterizeStyle()
        {
            Pixels.ForEach((x, y, color) =>
            {
                var fill = (Style.Fill * 255).Coerce(255).Round().Byte();
                color = color.A(fill);

                Style.Effects.ForEach(i => color = i.Apply(color));

                if (!Style.Channels.HasFlag(RedGreenBlue.Red))
                    color = color.R(0);

                if (!Style.Channels.HasFlag(RedGreenBlue.Green))
                    color = color.G(0);

                if (!Style.Channels.HasFlag(RedGreenBlue.Blue))
                    color = color.B(0);

                var opacity = (Style.Opacity * 255).Coerce(255).Round().Byte();
                color = color.A(opacity);

                return color;
            });
            Style.Clear();
        }

        //...

        public override Layer Clone()
        {
            var result = new PixelLayer()
            {
                IsLocked = IsLocked,
                IsVisible = IsVisible,
                Name = Name,
                Pixels = XWriteableBitmap.Clone(Pixels),
                X = X,
                Y = Y,
            };
            result.Style.Paste(Style);
            return result;
        }

        ICommand saveAsBrushCommand;
        [Category(Category.Save)]
        [DisplayName("Save as brush")]
        [Icon(App.ImagePath + "ConvertToBrush.png")]
        [Tool]
        public ICommand SaveAsBrushCommand
            => saveAsBrushCommand ??= new RelayCommand(() => Panel.Find<BrushesPanel>().SelectedGroup.Add(new Brush(Name, Pixels)), () => Panel.Find<BrushesPanel>().SelectedGroup != null);

        ICommand saveAsMatrixCommand;
        [Category(Category.Save)]
        [DisplayName("Save as matrix")]
        [Icon(App.ImagePath + "ConvertToMatrix.png")]
        [Tool]
        public ICommand SaveAsMatrixCommand
            => saveAsMatrixCommand ??= new RelayCommand(() => Panel.Find<MatricesPanel>().SelectedGroup.Add(new Matrix()), () => Panel.Find<MatricesPanel>().SelectedGroup != null);
    }
}