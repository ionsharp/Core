using System;

namespace Imagin.Common.Serialization
{
    /// <summary>
    /// Facilitates with serializing standard .NET enums.
    /// </summary>
    [Serializable]
    public class WritableEnum : WritableObject<string>
    {
        string type = string.Empty;
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public WritableEnum() : base()
        {
        }

        public WritableEnum(string Type, string Field) : base(Field)
        {
            this.Type = Type;
        }

        public WritableEnum(Enum Field) : this(Field.GetType().ToString(), Enum.GetName(Field.GetType(), Field))
        {
        }

        public static implicit operator WritableEnum(Enum Field)
        {
            return new WritableEnum(Field);
        }
    }
}
