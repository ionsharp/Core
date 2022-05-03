using System.Collections.Generic;

namespace Imagin.Common.Configuration
{
    public abstract class SingleApplication : BaseApplication, ISingleApplication
    {
        public event ReopenedEventHandler Reopened;

        public SingleApplication() : base() { }

        public virtual void OnReopened(IList<string> arguments)
        {
            MainWindow.Activate();

            if (arguments?.Count > 0)
                arguments.RemoveAt(0);

            Reopened?.Invoke(this, new ReopenedEventArgs(arguments));
        }
    }
}