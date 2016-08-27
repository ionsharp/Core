using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common
{
    [Flags]
    public enum Side
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
        All = Top | Bottom | Left | Right
    }
}