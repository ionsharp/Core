using Imagin.Core.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Core;

public class Instances<T>
{
    readonly Dictionary<Type, T> instances = new();

    public T this[Type input] => (T)instances.GetOrAdd(input, () => input.Create<T>());
}

public class Instances : Instances<object> { }