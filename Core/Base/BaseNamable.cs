using Imagin.Core.Input;
using System;

namespace Imagin.Core
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="IName"/>.
    /// </summary>
    [Serializable]
    public class BaseNamable : Base, IName
    {
        [field:NonSerialized]
        public event EventHandler<EventArgs<string>> NameChanged;

        string _name = string.Empty;
        public virtual string Name
        {
            get => _name;
            set
            {
                this.Change(ref _name, OnPreviewNameChanged(_name, value));
                OnNameChanged(_name);
            }
        }

        public BaseNamable() : base() { }

        public BaseNamable(string name) : base() => Name = name;

        protected virtual void OnNameChanged(string Value) => NameChanged?.Invoke(this, new EventArgs<string>(Value));

        protected virtual string OnPreviewNameChanged(string OldValue, string NewValue) => NewValue;

        public override string ToString() => Name;
    }
}
