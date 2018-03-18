namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SelectedEventHandler(object sender, SelectedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class SelectedEventArgs : EventArgs<object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public SelectedEventArgs(object value) : base(value) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        public SelectedEventArgs(object value, object parameter) : base(value, parameter) { }
    }
}
