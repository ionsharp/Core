using Imagin.Colour.Primitives;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class YIQConverter : ColorConverterBase<YIQ>
    {
        public YIQ Convert(RGB input)
        {
            var y = (input.R * 0.299) + (input.G * 0.587) + (input.B * 0.114);

            double i = 0, q = 0;
	        if (input.R != input.G || input.G != input.B)
            {
		        i = (input.R * 0.596) + (input.G * -0.275) + (input.B * -0.321);
		        q = (input.R * 0.212) + (input.G * -0.528) + (input.B * 0.311);
	        }

            return new YIQ(y, i, q);
        }
    }
#pragma warning restore 1591
}