using System;

namespace Imagin.Common.Serialization
{
    /// <summary>
    /// Facilitator for serializing otherwise unserializable objects.
    /// </summary>
    [Serializable]
    public abstract class WritableObject<T> : BindableObject
    {
        T value = default(T);
        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = OnPreviewValueChanged(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public WritableObject() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public WritableObject(T value) : base()
        {
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected virtual T OnPreviewValueChanged(T Value)
        {
            return Value;
        }
    }
}
