namespace Imagin.Common
{
    /// <summary>
    /// Represents a failed result.
    /// </summary>
    public class Error : Result
    {
        string message = default(string);
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
                OnPropertyChanged("Message");
            }
        }

        public Error() : base()
        {
        }

        public Error(string Message) : base()
        {
            this.Message = Message;
        }
    }
}
