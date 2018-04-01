using System;

namespace Imagin.Common.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    public static class Angle
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double GetDegree(double radian) => radian * (180.0 / Math.PI);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double GetRadian(double degree) => (Math.PI / 180.0) * degree;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double NormalizeDegree(double degree)
        {
            var result = degree % 360.0;
            return result >= 0 ? result : (result + 360.0);
        }
    }
}