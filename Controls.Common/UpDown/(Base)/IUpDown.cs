namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUpDown<T>
    {
        /// <summary>
        /// 
        /// </summary>
        T Maximum
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        T Minimum
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        T Value
        {
            get; set;
        }
    }
}
