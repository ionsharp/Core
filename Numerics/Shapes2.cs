using System;
using Imagin.Core.Reflection;

namespace Imagin.Core.Numerics;

[Serializable]
public enum Shapes2
{
    [Image("Square.png", AssemblyType.Core)] Square,
    [Image("Circle.png", AssemblyType.Core)] Circle
}