namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class UInt16Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static ushort Coerce(this ushort Value, ushort Maximum, ushort Minimum = 0)
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }
    }
}
