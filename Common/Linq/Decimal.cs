namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static decimal Coerce(this decimal Value, decimal Maximum, decimal Minimum = 0m)
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }
    }
}
