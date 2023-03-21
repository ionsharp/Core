using Imagin.Core.Input;

namespace Imagin.Core.Analytics;

public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

public class ErrorEventArgs : EventArgs<Error>
{
    public ErrorEventArgs(Error input) : base(input) { }
}