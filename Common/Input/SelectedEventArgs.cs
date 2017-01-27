namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    public delegate void SelectedEventHandler(SelectedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class SelectedEventArgs : EventArgs<object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public SelectedEventArgs(object Value) : base(Value)
        {
        }
    }
}
