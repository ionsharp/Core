namespace Imagin.Common
{
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
