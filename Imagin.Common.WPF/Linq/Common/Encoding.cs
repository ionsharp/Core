using Imagin.Common.Text;

namespace Imagin.Common.Linq
{
    public static class XEncoding
    {
        public static System.Text.Encoding Convert(this Encoding input)
        {
            return input switch
            {
                Encoding.ASCII => System.Text.Encoding.ASCII,
                Encoding.Unicode => System.Text.Encoding.Unicode,
                Encoding.UTF32 => System.Text.Encoding.UTF32,
                Encoding.UTF7 => System.Text.Encoding.UTF7,
                Encoding.UTF8 => System.Text.Encoding.UTF8,
                _ => default,
            };
        }
    }
}