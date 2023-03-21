using Imagin.Core.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core.Numerics;

#region NumberType

public static class NumberType
{
    static Dictionary<Type, object> Types = new Dictionary<Type, object>()
    {
        { typeof(Byte),
            new NumberType<Byte>(Byte.MinValue, Byte.MaxValue)
                { FromDouble = x => (Byte)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => (x + y).Byte(), Subtract = (x, y) => (x - y).Byte(), DivideByUInt32 = (x, y) => (x / y).Byte() } },
        { typeof(Decimal),
            new NumberType<Decimal>(Decimal.MinValue, Decimal.MaxValue)
                { FromDouble = x => (Decimal)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => x + y, Subtract = (x, y) => x - y, DivideByUInt32 = (x, y) => x / y } },
        { typeof(Double),
            new NumberType<Double>(Double.MinValue, Double.MaxValue)
                { FromDouble = x => (Double)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => x + y, Subtract = (x, y) => x - y, DivideByUInt32 = (x, y) => x / y } },
        { typeof(Int16),
            new NumberType<Int16>(Int16.MinValue, Int16.MaxValue)
                { FromDouble = x => (Int16)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => (x + y).Int16(), Subtract = (x, y) => (x - y).Int16(), DivideByUInt32 = (x, y) => (x / y).Int16() } },
        { typeof(Int32),
            new NumberType<Int32>(Int32.MinValue, Int32.MaxValue)
                { FromDouble = x => (Int32)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => x + y, Subtract = (x, y) => x - y, DivideByUInt32 = (x, y) => (x / y).Int32() } },
        { typeof(Int64),
            new NumberType<Int64>(Int64.MinValue, Int64.MaxValue)
                { FromDouble = x => (Int64)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => x + y, Subtract = (x, y) => x - y, DivideByUInt32 = (x, y) => x / y } },
        { typeof(Single),
            new NumberType<Single>(Single.MinValue, Single.MaxValue)
                { FromDouble = x => (Single)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => x + y, Subtract = (x, y) => x - y, DivideByUInt32 = (x, y) => x / y } },
        { typeof(UDouble),
            new NumberType<UDouble>(UDouble.MinValue, UDouble.MaxValue)
                { FromDouble = x => (UDouble)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => x + y, Subtract = (x, y) => x - y, DivideByUInt32 = (x, y) => x / y } },
        { typeof(UInt16),
            new NumberType<UInt16>(UInt16.MinValue, UInt16.MaxValue)
                { FromDouble = x => (UInt16)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => (x + y).UInt16(), Subtract = (x, y) => (x - y).UInt16(), DivideByUInt32 = (x, y) => (x / y).UInt16() } },
        { typeof(UInt32),
            new NumberType<UInt32>(UInt32.MinValue, UInt32.MaxValue)
                { FromDouble = x => (UInt32)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => x + y, Subtract = (x, y) => x - y, DivideByUInt32 = (x, y) => x / y } },
        { typeof(UInt64),
            new NumberType<UInt64>(UInt64.MinValue, UInt64.MaxValue)
                { FromDouble = x => (UInt64)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => x + y, Subtract = (x, y) => x - y, DivideByUInt32 = (x, y) => x / y } },
        { typeof(USingle),
            new NumberType<USingle>(USingle.MinValue, USingle.MaxValue)
                { FromDouble = x => (USingle)x, ToDouble = x => x.Double(), GreaterThan = (x, y) => x > y, LessThan = (x, y) => x < y,
                Add = (x, y) => x + y, Subtract = (x, y) => x - y, DivideByUInt32 = (x, y) => x / y } },
    };

    public static NumberType<T> Get<T>() => Types.ContainsKey(typeof(T)) ? Types.First(i => i.Key == typeof(T)).Value as NumberType<T> : throw new NotSupportedException();
}

#endregion

#region NumberType<T>

public class NumberType<T>
{
    public Func<T, T, T> Add { get; internal set; }

    public Func<T, T, T> Subtract { get; internal set; }

    ///

    public Func<T, uint, T> DivideByUInt32 { get; internal set; }

    ///

    public Func<T, T, bool> GreaterThan { get; internal set; }

    public Func<T, T, bool> LessThan { get; internal set; }

    ///

    public Func<double, T> FromDouble { get; internal set; }

    public Func<T, double> ToDouble { get; internal set; }

    ///

    public readonly T Maximum;

    public readonly T Minimum;

    ///

    public NumberType(T minimum, T maximum) : base()
    {
        Minimum = minimum; Maximum = maximum;
    }
}

#endregion