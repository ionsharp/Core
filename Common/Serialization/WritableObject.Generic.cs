using System;

namespace Imagin.Common.Serialization
{
    /// <summary>
    /// Facilitator for serializing otherwise unserializable objects.
    /// </summary>
    [Serializable]
    public abstract class WritableObject<T> : AbstractObject
    {
        T value = default(T);
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

        public WritableObject() : base()
        {
        }

        public WritableObject(T value) : base()
        {
            Value = value;
        }

        protected virtual T OnPreviewValueChanged(T Value)
        {
            return Value;
        }
    }
}
