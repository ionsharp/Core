using System;

namespace Imagin.Core.Conversion;

public interface IConvert
{
    Type InputType { get; }

    Type OutputType { get; }
}

public interface IConvert<A, B>
{
    B ConvertTo(A a);

    A ConvertBack(B b);
}