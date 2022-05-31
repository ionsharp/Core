namespace Imagin.Core
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="ICheck"/> (inherits <see cref="BaseCheckable"/>).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseCheckable<T> : BaseCheckable
    {
        T value;
        public T Value
        {
            get => value;
            set => this.Change(ref this.value, value);
        }

        public override string ToString() => value.ToString();

        public BaseCheckable() : this(false) { }

        public BaseCheckable(T value, bool isChecked = false) : this(isChecked) => Value = value;

        public BaseCheckable(bool isChecked = false) : base(isChecked) { }
    }
}