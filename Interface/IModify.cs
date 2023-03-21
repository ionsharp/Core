using Imagin.Core.Input;

namespace Imagin.Core;

public interface IModify
{
    event ModifiedEventHandler Modified;
}