namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents the result of something.
    /// </summary>
    public class Result : AbstractObject
    {
        public object Data
        {
            get; set;
        }

        public Result() : base()
        {
        }

        public Result(object data) : base()
        {
            Data = data;
        }
    }
}
