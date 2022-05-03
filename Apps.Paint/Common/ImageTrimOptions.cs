using Imagin.Common;

namespace Imagin.Apps.Paint
{
    public class ImageTrimOptions : Base
    {
        enum Category { Source, Target }

        ImageTrimSource source = ImageTrimSource.TransparentPixels;
        [Category(Category.Source)]
        public ImageTrimSource Source
        {
            get => source;
            set => this.Change(ref source, value);
        }

        ImageTrimTarget target = ImageTrimTarget.Top;
        [Category(Category.Target)]
        public ImageTrimTarget Target
        {
            get => target;
            set => this.Change(ref target, value);
        }

        double tolerance = 0;
        [Category(Category.Source)]
        public double Tolerance
        {
            get => tolerance;
            set => this.Change(ref tolerance, value);
        }

        public ImageTrimOptions() : base() { }
    }
}