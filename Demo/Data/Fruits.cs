using System;

namespace Imagin.NET.Demo
{
    [Flags]
    public enum Fruits
    {
        [Imagin.Common.Browsable(false)]
        None = 0,
        Apple = 1,
        Banana = 2,
        Cherry = 4,
        Grape = 8,
        Kiwi = 16,
        Mango = 32,
        Orange = 64,
        Peach = 128,
        Pear = 256,
        Pineapple = 512,
        [Imagin.Common.Browsable(false)]
        All = Apple | Banana | Cherry | Grape | Kiwi | Mango | Orange | Peach | Pear | Pineapple
    }
}
