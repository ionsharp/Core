using System;

namespace Imagin.Core.Numerics;

public interface IVector 
{
    int Length { get; }

    double[] Values { get; }

    Vector Transform(Func<int, double, double> action);
}