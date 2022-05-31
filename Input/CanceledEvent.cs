namespace Imagin.Core.Input
{
    public delegate void CancelEventHandler<T>(object sender, CancelEventArgs<T> e);

    public class CancelEventArgs<T> : EventArgs<T>
    {
        public bool Cancel { get; set; }

        public CancelEventArgs(T input) : base(input) { }
    }
}