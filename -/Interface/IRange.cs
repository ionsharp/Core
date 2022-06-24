namespace Imagin.Core
{
    public interface IRange
    {
        object Increment { get; set; }

        object Maximum { get; set; }

        object Minimum { get; set; }
    }
}