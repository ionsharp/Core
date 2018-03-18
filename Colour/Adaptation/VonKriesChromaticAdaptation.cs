using Imagin.Colour.Conversion;
using Imagin.Colour.Primitives;
using Imagin.Common;
using System;

namespace Imagin.Colour.Adaptation
{
    /// <summary>
    /// Basic implementation of the von Kries chromatic adaptation model
    /// </summary>
    /// <remarks>
    /// Transformation described here:
    /// http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
    /// </remarks>
    public class VonKriesChromaticAdaptation : IChromaticAdaptation
    {
        readonly IColorConverter<XYZ, LMS> _conversionToLMS;

        readonly IColorConverter<LMS, XYZ> _conversionToXYZ;

        XYZ _lastSourceWhitePoint;

        XYZ _lastTargetWhitePoint;

        Matrix _cachedDiagonalMatrix;

        VonKriesChromaticAdaptation(XYZAndLMSConverter converter) : this(converter, converter) { }

        /// <summary>
        /// 
        /// </summary>
        public VonKriesChromaticAdaptation() : this(new XYZAndLMSConverter()) { }

        /// <summary>
        /// Transformation matrix used for the conversion (definition of the cone response domain).
        /// <see cref="LMSTransformationMatrix"/>
        /// </summary>
        public VonKriesChromaticAdaptation(Matrix transformationMatrix) : this(new XYZAndLMSConverter(transformationMatrix)) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversionToLMS"></param>
        /// <param name="conversionToXYZ"></param>
        public VonKriesChromaticAdaptation(IColorConverter<XYZ, LMS> conversionToLMS, IColorConverter<LMS, XYZ> conversionToXYZ)
        {
            if (conversionToLMS == null)
                throw new ArgumentNullException(nameof(conversionToLMS));

            if (conversionToXYZ == null)
                throw new ArgumentNullException(nameof(conversionToXYZ));

            _conversionToLMS = conversionToLMS;
            _conversionToXYZ = conversionToXYZ;
        }

        /// <summary>
        /// Transforms XYZ color to destination reference white.
        /// </summary>
        public XYZ Transform(XYZ sourceColor, XYZ sourceWhitePoint, XYZ targetWhitePoint)
        {
            if (sourceColor == null)
                throw new ArgumentNullException(nameof(sourceColor));

            if (sourceWhitePoint == null)
                throw new ArgumentNullException(nameof(sourceWhitePoint));

            if (targetWhitePoint == null)
                throw new ArgumentNullException(nameof(targetWhitePoint));

            if (sourceWhitePoint.Equals(targetWhitePoint))
                return sourceColor;

            var sourceColorLMS = _conversionToLMS.Convert(sourceColor);

            if (sourceWhitePoint != _lastSourceWhitePoint || targetWhitePoint != _lastTargetWhitePoint)
            {
                var sourceWhitePointLMS = _conversionToLMS.Convert(sourceWhitePoint);
                var targetWhitePointLMS = _conversionToLMS.Convert(targetWhitePoint);

                _cachedDiagonalMatrix = Matrix.Diagonal(targetWhitePointLMS.L / sourceWhitePointLMS.L, targetWhitePointLMS.M / sourceWhitePointLMS.M, targetWhitePointLMS.S / sourceWhitePointLMS.S);
                _lastSourceWhitePoint = sourceWhitePoint;
                _lastTargetWhitePoint = targetWhitePoint;
            }

            var targetColorLMS = new LMS(_cachedDiagonalMatrix.Multiply(sourceColorLMS.Vector));
            var targetColor = _conversionToXYZ.Convert(targetColorLMS);
            return targetColor;
        }
    }
}
