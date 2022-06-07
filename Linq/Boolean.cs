using System;

namespace Imagin.Core.Linq;

public static class XBoolean
{
    public static void If(this bool a, bool b, Action @if, Action @else = null)
    {
        if (a == b)
            @if();

        else @else?.Invoke();
    }
}