using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Text;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class BulletMultiConverter : MultiConverter<string>
    {
        public static BulletMultiConverter Default { get; private set; } = new BulletMultiConverter();
        BulletMultiConverter() { }

        //...

        object TryConvert(Bullets bullet, double index)
        {
            object result = Binding.DoNothing;
            Try.Invoke(out result, () =>
            {
                var i = index.Int32();
                return bullet switch
                {
                    Bullets.LetterUpperPeriod => $"{i.Letter()}.".ToUpper(),
                    Bullets.LetterUpperParenthesis => $"{i.Letter()})".ToUpper(),
                    Bullets.LetterLowerPeriod => $"{i.Letter()}.".ToLower(),
                    Bullets.LetterLowerParenthesis => $"{i.Letter()})".ToLower(),
                    Bullets.NumberPeriod => $"{index}.",
                    Bullets.NumberParenthesis => $"{index})",
                    Bullets.RomanNumberUpperPeriod => $"{i.Roman()}.".ToUpper(),
                    Bullets.RomanNumberUpperParenthesis => $"{i.Roman()})".ToUpper(),
                    Bullets.RomanNumberLowerPeriod => $"{i.Roman()}.".ToLower(),
                    Bullets.RomanNumberLowerParenthesis => $"{i.Roman()})".ToLower(),
                    _ => throw new NotSupportedException(),
                };
            });
            return result;
        }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0)
            {
                if (values[0] is Bullets bullet)
                {
                    switch (values.Length)
                    {
                        case 1:
                            return TryConvert(bullet, 1);

                        case 2:
                            double.TryParse($"{values[1]}", out double index);
                            return TryConvert(bullet, index);
                    }
                }
            }
            return Binding.DoNothing;
        }
    }
}