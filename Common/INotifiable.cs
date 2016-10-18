using System.Timers;

namespace Imagin.Common
{
    public interface INotifiable
    {
        Timer NotifyTimer
        {
            get; 
        }

        void OnNotified(ElapsedEventArgs e);
    }
}
