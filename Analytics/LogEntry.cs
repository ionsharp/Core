using System;
using System.Xml.Serialization;

namespace Imagin.Core.Analytics
{
    [Serializable]
    [XmlType("Entry")]
    public class LogEntry : Base
    {
        DateTime added = DateTime.Now;
        [XmlAttribute]
        public DateTime Added
        {
            get => added;
            set => this.Change(ref added, value);
        }

        ResultLevel level = ResultLevel.Normal;
        [XmlAttribute]
        public ResultLevel Level
        {
            get => level;
            set => this.Change(ref level, value);
        }

        int line = 0;
        [XmlAttribute]
        public int Line
        {
            get => line;
            set => this.Change(ref line, value);
        }

        string member = default;
        [XmlAttribute]
        public string Member
        {
            get => member;
            set => this.Change(ref member, value);
        }

        Result result = default;
        [XmlElement]
        public Result Result
        {
            get => result;
            set => this.Change(ref result, value);
        }

        string sender = default;
        [XmlAttribute]
        public string Sender
        {
            get => sender;
            set => this.Change(ref sender, value);
        }

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
}