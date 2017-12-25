namespace Imagin.Common.Input
{
    public interface INotifyPropertyEnabled
    {
        event PropertyEnabledEventHandler PropertyEnabled;

        void OnPropertyEnabled(string PropertyName, bool IsEnabled);
    }
}
