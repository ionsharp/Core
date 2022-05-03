using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Apps.Paint
{
    public class ImageRotateOptions : Base
    {
        enum Category { Angle, Stretch }

        double angle = 0;
        [Category(Category.Angle)]
        public double Angle
        {
            get => angle;
            set => this.Change(ref angle, value);
        }

        Interpolations interpolation = Interpolations.Bilinear;
        [Category(Category.Stretch)]
        public Interpolations Interpolation
        {
            get => interpolation;
            set => this.Change(ref interpolation, value);
        }

        public ImageRotateOptions() : base() { }
    }
}