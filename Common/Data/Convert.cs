using System;

namespace Imagin.Common.Measurement
{
    public static class Convert
    {
        /// <summary>
        /// Converts a graphical unit value to another graphical unit value.
        /// </summary>
        /// <param name="Value">Number of units.</param>
        /// <param name="From">The unit to convert from.</param>
        /// <param name="To">The unit to convert to.</param>
        /// <param name="Ppi">Pixels per inch.</param>
        /// <param name="RoundTo">Decimal places to round to.</param>
        public static double ToUnit(this double Value, GraphicalUnit From, GraphicalUnit To, double Ppi = 72.0, int RoundTo = 3)
        {
            //Convert to pixels
            double Pixels = 0.0;
            switch (From)
            {
                case GraphicalUnit.Pixels:
                    Pixels = Math.Round(Value, 0);
                    break;
                case GraphicalUnit.Inches:
                    Pixels = Math.Round(Value * Ppi, 0);
                    break;
                case GraphicalUnit.Centimeters:
                    Pixels = Math.Round((Value * Ppi) / 2.54, 0);
                    break;
                case GraphicalUnit.Millimeters:
                    Pixels = Math.Round((Value * Ppi) / 25.4, 0);
                    break;
                case GraphicalUnit.Points:
                    Pixels = Math.Round((Value * Ppi) / 72, 0);
                    break;
                case GraphicalUnit.Picas:
                    Pixels = Math.Round((Value * Ppi) / 6, 0);
                    break;
                case GraphicalUnit.Twips:
                    Pixels = Math.Round((Value * Ppi) / 1140, 0);
                    break;
                case GraphicalUnit.Characters:
                    Pixels = Math.Round((Value * Ppi) / 12, 0);
                    break;
                case GraphicalUnit.Ens:
                    Pixels = Math.Round((Value * Ppi) / 144.54, 0);
                    break;
            }

            double Inches = Pixels / Ppi;

            double Result = Pixels;

            //Convert to target unit
            switch (To)
            {
                case GraphicalUnit.Inches:
                    Result = Inches;
                    break;
                case GraphicalUnit.Centimeters:
                    Result = Inches * 2.54;
                    break;
                case GraphicalUnit.Millimeters:
                    Result = Inches * 25.4;
                    break;
                case GraphicalUnit.Points:
                    Result = Inches * 72.0;
                    break;
                case GraphicalUnit.Picas:
                    Result = Inches * 6.0;
                    break;
                case GraphicalUnit.Twips:
                    Result = Inches * 1140.0;
                    break;
                case GraphicalUnit.Characters:
                    Result = Inches * 12.0;
                    break;
                case GraphicalUnit.Ens:
                    Result = Inches * 144.54;
                    break;
            }

            return Math.Round(Result, RoundTo);
        }
    }
}
