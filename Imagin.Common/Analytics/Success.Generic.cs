using System;
using System.Xml.Serialization;

namespace Imagin.Common.Analytics
{
    [Serializable]
    public class Success<T> : Result
    {
        [XmlIgnore]
        public readonly T Data;

        [XmlIgnore]
        public override ResultTypes Type => ResultTypes.Success;

        protected Success() : base() { }

        public Success(T data) : this() 
        {
            Data = data;
        }
    }
}