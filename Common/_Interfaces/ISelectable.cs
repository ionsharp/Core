using Imagin.Common.Input;
    
namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that can be selected.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// 
        /// </summary>
        event SelectedEventHandler Selected;

        /// <summary>
        /// 
        /// </summary>
        bool IsSelected
        {
            get; set;
        }
    }
}
