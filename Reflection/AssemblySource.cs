using System;

namespace Imagin.Core.Reflection;

public enum AssemblySource
{
    /// <summary>The assembly of the method that invoked the currently executing method.</summary>
    Calling,
    /// <summary>The process executable in the default application domain. In other application domains, this is the first executable that was executed by <see cref="AppDomain.ExecuteAssembly(string)"/>.</summary>
    Entry,
    /// <summary>The assembly that contains the code that is currently executing.</summary>
    Executing,
}