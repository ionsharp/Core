namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class UDoubleExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static UDouble Coerce(this UDouble value, UDouble maximum, UDouble minimum) => value > maximum ? maximum : (value < minimum ? minimum : value);
    }
}
