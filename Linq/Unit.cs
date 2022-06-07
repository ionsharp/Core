using Imagin.Core.Numerics;

namespace Imagin.Core.Linq;

public static class XUnit
{
    /// <summary>Converts the given value from <see cref="Unit"/> (a) to <see cref="Unit"/> (b).</summary>
    public static double Convert(this Unit a, Unit b, double value, double ppi = 72.0)
    {
        var pixels = 0d;
        switch (a)
        {
            case Unit.Pixel:
                pixels = System.Math.Round(value, 0);
                break;
            case Unit.Inch:
                pixels = System.Math.Round(value * ppi, 0);
                break;
            case Unit.Centimeter:
                pixels = System.Math.Round((value * ppi) / 2.54, 0);
                break;
            case Unit.Millimeter:
                pixels = System.Math.Round((value * ppi) / 25.4, 0);
                break;
            case Unit.Point:
                pixels = System.Math.Round((value * ppi) / 72, 0);
                break;
            case Unit.Pica:
                pixels = System.Math.Round((value * ppi) / 6, 0);
                break;
            case Unit.Twip:
                pixels = System.Math.Round((value * ppi) / 1140, 0);
                break;
            case Unit.Character:
                pixels = System.Math.Round((value * ppi) / 12, 0);
                break;
            case Unit.En:
                pixels = System.Math.Round((value * ppi) / 144.54, 0);
                break;
        }

        var inches = pixels / ppi;
        var result = pixels;

        switch (b)
        {
            case Unit.Inch:
                result = inches;
                break;
            case Unit.Centimeter:
                result = inches * 2.54;
                break;
            case Unit.Millimeter:
                result = inches * 25.4;
                break;
            case Unit.Point:
                result = inches * 72.0;
                break;
            case Unit.Pica:
                result = inches * 6.0;
                break;
            case Unit.Twip:
                result = inches * 1140.0;
                break;
            case Unit.Character:
                result = inches * 12.0;
                break;
            case Unit.En:
                result = inches * 144.54;
                break;
        }

        return result;
    }
}