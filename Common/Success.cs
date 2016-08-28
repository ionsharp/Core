namespace Imagin.Common
{
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
