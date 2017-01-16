namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a result.
    /// </summary>
    public abstract class Result : Result<object>
    {
        public Result() : base()
        {
        }

        public Result(object Data) : base(Data)
        {
        }
    }
}
