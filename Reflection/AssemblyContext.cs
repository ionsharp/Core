using System;
using System.Reflection;
using System.Xml.Serialization;

namespace Imagin.Core.Reflection;

/// <summary>Defines facilities for managing an <see cref="System.AppDomain"/>.</summary>
[Serializable]
public class AssemblyContext
{
    /// <summary>A reference to the <see cref="System.AppDomain"/>.</summary>
    [XmlIgnore]
    public AppDomain AppDomain { get; set; } = null;

    /// <summary>
    /// A reference to an <see cref="System.Reflection.Assembly"/>.
    /// </summary>
    [XmlIgnore]
    public Assembly Assembly { get; set; } = null;

    /// <summary>A unique identifier.</summary>
    public Guid Id { get; set; } = default;

    AssemblyContext() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainContext"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="appDomain"></param>
    public AssemblyContext(Guid id, Assembly assembly, AppDomain appDomain)
    {
        Id = id;
        Assembly = assembly;
        AppDomain = appDomain;
    }
}