using System;

namespace Imagin.Core.Serialization
{
    [Serializable]
    public class Enum<T> : Base
    {
        string value;
        public T Value
        {
            get => (T)Enum.Parse(typeof(T), value);
            set => this.Change(ref this.value, $"{value}");
        }

        public Enum() : base() { }
    }
}