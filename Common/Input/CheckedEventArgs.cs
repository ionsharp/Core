namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CheckedEventHandler(CheckedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class CheckedEventArgs : EventArgs<object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        public CheckedEventArgs(object Data) : base(Data)
        {
        }
    }
}
