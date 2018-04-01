namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Invert(this bool Value)
        {
            return !Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static short ToInt16(this bool Value)
        {
            return Value ? (short)1 : (short)0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int ToInt32(this bool Value)
        {
            return Value ? 1 : 0;
        }
    }
}
