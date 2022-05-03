namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> with a checkable state.
    /// </summary>
    public interface ICheck
    {
        /// <summary>
        /// 
        /// </summary>
        bool? IsChecked
        {
            get; set;
        }
    }
}
