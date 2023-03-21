using System;
using System.Xml.Serialization;

namespace Imagin.Core.Analytics;

[Serializable]
public class Message : Result
{
    [XmlIgnore]
    public override ResultTypes Type => ResultTypes.Message;

    public Message() : base() { }

    public Message(object text = null) : base(text) { }
}