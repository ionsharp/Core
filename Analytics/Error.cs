using Imagin.Core.Linq;
using System;
using System.Xml.Serialization;

namespace Imagin.Core.Analytics;

/// <summary>Represents an unsuccessful <see cref="Result"/> that encapsulates an <see cref="Exception"/>.</summary>
[Serializable]
public class Error : Result
{
    [XmlAttribute]
    public string FullName { get => Get(""); set => Set(value); }

    [XmlElement]
    public Error Inner { get => Get<Error>(); set => Set(value); }

    [XmlAttribute]
    public string Name { get => Get(""); set => Set(value); }

    [XmlElement]
    public string StackTrace { get => Get(""); set => Set(value); }

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
            = exception.GetType().GetAttribute<NameAttribute>()?.Name ?? exception.GetType().Name;
        FullName
            = exception.GetType().FullName;
        StackTrace
            = exception.StackTrace;
    }
}