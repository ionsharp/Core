using Imagin.Colour.Adaptation;
using Imagin.Colour.Primitives;
using Imagin.Common;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class XYZAndLMSConverter : ColorConverterBase, IColorConverter<XYZ, LMS>, IColorConverter<LMS, XYZ>
    {
        public static readonly Matrix DefaultTransformation = LMSTransformationMatrix.Bradford;

        Matrix _transformationInverse;

        Matrix _transformation;
        public Matrix Transformation
        {
            get => _transformation;
            internal set
            {
                _transformation = value;
                _transformationInverse = Transformation.Invert3By3();
            }
        }

        public XYZAndLMSConverter() : this(DefaultTransformation) { }

        public XYZAndLMSConverter(Matrix transformation) => Transformation = transformation;

        /// ------------------------------------------------------------------------------------

        public LMS Convert(XYZ input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return new LMS(Transformation.Multiply(input.Vector));
        }

        public XYZ Convert(LMS input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            
            return new XYZ(_transformationInverse.Multiply(input.Vector));
        }
    }
#pragma warning restore 1591
}