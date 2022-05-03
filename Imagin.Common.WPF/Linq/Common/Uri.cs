using System;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    public static class XUri
    {
        public static ImageSource GetImage(this Uri input)
            => (ImageSource)new ImageSourceConverter().ConvertFromString(input.OriginalString);
    }
}