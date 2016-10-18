namespace Imagin.Controls.Common
{
    public interface IUpDown<T>
    {
        T Maximum
        {
            get; set;
        }

        T Minimum
        {
            get; set;
        }

        T Value
        {
            get; set;
        }
    }
}
