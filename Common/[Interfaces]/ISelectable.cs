using Imagin.Common.Input;
    
namespace Imagin.Common
{
    /// <summary>
    /// 
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
