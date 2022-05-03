using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Bucket")]
    [Icon(App.ImagePath + "Bucket.png")]
    [Serializable]
    public class BucketTool : BlendTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Bucket.png");

        byte tolerance = 32;
        [Format(RangeFormat.UpDown)]
        [Range(byte.MinValue, byte.MaxValue, (byte)1)]
        public byte Tolerance
        {
            get => tolerance;
            set => this.Change(ref tolerance, value);
        }
        
        //...

        protected override bool AssertLayer() => AssertLayer<PixelLayer>();

        public override bool OnMouseDown(Point input)
        {
            if (base.OnMouseDown(input))
            {
                TargetLayer.As<PixelLayer>().Pixels.BlendAt(Mode, input.Int32(), Get.Current<Options>().ForegroundColor.Int32(), Opacity, Tolerance);
                return true;
            }
            return false;
        }
    }
}