using System;
using System.Xml.Serialization;

namespace Imagin.Common.Analytics
{
    /// <summary>
    /// Represents an unsuccessful <see cref="Result"/> that encapsulates an <see cref="Exception"/>.
    /// </summary>
    [Serializable]
    public class Error : Result
    {
        string fullName;
        [XmlAttribute]
        public string FullName
        {
            get => fullName;
            set => this.Change(ref fullName, value);
        }

        Error inner = null;
        [XmlElement]
        public Error Inner
        {
            get => inner;
            set => this.Change(ref inner, value);
        }

        string name;
        [XmlAttribute]
        public string Name
        {
            get => name;
            set => this.Change(ref name, value);
        }

        string stackTrace;
        [XmlElement]
        public string StackTrace
        {
            get => stackTrace;
            set => this.Change(ref stackTrace, value);
        }

        [XmlIgnore]
        public override ResultTypes Type => ResultTypes.Error;

        public Error() : this(new Exception()) { }

        public Error(object message) : this(new Exception($"{message}")) { }

        public Error(Exception exception) : base(exception.Message) 
        {
            exception = exception ?? new Exception();

            Inner
                = exception.InnerException != null
                ? new Error(exception.InnerException)
                : null;

            Text
                = exception.Message;
            Name
                = exception.GetType().Name;
            FullName
                = exception.GetType().FullName;
            StackTrace
                = exception.StackTrace;
        }
    }
}