using System;

namespace Imagin.Common
{
    public interface ILockable
    {
        bool IsLocked
        {
            get; set;
        }
    }
}
