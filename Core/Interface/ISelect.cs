using Imagin.Core.Input;
    
namespace Imagin.Core
{
    /// <summary>
    /// Specifies an <see cref="object"/> that can be selected.
    /// </summary>
    public interface ISelect
    {
        event SelectedEventHandler Selected;

        bool IsSelected
        {
            get; set;
        }
    }
}
