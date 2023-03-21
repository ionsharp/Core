namespace Imagin.Core.Threading;

public enum TaskManagement
{
    /// <summary>Thread already started; calling method is already running on a new thread. Useful for quick operations.</summary>
    Managed,
    /// <summary>Thread not yet started; calling method is responsible for thread creation. Useful for working around UI operations.</summary>
    Unmanaged,
}