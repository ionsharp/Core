using System;
using System.Xml.Serialization;

namespace Imagin.Core.Analytics
{
    [Serializable]
    public class Warning : Result
    {
        [XmlIgnore]
        public override ResultTypes Type => ResultTypes.Warning;

        protected Warning() : base() { }

        public Warning(object message = null) : base(message) { }
    }
}