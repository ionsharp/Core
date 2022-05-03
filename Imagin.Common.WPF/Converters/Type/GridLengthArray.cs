using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common.Converters
{
    public class GridLengthArrayTypeConverter : StringTypeConverter<GridLength>
    {
        protected override int? Length => null;

        protected override GridLength Convert(string input)
        {
            switch (input.ToLower())
            {
                case "auto":
                    return new GridLength(1, GridUnitType.Auto);

                case "*":
                    return new GridLength(1, GridUnitType.Star);

                default:

                    if (input.EndsWith("*"))
                    {
                        var i = input.Replace("*", string.Empty);
                        if (i.PositiveNumber())
                            return new GridLength(double.Parse(i), GridUnitType.Star);
                    }

                    if (input.PositiveNumber())
                        return new GridLength(double.Parse(input), GridUnitType.Pixel);

                    throw new NotSupportedException();
            }
        }

        protected override object Convert(GridLength[] input) => input;
    }
}