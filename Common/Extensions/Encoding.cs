using Imagin.Common.Text;

namespace Imagin.Common.Extensions
{
    public static class EncodingExtensions
    {
        public static System.Text.Encoding ToComplement(this Encoding ToConvert)
        {
            switch (ToConvert)
            {
                case Encoding.ASCII:
                    return System.Text.Encoding.ASCII;
                case Encoding.Unicode:
                    return System.Text.Encoding.Unicode;
                case Encoding.UTF32:
                    return System.Text.Encoding.UTF32;
                case Encoding.UTF7:
                    return System.Text.Encoding.UTF7;
                case Encoding.UTF8:
                    return System.Text.Encoding.UTF8;
            }
            return System.Text.Encoding.Default;
        }
    }
}
