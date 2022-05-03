using Imagin.Common;
using System;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Static layer")]
    [Icon(App.ImagePath + "LayerPixel.png")]
    public class StaticLayer : Layer
    {
        [field: NonSerialized]
        double displayHeight = double.NaN;
        [Hidden]
        public double DisplayHeight
        {
            get => displayHeight;
            set => this.Change(ref displayHeight, value);
        }

        [field: NonSerialized]
        double displayWidth = double.NaN;
        [Hidden]
        public double DisplayWidth
        {
            get => displayWidth;
            set => this.Change(ref displayWidth, value);
        }

        double opacity = 1;
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Opacity
        {
            get => opacity;
            set => this.Change(ref opacity, value);
        }

        [field: NonSerialized]
        WriteableBitmap source = null;
        [Hidden]
        public WriteableBitmap Source
        {
            get => source;
            set => this.Change(ref source, value);
        }

        public StaticLayer(WriteableBitmap source) : base(LayerType.Static) => Source = source;

        public override Layer Clone() => new StaticLayer(null);
    }
}