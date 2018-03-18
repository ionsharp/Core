namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRange
    {
        /// <summary>
        /// 
        /// </summary>
        object Coerce(object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        void SetRange(object minimum, object maximum);
    }
}
