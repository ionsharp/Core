namespace Imagin.Common
{
    /// <summary>
    /// Specifies an object with a checked state.
    /// </summary>
    public interface ICheckable
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
