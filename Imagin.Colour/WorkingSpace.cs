using Imagin.Colour.Compression;
using Imagin.Colour.Primitives;
using Imagin.Common;
using System;

namespace Imagin.Colour
{
    /// <summary>
    /// 
    /// </summary>
    public struct WorkingSpace : IEquatable<WorkingSpace> //: IWorkingSpace
    {
        readonly ChromaticityCoordinates _chromaticity;
        /// <summary>
        /// 
        /// </summary>
        public ChromaticityCoordinates Chromaticity => _chromaticity;

        readonly ICompanding _companding;
        /// <summary>
        /// 
        /// </summary>
        public ICompanding Companding => _companding;

        readonly XYZ _illuminant;
        /// <summary>
        /// 
        /// </summary>
        public XYZ Illuminant => _illuminant;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="illuminant"></param>
        /// <param name="companding"></param>
        /// <param name="chromaticity"></param>
        public WorkingSpace(XYZ illuminant, ICompanding companding, ChromaticityCoordinates chromaticity)
        {
            _illuminant = illuminant;
            _companding = companding;
            _chromaticity = chromaticity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(WorkingSpace left, WorkingSpace right)
        {
            if (ReferenceEquals(left, null))
            {
                if (ReferenceEquals(right, null))
                    return true;

                return false;
            }
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(WorkingSpace left, WorkingSpace right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(WorkingSpace o)
        {
            if (ReferenceEquals(o, null))
                return false;

            if (ReferenceEquals(this, o))
                return true;

            if (GetType() != o.GetType())
                return false;

            return _chromaticity == o._chromaticity && _companding == o._companding && _illuminant == o._illuminant;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((WorkingSpace)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _chromaticity, _companding, _illuminant }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Matrix GetRGBToXYZ(WorkingSpace workingSpace)
        {
            if (workingSpace == null)
                throw new ArgumentNullException(nameof(workingSpace));

            var chromaticity = workingSpace.Chromaticity;
            double xr = chromaticity.R.X, xg = chromaticity.G.X, xb = chromaticity.B.X, yr = chromaticity.R.Y, yg = chromaticity.G.Y, yb = chromaticity.B.Y;

            var Xr = xr / yr;
            const double Yr = 1;
            var Zr = (1 - xr - yr) / yr;

            var Xg = xg / yg;
            const double Yg = 1;
            var Zg = (1 - xg - yg) / yg;

            var Xb = xb / yb;
            const double Yb = 1;
            var Zb = (1 - xb - yb) / yb;

            var S = new Matrix
            (
                new[]
                {
                    new[] { Xr, Xg, Xb },
                    new[] { Yr, Yg, Yb },
                    new[] { Zr, Zg, Zb },
                }
            )
            .Invert3By3();

            var W = workingSpace.Illuminant.Vector;

            var SW = S.Multiply(W);

            double Sr = SW[0];
            double Sg = SW[1];
            double Sb = SW[2];

            return new[]
            {
                new[] { Sr * Xr, Sg * Xg, Sb * Xb },
                new[] { Sr * Yr, Sg * Yg, Sb * Yb },
                new[] { Sr * Zr, Sg * Zg, Sb * Zb },
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Matrix GetXYZToRGB(WorkingSpace workingSpace) => GetRGBToXYZ(workingSpace).Invert3By3();
    }
}
