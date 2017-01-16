namespace Imagin.Common.Debug
{
    public class Success<T> : Success
    {
        public new T Data
        {
            get; set;
        }

        public Success() : base()
        {
        }

        public Success(T data) : base()
        {
            Data = data;
        }
    }
}
