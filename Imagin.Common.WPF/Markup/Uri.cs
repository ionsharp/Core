using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public class Uri : MarkupExtension
    {
        public virtual string Assembly { get; set; } = null;

        public string RelativePath { get; set; }

        public Uri() : base() { }

        public Uri(string relativePath) : this()
        {
            RelativePath = relativePath;
        }

        public Uri(string assembly, string relativePath) : this(relativePath)
        {
            Assembly = assembly;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Assembly == null)
                return new System.Uri(RelativePath, UriKind.Relative);

            return Resources.Uri(Assembly, RelativePath);
        }
    }

    public class InternalUri : Uri
    {
        public override string Assembly => InternalAssembly.Name;

        public InternalUri(string relativePath) : base(relativePath) { }
    }

    //...

    public class ProjectUri : Uri
    {
        public override string Assembly => XAssembly.ShortName();

        public ProjectUri(string relativePath) : base(relativePath) { }
    }

    public class ProjectCursor : ProjectUri
    {
        public System.Drawing.Point Point { get; set; } = new System.Drawing.Point(0, 0);

        public ProjectCursor(string relativePath) : base(relativePath) { }

        public ProjectCursor(string relativePath, int x, int y) : this(relativePath) => Point = new(x, y);

        public override object ProvideValue(IServiceProvider serviceProvider)
            => base.ProvideValue(serviceProvider).To<System.Uri>().GetImage().Bitmap(ImageExtensions.Png).Cursor(Point.X, Point.Y).Convert();
    }
}