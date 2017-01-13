namespace Imagin.Common.Extensions
{
    public static class Int16Extensions
    {
        public static short Coerce(this short Value, short Maximum, short Minimum = 0)
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }
    }
}
