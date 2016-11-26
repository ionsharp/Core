using System;
using System.Reflection;
using System.Xml.Serialization;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// Defines necessary utilities to manage an AppDomain.
    /// </summary>
    [Serializable]
    public class AppDomainContext
    {
        [XmlIgnore]
        public Assembly Assembly
        {
            get; set;
        }

        [XmlIgnore]
        public AppDomain Domain
        {
            get; set;
        }

        public Guid Id
        {
            get; set;
        }

        AppDomainContext()
        {
        }

        public AppDomainContext(Guid Id, AppDomain Domain)
        {
            this.Id = Id;
            this.Domain = Domain;
            this.Assembly = Assembly;
        }
    }
}
