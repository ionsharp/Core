namespace Imagin.Common
{
    public interface ICoercable<T>
    {
        T Minimum
        {
            get; set;
        }

        T Maximum
        {
            get; set;
        }
    }
}
