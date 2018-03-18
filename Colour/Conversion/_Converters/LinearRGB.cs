using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class LinearRGBConverter : ColorConverterBase<LinearRGB>
    {
        readonly Matrix _conversionMatrix;

        public WorkingSpace TargetWorkingSpace
        {
            get;
        }

        public LinearRGBConverter() : this(default(WorkingSpace)) { }

        public LinearRGBConverter(WorkingSpace targetWorkingSpace)
        {
            TargetWorkingSpace = targetWorkingSpace == default(WorkingSpace) ? WorkingSpaces.Default : targetWorkingSpace;
            _conversionMatrix = WorkingSpace.GetXYZToRGB(TargetWorkingSpace);
        }

        static Vector Uncompand(RGB input)
        {
            var companding = input.WorkingSpace.Companding;
            var compandedVector = input.Vector;
            return new[]
            {
                companding.InverseCompanding(compandedVector[0]).Coerce(1),
                companding.InverseCompanding(compandedVector[1]).Coerce(1),
                companding.InverseCompanding(compandedVector[2]).Coerce(1)
            };
        }

        /// ------------------------------------------------------------------------------------

        public LinearRGB Convert(RGB input) => new LinearRGB(Uncompand(input), input.WorkingSpace);

        public LinearRGB Convert(XYZ input)
        {
            var inputVector = input.Vector;
            Vector uncompandedVector = _conversionMatrix.Multiply(inputVector).Coerce(Vector.New(0, 0, 0), Vector.New(1, 1, 1));
            var result = new LinearRGB(uncompandedVector, TargetWorkingSpace);
            return result;
        }
    }
#pragma warning restore 1591
}