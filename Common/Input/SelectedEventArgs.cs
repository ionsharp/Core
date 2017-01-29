namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
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
        /// <param name="Data"></param>
        public SelectedEventArgs(object Data) : base(Data)
        {
        }
    }
}
