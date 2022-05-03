using Imagin.Common.Input;

namespace Imagin.Common.Analytics
{
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

    public class ErrorEventArgs : EventArgs<Error>
    {
        public ErrorEventArgs(Error input) : base(input) { }
    }
}