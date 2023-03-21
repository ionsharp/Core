namespace Imagin.Core;

public interface IRange<T>
{
    T Maximum { get; }

    T Minimum { get; }
}