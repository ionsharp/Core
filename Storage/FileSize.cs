using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Storage;

[Serializable]
public struct FileSize : IEquatable<FileSize>
{
    /// <summary>Specifies the largest possible value (<see cref="ulong.MaxValue"/>).</summary>
    public readonly static double MaxValue = ulong.MaxValue;

    /// <summary>Specifies the smallest possible value (<see cref="ulong.MinValue"/>).</summary>
    public readonly static double MinValue = ulong.MinValue;

    ///

    public ulong Value { get; private set; }

    ///

    public FileSize(long input) => Value = input.UInt64();

    public FileSize(ulong input) => Value = input;

    ///

    public static bool operator ==(FileSize a, FileSize b) => a.EqualsOverload(b);

    public static bool operator !=(FileSize a, FileSize b) => !(a == b);

    ///

    public bool Equals(FileSize i)
        => this.Equals<FileSize>(i) && Value.Equals(i.Value);

    public override bool Equals(object i)
        => Equals((FileSize)i);

    public override int GetHashCode()
        => Value.GetHashCode();

    ///

    public override string ToString() => ToString(FileSizeFormat.BinaryUsingSI, 1);

    public string ToString(FileSizeFormat format, int round = 1)
    {
        if (format == FileSizeFormat.Bytes)
            return Value.ToString();

        var Labels = new string[]
        {
            "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"
        };

        if (format == FileSizeFormat.BinaryUsingSI || format == FileSizeFormat.DecimalUsingSI)
        {
            Labels = new string[]
            {
            "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
            };
        }

        if (Value == 0)
            return "0 B";

        var f = format == FileSizeFormat.BinaryUsingSI || format == FileSizeFormat.IECBinary ? (ulong)1024 : 1000;

        var m = (int)Math.Log(Value, f);
        var a = (decimal)Value / (1L << (m * 10));

        if (Math.Round(a, round) >= 1000)
        {
            m += 1;
            a /= f;
        }

        var result = string.Format("{0:n" + round + "}", a);

        var j = result.Length;
        for (var i = result.Length - 1; i >= 0; i--)
        {
            if (result[i] == '.')
            {
                j--;
                break;
            }
            if (result[i] == '0')
            {
                j--;
            }
            else break;
        }

        return $"{result.Substring(0, j)} {Labels[m]}"; ;
    }
}