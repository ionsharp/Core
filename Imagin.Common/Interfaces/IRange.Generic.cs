namespace Imagin.Common
{
    public interface IRange<T>
    {
        T Maximum
        {
            get; set;
        }

        T Minimum
        {
            get; set;
        }
    }
}