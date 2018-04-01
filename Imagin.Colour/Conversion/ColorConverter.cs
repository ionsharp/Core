using Imagin.Colour.Adaptation;
using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Colour.Conversion
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ColorConverter
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly XYZ DefaultWhitePoint = Illuminants.D65;

              XYZConverter _lastXYZConverter;

        LinearRGBConverter _lastLinearRGBConverter;

        XYZAndLMSConverter _cachedXYZAndLMSConverter;

        bool IsChromaticAdaptationPerformed => WhitePoint != null && ChromaticAdaptation != null;

        /// <summary>
        /// Gets or sets the <see cref="IChromaticAdaptation"/> to perform; if <see langword="null"/>, no <see cref="IChromaticAdaptation"/> is performed.
        /// </summary>
        public IChromaticAdaptation ChromaticAdaptation
        {
            get; set;
        }

        Matrix _transformationMatrix;
        /// <summary>
        /// 
        /// </summary>
        public Matrix LMSTransformationMatrix
        {
            get => _transformationMatrix;
            set
            {
                _transformationMatrix = value;
                if (_cachedXYZAndLMSConverter == null)
                {
                    _cachedXYZAndLMSConverter = new XYZAndLMSConverter(value);
                }
                else _cachedXYZAndLMSConverter.Transformation = value;
            }
        }

        /// <summary>
        /// White point used for chromatic adaptation in conversions from/to XYZ color space.
        /// When null, no adaptation will be performed.
        /// <seealso cref="TargetLabWhitePoint"/>
        /// </summary>
        public XYZ WhitePoint
        {
            get; set;
        }

        /// <summary>
        /// White point used *when creating* Lab/LChab colors. (Lab/LChab colors on the input already contain the white point information)
        /// Defaults to: <see cref="Lab.DefaultIlluminant"/>.
        /// </summary>
        public XYZ TargetLabWhitePoint
        {
            get; set;
        }

        /// <summary>
        /// White point used *when creating* Luv/LChuv colors. (Luv/LChuv colors on the input already contain the white point information)
        /// Defaults to: <see cref="Luv.DefaultIlluminant"/>.
        /// </summary>
        public XYZ TargetLuvWhitePoint
        {
            get; set;
        }

        /// <summary>
        /// White point used *when creating* HunterLab colors. (HunterLab colors on the input already contain the white point information)
        /// Defaults to: <see cref="HunterLab.DefaultIlluminant"/>.
        /// </summary>
        public XYZ TargetHunterLabWhitePoint
        {
            get; set;
        }

        /// <summary>
        /// Working space used *when creating* RGB colors. (RGB colors on the input already contain the working space information)
        /// Defaults to: <see cref="WorkingSpaces.Default"/>.
        /// </summary>
        public WorkingSpace TargetWorkingSpace
        {
            get; set;
        }

        #endregion

        #region ColorConverter

        /// <summary>
        /// 
        /// </summary>
        public ColorConverter()
        {
            WhitePoint = DefaultWhitePoint;
            LMSTransformationMatrix = XYZAndLMSConverter.DefaultTransformation;
            ChromaticAdaptation = new VonKriesChromaticAdaptation(_cachedXYZAndLMSConverter, _cachedXYZAndLMSConverter);
            TargetLabWhitePoint = Lab.DefaultIlluminant;
            TargetHunterLabWhitePoint = HunterLab.DefaultIlluminant;
            TargetLuvWhitePoint = Luv.DefaultIlluminant;
            TargetWorkingSpace = WorkingSpaces.Default;
        }

        #endregion

        #region Adaptation

        /// <summary>
        /// Adapts Lab color from the source white point to white point set in <see cref="TargetLabWhitePoint"/>.
        /// </summary>
        public Lab Adapt(Lab color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            if (!IsChromaticAdaptationPerformed)
                throw new InvalidOperationException("Cannot perform chromatic adaptation, provide chromatic adaptation method and white point.");

            if (color.Illuminant.Equals(TargetLabWhitePoint))
                return color;

            return Convert<Lab>(Convert<XYZ>(color));
        }

        /// <summary>
        /// Adapts LChab color from the source white point to white point set in <see cref="TargetLabWhitePoint"/>.
        /// </summary>
        public LChab Adapt(LChab color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            if (!IsChromaticAdaptationPerformed)
                throw new InvalidOperationException("Cannot perform chromatic adaptation, provide chromatic adaptation method and white point.");

            if (color.Illuminant.Equals(TargetLabWhitePoint))
                return color;

            return Convert<LChab>(Convert<Lab>(color));
        }

        /// <summary>
        /// Adapts linear RGB color from the source working space to working space set in <see cref="TargetWorkingSpace"/>.
        /// </summary>
        public LinearRGB Adapt(LinearRGB color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            if (!IsChromaticAdaptationPerformed)
                throw new InvalidOperationException("Cannot perform chromatic adaptation, provide chromatic adaptation method and white point.");

            if (color.WorkingSpace.Equals(TargetWorkingSpace))
                return color;

            // conversion to XYZ
            var converterToXYZ = GetLinearRGBToXYZConverter(color.WorkingSpace);
            var unadapted = converterToXYZ.Convert(color);

            // adaptation
            var adapted = ChromaticAdaptation.Transform(unadapted, color.WorkingSpace.Illuminant, TargetWorkingSpace.Illuminant);

            // conversion back to RGB
            var converterToRGB = GetXYZToLinearRGBConverter(TargetWorkingSpace);
            var result = converterToRGB.Convert(adapted);

            return result;
        }

        /// <summary>
        /// Adapts Luv color from the source white point to white point set in <see cref="TargetLuvWhitePoint"/>.
        /// </summary>
        public Luv Adapt(Luv color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            if (!IsChromaticAdaptationPerformed)
                throw new InvalidOperationException("Cannot perform chromatic adaptation, provide chromatic adaptation method and white point.");

            if (color.Illuminant.Equals(TargetLuvWhitePoint))
                return color;

            return Convert<Luv>(Convert<XYZ>(color));
        }

        /// <summary>
        /// Adapts Lab color from the source white point to white point set in <see cref="TargetHunterLabWhitePoint"/>.
        /// </summary>
        public HunterLab Adapt(HunterLab color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            if (!IsChromaticAdaptationPerformed)
                throw new InvalidOperationException("Cannot perform chromatic adaptation, provide chromatic adaptation method and white point.");

            if (color.Illuminant.Equals(TargetHunterLabWhitePoint))
                return color;

            return Convert<HunterLab>(Convert<XYZ>(color));
        }

        /// <summary>
        /// Adapts RGB color from the source working space to working space set in <see cref="TargetWorkingSpace"/>.
        /// </summary>
        public RGB Adapt(RGB color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            var ilinear = Convert<LinearRGB>(color);
            var olinear = Adapt(ilinear);
            var compandedOutput = Convert<RGB>(olinear);

            return compandedOutput;
        }

        /// <summary>
        /// Performs chromatic adaptation of given XYZ color. Target white point is <see cref="WhitePoint"/>.
        /// </summary>
        public XYZ Adapt(XYZ color, XYZ sourceWhitePoint)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            if (sourceWhitePoint == null)
                throw new ArgumentNullException(nameof(sourceWhitePoint));

            if (!IsChromaticAdaptationPerformed)
                throw new InvalidOperationException("Cannot perform chromatic adaptation, provide chromatic adaptation method and white point.");

            var result = ChromaticAdaptation.Transform(color, sourceWhitePoint, WhitePoint);
            return result;
        }

        #endregion

        #region Conversion

        /// <summary>
        /// 
        /// </summary>
        public static readonly Dictionary<Type, Type> Converters = new Dictionary<Type, Type>()
        {
            {typeof(CMY),       typeof(CMYConverter)},
            {typeof(CMYK),      typeof(CMYKConverter)},
            {typeof(HCG),       typeof(HCGConverter)},
            {typeof(HSB),       typeof(HSBConverter)},
            {typeof(HSI),       typeof(HSIConverter)},
            {typeof(HSL),       typeof(HSLConverter)},
            {typeof(HSM),       typeof(HSMConverter)},
            {typeof(HSP),       typeof(HSPConverter)},
            {typeof(HunterLab), typeof(HunterLabConverter)},
            {typeof(HWB),       typeof(HWBConverter)},
            {typeof(Lab),       typeof(LabConverter)},
            {typeof(LChab),     typeof(LChabConverter)},
            {typeof(LChuv),     typeof(LChuvConverter)},
            {typeof(LinearRGB), typeof(LinearRGBConverter)},
          //{typeof(LMS),       typeof(LMSConverter)},
            {typeof(Luv),       typeof(LuvConverter)},
            {typeof(RG),        typeof(RGConverter)},
            {typeof(RGB),       typeof(RGBConverter)},
            {typeof(TSL),       typeof(TSLConverter)},
            {typeof(xvYCC),     typeof(xvYCCConverter)},
            {typeof(xyY),       typeof(xyYConverter)},
            {typeof(XYZ),       typeof(XYZConverter)},
            {typeof(YCbCr),     typeof(YCbCrConverter)},
            {typeof(YcCbcCrc),  typeof(YcCbcCrcConverter)},
            {typeof(YCoCg),     typeof(YCoCgConverter)},
            {typeof(YDbDr),     typeof(YDbDrConverter)},
            {typeof(YES),       typeof(YESConverter)},
            {typeof(YIQ),       typeof(YIQConverter)},
            {typeof(YPbPr),     typeof(YPbPrConverter)},
            {typeof(YUV),       typeof(YUVConverter)},
        };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public TOutput Convert<TOutput>(IColor input) where TOutput : IColor
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.GetType() == typeof(TOutput))
                return (TOutput)input;

            var target = typeof(TOutput);
            var result = default(IColor);

            var NotImplementedErrorMessage = new Func<string, string>(message => "Conversion between '{0}' and '{1}' does not exist{2}.".F(input?.GetType().FullName, target.FullName, message));

            //The following should be evaluated first

            #region -> HunterLab

            if (target == typeof(HunterLab))
            {
                if (input is Lab)
                    result = Convert<HunterLab>(Convert<XYZ>((Lab)input));

                else if (input is LChab)
                    result = Convert<HunterLab>(Convert<XYZ>((LChab)input));

                else if (input is LChuv)
                    result = Convert<HunterLab>(Convert<XYZ>((LChuv)input));

                else if (input is LinearRGB)
                    result = Convert<HunterLab>(Convert<XYZ>((LinearRGB)input));

                else if (input is LMS)
                    result = Convert<HunterLab>(Convert<XYZ>((LMS)input));

                else if (input is Luv)
                    result = Convert<HunterLab>(Convert<XYZ>((Luv)input));

                else if (input is RGB)
                    result = Convert<HunterLab>(Convert<XYZ>((RGB)input));

                else if (input is xyY)
                    result = Convert<HunterLab>(Convert<XYZ>((xyY)input));

                else if (input is XYZ)
                {
                    var adapted = !WhitePoint.Equals(TargetHunterLabWhitePoint) && IsChromaticAdaptationPerformed ? ChromaticAdaptation.Transform((XYZ)input, WhitePoint, TargetHunterLabWhitePoint) : (XYZ)input;
                    result = new HunterLabConverter(TargetHunterLabWhitePoint).Convert(adapted);
                }
            }

            #endregion

            #region -> Lab

            else if (target == typeof(Lab))
            {
                if (input is HunterLab)
                    result = Convert<Lab>(Convert<XYZ>((HunterLab)input));

                else if (input is LChab)
                {
                    var unadapted = new LabConverter().Convert((LChab)input);
                    result = !IsChromaticAdaptationPerformed ? unadapted : Adapt(unadapted);
                }

                else if (input is LChuv)
                    result = Convert<Lab>(Convert<XYZ>((LChuv)input));

                else if (input is LinearRGB)
                    result = Convert<Lab>(Convert<XYZ>((LinearRGB)input));

                else if (input is LMS)
                    result = Convert<Lab>(Convert<XYZ>((LMS)input));

                else if (input is Luv)
                    result = Convert<Lab>(Convert<XYZ>((Luv)input));

                else if (input is RGB)
                    result = Convert<Lab>(Convert<XYZ>((RGB)input));

                else if (input is xyY)
                    result = Convert<Lab>(Convert<XYZ>((xyY)input));

                else if (input is XYZ)
                {
                    var adapted = !WhitePoint.Equals(TargetLabWhitePoint) && IsChromaticAdaptationPerformed ? ChromaticAdaptation.Transform((XYZ)input, WhitePoint, TargetLabWhitePoint) : (XYZ)input;
                    result = new LabConverter(TargetLabWhitePoint).Convert(adapted);
                }
            }

            #endregion

            #region -> LChab

            else if (target == typeof(LChab))
            {
                if (input is HunterLab)
                    result = Convert<LChab>(Convert<XYZ>((HunterLab)input));

                else if (input is Lab)
                {
                    var adapted = IsChromaticAdaptationPerformed ? Adapt((Lab)input) : (Lab)input;
                    result = new LChabConverter().Convert(adapted);
                }

                else if (input is LChuv)
                    result = Convert<LChab>(Convert<XYZ>((LChuv)input));

                else if (input is LinearRGB)
                    result = Convert<LChab>(Convert<XYZ>((LinearRGB)input));

                else if (input is LMS)
                    result = Convert<LChab>(Convert<XYZ>((LMS)input));

                else if (input is Luv)
                    result = Convert<LChab>(Convert<XYZ>((Luv)input));

                else if (input is RGB)
                    result = Convert<LChab>(Convert<XYZ>((RGB)input));

                else if (input is xyY)
                    result = Convert<LChab>(Convert<XYZ>((xyY)input));

                else if (input is XYZ)
                    result = Convert<LChab>(Convert<Lab>((XYZ)input));
            }

            #endregion

            #region -> LChuv

            else if (target == typeof(LChuv))
            {
                if (input is HunterLab)
                    result = Convert<LChuv>(Convert<XYZ>((HunterLab)input));

                else if (input is Lab)
                    result = Convert<LChuv>(Convert<XYZ>((Lab)input));

                else if (input is LChab)
                    result = Convert<LChuv>(Convert<XYZ>((LChab)input));

                else if (input is LinearRGB)
                    result = Convert<LChuv>(Convert<XYZ>((LinearRGB)input));

                else if (input is LMS)
                    result = Convert<LChuv>(Convert<XYZ>((LMS)input));

                else if (input is Luv)
                {
                    var adapted = IsChromaticAdaptationPerformed ? Adapt((Luv)input) : (Luv)input;
                    result = new LChuvConverter().Convert(adapted);
                }

                else if (input is RGB)
                    result = Convert<LChuv>(Convert<XYZ>((RGB)input));

                else if (input is xyY)
                    result = Convert<LChuv>(Convert<XYZ>((xyY)input));

                else if (input is XYZ)
                    result = Convert<LChuv>(Convert<Luv>((XYZ)input));
            }

            #endregion

            #region -> LinearRGB

            else if (target == typeof(LinearRGB))
            {
                if (input is HunterLab)
                    result = Convert<LinearRGB>(Convert<XYZ>((HunterLab)input));

                else if (input is Lab)
                    result = Convert<LinearRGB>(Convert<XYZ>((Lab)input));

                else if (input is LChab)
                    result = Convert<LinearRGB>(Convert<XYZ>((LChab)input));

                else if (input is LChuv)
                    result = Convert<LinearRGB>(Convert<XYZ>((LChuv)input));

                else if (input is LMS)
                    result = Convert<LinearRGB>(Convert<XYZ>((LMS)input));

                else if (input is Luv)
                    result = Convert<LinearRGB>(Convert<XYZ>((Luv)input));

                else if (input is RGB)
                    result = new LinearRGBConverter().Convert((RGB)input);

                else if (input is xyY)
                    result = Convert<LinearRGB>(Convert<XYZ>((xyY)input));

                else if (input is XYZ)
                {
                    var adapted = TargetWorkingSpace.Illuminant.Equals(WhitePoint) || !IsChromaticAdaptationPerformed ? (XYZ)input : ChromaticAdaptation.Transform((XYZ)input, WhitePoint, TargetWorkingSpace.Illuminant);
                    result = GetXYZToLinearRGBConverter(TargetWorkingSpace).Convert(adapted);
                }
            }

            #endregion

            #region -> LMS

            else if (target == typeof(LMS))
            {
                if (input is HunterLab)
                    result = Convert<LMS>(Convert<XYZ>((HunterLab)input));

                else if (input is Lab)
                    result = Convert<LMS>(Convert<XYZ>((Lab)input));

                else if (input is LChab)
                    result = Convert<LMS>(Convert<XYZ>((LChab)input));

                else if (input is LChuv)
                    result = Convert<LMS>(Convert<XYZ>((LChuv)input));

                else if (input is LinearRGB)
                    result = Convert<LMS>(Convert<XYZ>((LinearRGB)input));

                else if (input is Luv)
                    result = Convert<LMS>(Convert<XYZ>((Luv)input));

                else if (input is RGB)
                    result = Convert<LMS>(Convert<XYZ>((RGB)input));

                else if (input is xyY)
                    result = Convert<LMS>(Convert<XYZ>((xyY)input));

                else if (input is XYZ)
                    result = _cachedXYZAndLMSConverter.Convert((XYZ)input);
            }

            #endregion

            #region -> Luv

            else if (target == typeof(Luv))
            {
                if (input is HunterLab)
                    result = Convert<Luv>(Convert<XYZ>((HunterLab)input));

                else if (input is Lab)
                    result = Convert<Luv>(Convert<XYZ>((Lab)input));

                else if (input is LChab)
                    result = Convert<Luv>(Convert<XYZ>((LChab)input));

                else if (input is LChuv)
                {
                    var unadapted = new LuvConverter().Convert((LChuv)input);
                    result = !IsChromaticAdaptationPerformed ? unadapted : Adapt(unadapted);
                }

                else if (input is LinearRGB)
                    result = Convert<Luv>(Convert<XYZ>((LinearRGB)input));

                else if (input is LMS)
                    result = Convert<Luv>(Convert<XYZ>((LMS)input));

                else if (input is RGB)
                    result = Convert<Luv>(Convert<XYZ>((RGB)input));

                else if (input is xyY)
                    result = Convert<Luv>(Convert<XYZ>((xyY)input));

                else if (input is XYZ)
                {
                    var adapted = !WhitePoint.Equals(TargetLuvWhitePoint) && IsChromaticAdaptationPerformed ? ChromaticAdaptation.Transform((XYZ)input, WhitePoint, TargetLuvWhitePoint) : (XYZ)input;
                    result = new LuvConverter(TargetLuvWhitePoint).Convert(adapted);
                }
            }

            #endregion

            #region -> RGB

            else if (target == typeof(RGB))
            {
                if (input is CMY)
                    result = new RGBConverter().Convert((CMY)input);

                else if(input is CMYK)
                    result = new RGBConverter().Convert((CMYK)input);

                else if (input is HCG)
                    result = new RGBConverter().Convert((HCG)input);

                else if (input is HSB)
                    result = new RGBConverter().Convert((HSB)input);

                else if (input is HSI)
                    result = new RGBConverter().Convert((HSI)input);

                else if (input is HSL)
                    result = new RGBConverter().Convert((HSL)input);

                else if (input is HSM)
                    result = new RGBConverter().Convert((HSM)input);

                else if (input is HSP)
                    result = new RGBConverter().Convert((HSP)input);

                else if (input is HunterLab)
                    result = Convert<RGB>(Convert<XYZ>((HunterLab)input));

                else if (input is HWB)
                    result = new RGBConverter().Convert((HWB)input);

                else if (input is Lab)
                    result = Convert<RGB>(Convert<XYZ>((Lab)input));

                else if (input is LChab)
                    result = Convert<RGB>(Convert<XYZ>((LChab)input));

                else if (input is LChuv)
                    result = Convert<RGB>(Convert<XYZ>((LChuv)input));

                else if (input is LinearRGB)
                    result = new RGBConverter().Convert((LinearRGB)input);

                else if (input is LMS)
                    result = Convert<RGB>(Convert<XYZ>((LMS)input));

                else if (input is Luv)
                    result = Convert<RGB>(Convert<XYZ>((Luv)input));

                else if (input is RG)
                    result = new RGBConverter().Convert((RG)input);

                else if (input is TSL)
                    result = new RGBConverter().Convert((TSL)input);

                else if (input is xyY)
                    result = Convert<RGB>(Convert<XYZ>((xyY)input));

                else if (input is XYZ)
                    result = Convert<RGB>(Convert<LinearRGB>((XYZ)input));

                else if (input is YCoCg)
                    result = new RGBConverter().Convert((YCoCg)input);

                else if (input is YES)
                    result = new RGBConverter().Convert((YES)input);

                else if (input is YIQ)
                    result = new RGBConverter().Convert((YIQ)input);

                else if (input is YUV)
                    result = new RGBConverter().Convert((YUV)input);
            }

            #endregion

            #region -> xyY

            else if (target == typeof(xyY))
            {
                if (input is HunterLab)
                    result = Convert<xyY>(Convert<XYZ>((HunterLab)input));

                else if (input is Lab)
                    result = Convert<xyY>(Convert<XYZ>((Lab)input));

                else if (input is LChab)
                    result = Convert<xyY>(Convert<XYZ>((LChab)input));

                else if (input is LChuv)
                    result = Convert<xyY>(Convert<XYZ>((LChuv)input));

                else if (input is LinearRGB)
                    result = Convert<xyY>(Convert<XYZ>((LinearRGB)input));

                else if (input is LMS)
                    result = Convert<xyY>(Convert<XYZ>((LMS)input));

                else if (input is Luv)
                    result = Convert<xyY>(Convert<XYZ>((Luv)input));

                else if (input is RGB)
                    result = Convert<xyY>(Convert<XYZ>((RGB)input));

                else if (input is XYZ)
                    result = new xyYConverter().Convert((XYZ)input);
            }

            #endregion

            #region -> XYZ

            else if (target == typeof(XYZ))
            {
                if (input is HunterLab)
                {
                    var unadapted = new XYZConverter().Convert((HunterLab)input);

                    result
                        = ((HunterLab)input).Illuminant.Equals(WhitePoint) || !IsChromaticAdaptationPerformed
                        ? unadapted
                        : Adapt(unadapted, ((HunterLab)input).Illuminant);
                }

                else if (input is Lab)
                {
                    var unadapted = new XYZConverter().Convert((Lab)input);

                    result
                        = ((Lab)input).Illuminant.Equals(WhitePoint) || !IsChromaticAdaptationPerformed
                        ? unadapted
                        : Adapt(unadapted, ((Lab)input).Illuminant);
                }

                else if (input is LChab)
                {
                    var _input = new LabConverter().Convert((LChab)input);
                    result = Convert<XYZ>(_input);
                }

                else if (input is LChuv)
                {
                    var _input = new LuvConverter().Convert((LChuv)input);
                    result = Convert<XYZ>(_input);
                }

                else if (input is LinearRGB)
                {
                    var unadapted = GetLinearRGBToXYZConverter(((LinearRGB)input).WorkingSpace).Convert((LinearRGB)input);

                    result
                        = ((LinearRGB)input).WorkingSpace.Illuminant.Equals(WhitePoint) || !IsChromaticAdaptationPerformed
                        ? unadapted
                        : Adapt(unadapted, ((LinearRGB)input).WorkingSpace.Illuminant);
                }

                else if (input is LMS)
                    result = _cachedXYZAndLMSConverter.Convert((LMS)input);

                else if (input is Luv)
                {
                    var unadapted = new XYZConverter().Convert((Luv)input);

                    result
                        = ((Luv)input).Illuminant.Equals(WhitePoint) || !IsChromaticAdaptationPerformed
                        ? unadapted
                        : Adapt(unadapted, ((Luv)input).Illuminant);
                }

                else if (input is RGB)
                {
                    var _input = new LinearRGBConverter().Convert((RGB)input);
                    result = Convert<XYZ>(_input);
                }

                else if (input is xyY)
                    result = new XYZConverter().Convert((xyY)input);
            }

            #endregion

            //All others should be converted dynamically

            #region -> (Other)

            else
            {
                var  converter = Converters[target]?.CreateInstance<ColorConverterBase>();
                var _converter = (dynamic)converter;

                try
                {
                    result = _converter.Convert((dynamic)input);
                }
                catch (Exception e)
                {
                    //If the converter does not define a conversion from the input to the output, the conversion has not been implemented!
                    throw new NotImplementedException(NotImplementedErrorMessage(" ({0})".F(e.Message)));
                }
            }

            #endregion

            if (result == null)
                throw new NotImplementedException(NotImplementedErrorMessage(string.Empty));

            return (TOutput)result;
        }

        #endregion

        #region Methods

        XYZConverter GetLinearRGBToXYZConverter(WorkingSpace workingSpace)
        {
            if (_lastXYZConverter != null &&
                _lastXYZConverter.SourceWorkingSpace.Equals(workingSpace))
                return _lastXYZConverter;

            return _lastXYZConverter = new XYZConverter(workingSpace);
        }

        LinearRGBConverter GetXYZToLinearRGBConverter(WorkingSpace workingSpace)
        {
            if (_lastLinearRGBConverter != null &&
                _lastLinearRGBConverter.TargetWorkingSpace.Equals(workingSpace))
                return _lastLinearRGBConverter;

            return _lastLinearRGBConverter = new LinearRGBConverter(workingSpace);
        }

        #endregion
    }
}
