namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    public class Success : Result
    {
        public Success() : base()
        {
        }

        public Success(object Data) : base(Data)
        {
        }
    }
}
