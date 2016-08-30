namespace Imagin.Common
{
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    public class Success : Result
    {
        public object Result
        {
            get; set;
        }

        public Success() : base()
        {
        }

        public Success(object Result) : base()
        {
            this.Result = Result;
        }
    }
}
