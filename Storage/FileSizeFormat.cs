using System;

namespace Imagin.Core.Storage;

[Serializable]
public enum FileSizeFormat
{
    /// <summary>Specifies number of bytes with no base quantity.</summary><returns>Total number of bytes.</returns>
    Bytes = 0,
    /// <summary>Specifies number of bytes with a base quantity of 1024</summary><remarks>B, KiB, MiB, GiB, TiB, PiB, EiB, ZiB, YiB</remarks>
    IECBinary,
    /// <summary>Specifies number of bytes with a base quantity of 1000</summary><remarks>B, kB, MB, GB, TB, PB, EB, ZB, YB</remarks>
    BinaryUsingSI,
    /// <summary>Specifies number of bytes with a base quantity of 1000</summary><remarks>B, kB, MB, GB, TB, PB, EB, ZB, YB</remarks>
    DecimalUsingSI
}