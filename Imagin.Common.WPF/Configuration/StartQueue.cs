using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imagin.Common.Configuration
{
    public class StartQueue : List<StartTask>
    {
        public event EventHandler<EventArgs> Completed;

        public SplashWindow SplashWindow { get; private set; }

        public void Add(bool dispatch, string message, Action action)
        {
            Add(new StartTask(dispatch, message, action));
        }

        async Task Alert(string message, double progress)
        {
            await Dispatch.InvokeAsync(() =>
            {
                SplashWindow.Message = message;
                SplashWindow.Progress = progress;
            });
        }

        public async Task Invoke(SplashWindow splashWindow)
        {
            SplashWindow = splashWindow;
            await Task.Run(async () =>
            {
                double progress = 0;

                double a = 0;
                double b = Count;

                foreach (var i in this)
                {
                    a++;
                    progress = a / b;

                    await Alert(i.Message, progress);
                    Try.Invoke(() =>
                    {
                        if (!i.Dispatch)
                        {
                            i.Action();
                            return;
                        }
                        Dispatch.Invoke(i.Action);
                    },
                    e => Log.Write<StartQueue>(e));
                }
            });
            Completed?.Invoke(this, EventArgs.Empty);
        }
    }
}