using Imagin.Core.Linq;
using System;
using System.Xml.Serialization;

namespace Imagin.Core.Analytics
{
    /// <summary>
    /// Represents a result.
    /// </summary>
    [Serializable]
    public abstract class Result : Base
    {
        string text = string.Empty;
        [XmlElement]
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        [XmlIgnore]
        public abstract ResultTypes Type { get; }

        protected Result() : base() { }

        public Result(object text) : this() => Text = $"{text}";

        public static implicit operator bool(Result a) => a?.Type == ResultTypes.Success;

        public static implicit operator Result(Exception e) => new Error(e);

        public static implicit operator Result(string i) => new Message(i);

        public static implicit operator Result(bool i) => i ? (Result)new Success() : new Error();

        public void If(bool i, Action action) => ((bool)this).If(i, action);

        public override string ToString() => text;
    }
}