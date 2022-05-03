using System;

namespace Imagin.Common.Configuration
{
    [Serializable]
    public enum ExitMethod
    {
        None,
        Exit,
        Hibernate,
        Lock,
        LogOff,
        Restart,
        Shutdown,
        Sleep
    }
}