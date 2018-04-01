namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotifyPropertyEnabled
    {
        /// <summary>
        /// 
        /// </summary>
        event PropertyEnabledEventHandler PropertyEnabled;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="IsEnabled"></param>
        void OnPropertyEnabled(string PropertyName, bool IsEnabled);
    }
}
