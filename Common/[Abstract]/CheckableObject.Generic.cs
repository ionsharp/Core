namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class CheckableObject<TValue> : CheckableObject
    {
        /// <summary>
        /// 
        /// </summary>
        protected TValue value;
        /// <summary>
        /// 
        /// </summary>
        public TValue Value
        {
            get => value;
            set => SetValue(ref this.value, value, () => Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public CheckableObject() : this(false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isChecked"></param>
        public CheckableObject(TValue value, bool isChecked = false) : this(isChecked)
        {
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isChecked"></param>
        public CheckableObject(bool isChecked = false) : base(isChecked)
        {
        }
    }
}
