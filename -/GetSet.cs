using System;

namespace Imagin.Core
{
    public class GetSet<T> : Base
    {
        public readonly Func<T> Get;

        public readonly Action<T> Set;

        public T Value
        {
            get => Get();
            set
            {
                Set(value);
                OnPropertyChanged(nameof(Value));
            }
        }

        public GetSet(Func<T> get, Action<T> set)
        {
            Get = get; Set = set;
        }
    }

    public class GetSetBoolean : GetSet<bool>
    {
        public GetSetBoolean(Func<bool> get, Action<bool> set) : base(get, set) { }
    }
}