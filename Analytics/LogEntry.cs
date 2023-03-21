using System;
using System.Xml.Serialization;

namespace Imagin.Core.Analytics;

[Serializable]
[XmlType("Entry")]
public class LogEntry : Base
{
    [XmlAttribute]
    public DateTime Added { get => Get(DateTime.Now); set => Set(value); }

    [XmlAttribute]
    public ResultLevel Level { get => Get(ResultLevel.Normal); set => Set(value); }

    [XmlAttribute]
    public int Line { get => Get(0); set => Set(value); }

    [XmlAttribute]
    public string Member { get => Get(""); set => Set(value); }

    [XmlElement]
    public Result Result { get => Get<Result>(); set => Set(value); }

    [XmlAttribute]
    public string Sender { get => Get(""); set => Set(value); }

    public LogEntry() : base() { }

    internal LogEntry(ResultLevel level, string sender, Result result, string member, int line)
    {
        Level 
            = level;
        Sender 
            = sender;
        Result 
            = result;
        Member 
            = member;
        Line 
            = line;
    }
}