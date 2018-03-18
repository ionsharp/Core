namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="ICheckable"/> (inherits <see cref="CheckableObject"/>).
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class CheckableObject<TValue> : CheckableObject
    {
        /// <summary>
        /// 
        /// </summary>
        TValue _value;
        /// <summary>
        /// 
        /// </summary>
        public TValue Value
        {
            get => _value;
            set => Property.Set(this, ref _value, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _value.ToString();

        /// <summary>
        /// 
        /// </summary>
        public CheckableObject() : this(false) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isChecked"></param>
        public CheckableObject(TValue value, bool isChecked = false) : this(isChecked) => Value = value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isChecked"></param>
        public CheckableObject(bool isChecked = false) : base(isChecked) { }
    }
}
