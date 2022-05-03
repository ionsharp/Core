using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Uri), typeof(ImageSource))]
    public class UriToImageSourceConverter : Converter<Uri, ImageSource>
    {
        public static UriToImageSourceConverter Default { get; private set; } = new UriToImageSourceConverter();
        UriToImageSourceConverter() { }

        protected override ConverterValue<ImageSource> ConvertTo(ConverterData<Uri> input)
        {
            var i = new BitmapImage();

            i.BeginInit();
            i.UriSource = input.Value;
            i.EndInit();

            return i as ImageSource;
        }

        protected override ConverterValue<Uri> ConvertBack(ConverterData<ImageSource> input) => Nothing.Do;
    }
}