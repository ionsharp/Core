namespace Imagin.Core.Input;

public delegate void SelectedEventHandler(object sender, SelectedEventArgs e);

public class SelectedEventArgs : EventArgs<object>
{
    public SelectedEventArgs(object value) : base(value) { }

    public SelectedEventArgs(object value, object parameter) : base(value, parameter) { }
}
