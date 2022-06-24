using System;

namespace Imagin.Core
{
    [Serializable]
    public class Namable<T> : BaseNamable
    {
        [Featured]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        T value = default;
        public virtual T Value
        {
            get => value;
            set => this.Change(ref this.value, value);
        }

        public Namable() : base() { }

        public Namable(string name) : this() => Name = name;

        public Namable(string name, T value) : this(name) => Value = value;
    }

    [Serializable]
    public class NamableCategory<T> : Namable<T>
    {
        string category = default;
        public string Category
        {
            get => category;
            set => this.Change(ref category, value);
        }

        public NamableCategory() : base() { }

        public NamableCategory(string name, string category = default) : base(name) => Category = category;

        public NamableCategory(string name, string category, T value) : base(name, value) => Category = category;
    }
}